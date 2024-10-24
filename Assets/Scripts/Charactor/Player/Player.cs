using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Player : Charactor {
    #region 参数
    [Header("________ IDLE ________")]
    [SerializeField] Transform footPos;
    [SerializeField] Vector2 footOffset;

    [Header("________ WALK ________")]
    [SerializeField] float walkSpeed;
    [SerializeField] StepAudio currentAudio;
    [SerializeField] AudioData[] OnShuYeAudio;
    [SerializeField] AudioData[] OnHardGroundAudio;
    [SerializeField] AudioData[] InWaterAudio;

    [Header("________ Run ________")]
    [SerializeField] float runSpeed;

    [Header("________ JUMP ________")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] AudioData[] jumpAudio;
    [SerializeField] AudioData[] touchGroundAudio;

    [Header("________ ATTACK ________")]
    [SerializeField] float moveSpeedWhenAttack;
    //[SerializeField] float attackDamage;
    public Transform[] AttackRange;
    //[SerializeField] float attackRange;
    //[SerializeField] Vector2 attackOffset;
    [SerializeField] AudioData[] attackAudio;
    [SerializeField] AudioData[] sheathAudio;

    [Header("________ SKILL _________")]
    [SerializeField] AudioData[] skillAudio;

    [Header("________ HURT _________")]
    [SerializeField] AudioData[] hurtAudio;

    [Header("________ DEAD _________")]
    [SerializeField] AudioData[] deadAudio;

    [Header("________ 其它 _________")]
    [SerializeField] LayerMask terrianLayer;
    [SerializeField] LayerMask EnemyLayer;
    public Transform viewLight;
    public MyEventDispatcher underView;

    [Header("________ Cast ________")]
    public GameObject kunai;
    public int kunaiNum = 10;
    [SerializeField] TextMeshProUGUI kunaiNumUI;
    [SerializeField] AudioData[] castAudio;

    [Header("________ Hide _________")]
    public bool isHide;
    public int foundedNum;
    #endregion

    public float MoveSpeedWhenAttack { get => moveSpeedWhenAttack; }
    public float WalkSpeed { get => walkSpeed;}
    public float RunSpeed { get => runSpeed;}
    public float JumpForce { get => jumpForce; }
    public float JumpSpeed { get => jumpSpeed; }
    //public float AttackDamage { get => attackDamage;}

    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // 注意状态机部分状态类需要eventDispatcher.
        eventDispatcher = new AnimationEventDispatcher();
        RegisterFSM();
        // HealthCOntroller需要状态机
        healthController = GetComponent<HealthController>();
        healthController.Init(this);

        underView = new MyEventDispatcher();
        sr = GetComponent<SpriteRenderer>();
        isHide = false;

        UpdateUI();
    }

    void Update()
    {
        //Debug.Log("isOnGround:" + CheckOnGround());
        m_FSM.Execute();
        Recover();
    }
    
    // 缓慢回血
    private void Recover()
    {
        healthController.Recover(1 * Time.deltaTime);
    }

    public void CheckOriented()
    {
        if (rg.velocity.x > 0.001f && transform.localScale.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rg.velocity.x < -0.001f && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public bool CheckOnGround()
    {
        bool isOnGround = false;
        float rayDistance = 0.2f;
        Vector2 origin = new Vector2(footPos.position.x, footPos.position.y);
        Vector2 rayDir = Vector2.down;
        if (Physics2D.Raycast(origin + footOffset, rayDir, rayDistance, terrianLayer))
        {
            Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.red);//起点，方向，颜色（可选）//线的长度与射线相同
            isOnGround = true;
        }
        else
        {
            Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.green);
        }

        if (Physics2D.Raycast(origin - footOffset, rayDir, rayDistance, terrianLayer))
        {
            Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.red);//起点，方向，颜色（可选）//线的长度与射线相同
            isOnGround = true;
        }
        else
        {
            Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.green);
        }
        return isOnGround;
    }

    public void CheckHit(int index)
    {
        AttackRange[index].gameObject.SetActive(true);
    }

    public bool isCheckHit(int index)
    {
        return AttackRange[index].gameObject.activeSelf;
    }

    public void StopCheckHit(int index)
    {
        AttackRange[index].gameObject.SetActive(false);
    }

    public bool CheckFall()
    {
        if (!CheckOnGround() && rg.velocity.y < 0)
        {
            return true;
        }
        return false;
    }

    // ———————————————————————— 动作事件 ————————————————————————
    public void AnimationEventCallback(AnimationStage stage)
    {
        eventDispatcher.TriggerAnimationEvent(stage);
    }

    // ———————————————————————— 音频事件 ————————————————————————
    public void AudioCallBack(actionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource, type switch
        {
            actionAudio.walk => FootStepAudio(),
            actionAudio.attack1 => attackAudio,
            actionAudio.sheath => sheathAudio,
            actionAudio.skill => skillAudio,
            actionAudio.shoot => castAudio,
            actionAudio.jump => jumpAudio,
            actionAudio.touchground => touchGroundAudio,
            actionAudio.hurt => hurtAudio,
            actionAudio.dead => deadAudio,
            // 添加其他音频类型的处理
            _ => null
        });
    }

    public void SetStepAudio(StepAudio stepAudio)
    {
        currentAudio = stepAudio;
    }

    AudioData[] FootStepAudio()
    {
        return currentAudio switch
        {
            StepAudio.Shuye => OnShuYeAudio,
            StepAudio.HardGround => OnHardGroundAudio,
            StepAudio.Water => InWaterAudio,
            _ => null,
        };
    }

    public void SetState(actionType actionType)
    {
        m_FSM.TransitionStatus(actionType);
    }

    private void RegisterFSM()
    {
        m_FSM = new Fsm();
        m_FSM.Init(this, actionType.idle, new PlayerIdle(this));
        m_FSM.AddAction(actionType.walk, new PlayerWalk(this));
        m_FSM.AddAction(actionType.run, new PlayerRun(this));
        m_FSM.AddAction(actionType.jump, new PlayerJump(this));
        m_FSM.AddAction(actionType.fall, new PlayerFall(this));
        m_FSM.AddAction(actionType.touchGround, new PlayerTouchGround(this));
        m_FSM.AddAction(actionType.attack1, new PlayerAttack1(this));
        m_FSM.AddAction(actionType.attack2, new PlayerAttack2(this));
        m_FSM.AddAction(actionType.shoot, new PlayerShoot(this)); //远程攻击;
        m_FSM.AddAction(actionType.skill, new PlayerSkill(this));
        m_FSM.AddAction(actionType.hurt, new  PlayerHurt(this));
        m_FSM.AddAction(actionType.dead, new PlayerDie(this));
    }

    /// <summary>
    /// 冻结自身，取消碰撞
    /// </summary>
    public void DestroySelf()
    {
        //rg.constraints = RigidbodyConstraints2D.FreezePosition;
        rg.constraints = RigidbodyConstraints2D.FreezeAll;
        CapsuleCollider2D caps = GetComponent<CapsuleCollider2D>();
        caps.enabled = false;
        GameManager.GameOver();
    }

    public void UpdateUI()
    {
        kunaiNumUI.text = kunaiNum.ToString();
    }

    /// <summary>
    /// 尝试进入隐藏状态
    /// </summary>
    /// <returns></returns>
    public bool TryHide()
    {
        //没有人注意，则可以进入躲藏
        //if(foundedNum != 0)
        //    return false;
        if(foundedNum != 0)
        {
            //Debug.Log($"{DateTime.Now} 当前是有观察者{foundedNum}, 不能进入隐藏");
            return false;
        }
        else
        {
            //Debug.Log($"{DateTime.Now} 当前是有观察者{foundedNum}，进入隐藏");
        }

        isHide = true;
        sr.color = Color.black;
        ChangeHideEffect();

        return isHide;
    }

    /// <summary>
    /// 尝试取消隐藏状态,刷新Player的碰撞体
    /// </summary>
    /// <returns></returns>
    public void CancelHideStateWithRefresh()
    {
        if(!isHide) return;

        isHide = false;
        sr.color = Color.white;

        InformAllSubscribe();
        ChangeHideEffect();
    }

    // /// <summary>
    // /// 尝试取消隐藏状态,但不刷新Player的碰撞体
    // /// </summary>
    // /// <returns></returns>
    //public void CancelHideState()
    //{
    //    if (!isHide) return;

    //    isHide = false;
    //    sr.color = Color.white;
    //    ChangeHideEffect();
    //}

    /// <summary>
    /// 返回隐藏状态
    /// </summary>
    /// <returns></returns>
    public bool GetHideState()
    {
        return isHide;
    }

    /// <summary>
    /// 注册观察者
    /// </summary>
    public void RegisterFounded()
    {
        foundedNum++;
        //Debug.Log($"Found {DateTime.Now} : {foundedNum}");
    }

    public void InformMiss()
    {
        foundedNum--;
        //Debug.Log($"Miss {DateTime.Now} : {foundedNum}");
    }

    public void InformAllSubscribe()
    {
        underView.TriggerInform();
    }

    private void ChangeHideEffect()
    {
        VolumeController.Instance.ChangeHidenEffect();
    }

}
