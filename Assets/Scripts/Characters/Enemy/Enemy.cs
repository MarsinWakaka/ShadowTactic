using System;
using UnityEngine;
using Universal.AudioSystem;
using Universal.HealthSystem;

namespace Characters.Enemy
{
    public abstract class Enemy : BaseCharacter
    {
        #region ����
        [Header("_________ �ȴ� _________")]
        [SerializeField] protected float waitTime_Idle;

        [Header("_________ Ѳ�� _________")]
        [SerializeField] protected float waitTime_patrol;
        [SerializeField] protected float moveSpeedWhenAttack;
        [SerializeField] protected float walkSpeed;
        [SerializeField] protected AudioData[] patrolAudio;

        [Header("_________ ׷�� _________")]
        [SerializeField] protected float chaseSpeed;
        [SerializeField] protected float maxChaseTime;
        [SerializeField] protected AudioData[] chaseAudio;

        [Header("_________ ��ս _________")]
        [SerializeField] protected float attackForce;
        public float strikingDistance; //��ս������Χ
        [SerializeField] LayerMask PlayerLayer;
        private Vector2 attackOffset;
        private int attackIndexMax;
        public Transform attackPos;
        [SerializeField] protected AudioData[] attackAudio;

        [Header("_________ ���� _________")]
        //[HideInInspector]
        public GameObject Target;
        [SerializeField] protected AudioData[] hurtAudio;

        [Header("_________ ���� _________")]
        [SerializeField] protected AudioData[] deadAudio;

        [Header("_________ ��������趨 _________")]
        public EnemyViewer viewer;

        [Header("����")]
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


            EventDispatcher = new AnimationEventDispatcher();
            RegisterFSM();

            healthController = GetComponent<HealthController>();
            healthController.Init(this);

            viewer.Register(this);

            UnityEngine.Random.InitState((int)Time.realtimeSinceStartup);

            //�����ĳ�ʼ��
            attackOffset = new Vector2(attackPos.localPosition.x, attackPos.localPosition.y);

        }

        protected virtual void Update()
        {
            m_FSM.Execute();
        }

        protected abstract void RegisterFSM();

        /// <summary>
        /// ���������¼��Ļص���ʹ�����¼�֪ͨ����״̬��
        /// </summary>
        /// <param name="stage"></param>
        public void AnimationEventCallback(AnimationStage stage)
        {
            EventDispatcher.TriggerAnimationEvent(stage);
        }

        /// <summary>
        /// ǿ���趨״̬
        /// ���׳�����
        /// </summary>
        /// <param name="actionType"></param>
        public void SetState(actionType actionType)
        {
            m_FSM.TransitionStatus(actionType);
        }

        /// <summary>
        /// ���ݹ������뷢������
        /// ���������Attack Force�˺�
        /// </summary>
        public bool CheckHit()
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y) + new Vector2(attackOffset.x * transform.localScale.x, attackOffset.y);
            Vector2 rayDir = Vector2.right * (transform.localScale.x * strikingDistance); //�����˳���
            RaycastHit2D rayCastHit2D = Physics2D.Raycast(origin, rayDir, strikingDistance, PlayerLayer);
            if (rayCastHit2D)
            {
                var hc = rayCastHit2D.transform.gameObject.GetComponent<HealthController>();
                if (!hc.TryTakeDamage(attackForce))
                {
                    m_FSM.TransitionStatus(actionType.stiff);
                }
                // Debug.DrawRay(origin, rayDir, Color.red, 0.5f);
                return true;
            }
            else
            {
                // Debug.DrawRay(origin, rayDir, Color.green, 0.5f);
                return false;
            }
        }

        /// <summary>
        /// ����Ŀ�꣬��Ŀ��Ϊ��ֱ�ӽ���
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
        /// ������Ŀ��ľ���(>0),��������Ŀ���������
        /// </summary>
        /// <returns></returns>
        public float distFromTarget()
        {
            if (Target == null)
                return float.MaxValue;

            return Mathf.Abs(Target.transform.position.x - transform.position.x);
        }

        /// <summary>
        /// �����ù�������
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
        /// ���ݳ���ָ����ˮƽ�ٶȣ����ı䴹ֱ������ٶ�
        /// </summary>
        /// <param name="xAxis"></param>
        public void Move(float xAxis)
        {
            rg.velocity = new Vector2(xAxis * transform.localScale.x, rg.velocity.y);
        }

        /// <summary>
        /// ���������Ĳ���
        /// </summary>
        /// <param name="type"></param>
        public virtual void AudioCallBack(ActionAudio type)
        {
            SoundManager.Instance.PlaySFX(audioSource ,type switch
            {
                ActionAudio.walk => patrolAudio,
                ActionAudio.run => chaseAudio,
                ActionAudio.attack1 => attackAudio,
                ActionAudio.hurt => hurtAudio,
                ActionAudio.dead => deadAudio,
                // ���������Ƶ���͵Ĵ���
                _ => null
            });
        }

        /// <summary>
        /// ��������ȡ����ײ��5�����������
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
}
