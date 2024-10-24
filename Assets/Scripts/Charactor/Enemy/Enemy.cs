using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Charactor
{
    #region 参数
    [Header("_________ 等待 _________")]
    [SerializeField] protected float waitTime_Idle;

    [Header("_________ 巡逻 _________")]
    [SerializeField] protected float waitTime_patrol;
    [SerializeField] protected float moveSpeedWhenAttack;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected AudioData[] patrolAudio;

    [Header("_________ 追逐 _________")]
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float maxChaseTime;
    [SerializeField] protected AudioData[] chaseAudio;

    [Header("_________ 近战 _________")]
    [SerializeField] protected float attackForce;
    public float strikingDistance; //近战攻击范围
    [SerializeField] LayerMask PlayerLayer;
    private Vector2 attackOffset;
    private int attackIndexMax;
    public Transform attackPos;
    [SerializeField] protected AudioData[] attackAudio;

    [Header("_________ 受伤 _________")]
    //[HideInInspector]
    public GameObject Target;
    [SerializeField] protected AudioData[] hurtAudio;

    [Header("_________ 死亡 _________")]
    [SerializeField] protected AudioData[] deadAudio;

    [Header("_________ 外置组件设定 _________")]
    public EnemyViewer viewer;

    [Header("其它")]
    public LayerMask terrianLayer;
    #endregion

    public float MoveSpeedWhenAttacked { get => moveSpeedWhenAttack; }
    public float WalkSpeed { get => walkSpeed; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float PatrolTime { get => waitTime_patrol; set => waitTime_patrol = value; }
    public float WaitTime { get => waitTime_Idle; set => waitTime_Idle = value; }
    public int AttackIndexMax { get => attackIndexMax; set => attackIndexMax = value; }
    public float MaxChaseTime { get => maxChaseTime; set => maxChaseTime = value; }

    protected void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();


        eventDispatcher = new AnimationEventDispatcher();
        RegisterFSM();

        healthController = GetComponent<HealthController>();
        healthController.Init(this);

        viewer.Register(this);

        UnityEngine.Random.InitState((int)Time.realtimeSinceStartup);

        //参数的初始化
        attackOffset = new Vector2(attackPos.localPosition.x, attackPos.localPosition.y);

    }

    protected virtual void Update()
    {
        m_FSM.Execute();
    }

    protected abstract void RegisterFSM();

    /// <summary>
    /// 用作动画事件的回调，使动画事件通知各个状态类
    /// </summary>
    /// <param name="stage"></param>
    public void AnimationEventCallback(AnimationStage stage)
    {
        eventDispatcher.TriggerAnimationEvent(stage);
    }

    /// <summary>
    /// 强制设定状态
    /// 容易出问题
    /// </summary>
    /// <param name="actionType"></param>
    public void SetState(actionType actionType)
    {
        m_FSM.TransitionStatus(actionType);
    }

    /// <summary>
    /// 根据攻击距离发射射线
    /// 击中则造成Attack Force伤害
    /// </summary>
    public bool CheckHit()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y) + new Vector2(attackOffset.x * transform.localScale.x, attackOffset.y);
        Vector2 rayDir = Vector2.right * transform.localScale.x * strikingDistance; //包含了长度
        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, rayDir, strikingDistance, PlayerLayer);
        if (raycastHit2D)
        {
            var hc = raycastHit2D.transform.gameObject.GetComponent<HealthController>();
            hc.Hurt(attackForce);

            Debug.DrawRay(origin, rayDir, Color.red, 0.5f);
            //Debug.Log($"{this.ToString()}击中了{raycastHit2D.collider.gameObject.ToString()}");

            return true;
        }
        else
        {
            Debug.DrawRay(origin, rayDir, Color.green, 0.5f);
            //Debug.Log($"{this.ToString()}挥空了");

            return false;
        }
    }

    /// <summary>
    /// 朝向目标，若目标为空直接结束
    /// </summary>
    public void CheckOriented()
    {
        if (Target != null)
        {
            int facialDir = Target.transform.position.x - transform.position.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(facialDir * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        } 
    }

    public void ChangeOriented()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }

    /// <summary>
    /// 返回与目标的距离(>0),若不存在目标则无穷大
    /// </summary>
    /// <returns></returns>
    public float distFromTarget()
    {
        if (Target == null)
            return float.MaxValue;

        return Mathf.Abs(Target.transform.position.x - transform.position.x);
    }

    /// <summary>
    /// 随机获得攻击索引
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public actionType GetAttackIndex()
        => UnityEngine.Random.Range(1, AttackIndexMax) switch
        {
            1 => actionType.attack1,
            2 => actionType.attack2,
            3 => actionType.attack3,
            _ => throw new ArgumentException(message: "invalid enum value"),
        };

    /// <summary>
    /// 根据朝向，指定的水平速度，不改变垂直方向的速度
    /// </summary>
    /// <param name="xAxis"></param>
    public void Move(float xAxis)
    {
        rg.velocity = new Vector2(xAxis * transform.localScale.x, rg.velocity.y);
    }

    /// <summary>
    /// 控制声音的播放
    /// </summary>
    /// <param name="type"></param>
    public virtual void AudioCallBack(actionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource ,type switch
        {
            actionAudio.walk => patrolAudio,
            actionAudio.run => chaseAudio,
            actionAudio.attack1 => attackAudio,
            actionAudio.hurt => hurtAudio,
            actionAudio.dead => deadAudio,
            // 添加其他音频类型的处理
            _ => null
        });
    }

    /// <summary>
    /// 冻结自身，取消碰撞，5秒后销毁自身
    /// </summary>
    public void DestorySelf()
    {
        rg.constraints = RigidbodyConstraints2D.FreezePosition;
        CapsuleCollider2D caps = GetComponent<CapsuleCollider2D>();
        caps.enabled = false;
        anim.enabled = false;
        Destroy(this.gameObject, 5f);
    }
}
