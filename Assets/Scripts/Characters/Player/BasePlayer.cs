using UnityEngine;
using Universal.AudioSystem;
using Universal.HealthSystem;
using Utilities;

namespace Characters.Player
{
    public class BasePlayer : BaseCharacter {
        #region ����
        
        [Header("[��ɫ����]")]
        [SerializeField] protected float walkSpeed;
        [SerializeField] protected float runSpeed;
        [SerializeField] protected float moveSpeedWhenAttack;
        [SerializeField] protected float jumpSpeed;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected FootStepAudioType currentAudioType;
        
        [Header("[��ɫ����]")]
        public Transform[] attackCollider;
        public Transform viewLight;
        [SerializeField] protected Transform footPosition;
        [SerializeField] protected Vector2 footOffset;
        [SerializeField] protected LayerMask terrainLayer;
        [SerializeField] protected LayerMask enemyLayer;
        [Header("[��Ƶ����]")]
        [SerializeField] protected AudioData[] onLeavesAudio;
        [SerializeField] protected AudioData[] onHardSurfaceAudio;
        [SerializeField] protected AudioData[] stepForwardInWaterAudio;
        [SerializeField] protected AudioData[] jumpAudio;
        [SerializeField] protected AudioData[] touchGroundAudio;
        [SerializeField] protected AudioData[] attackAudio;
        [SerializeField] protected AudioData[] sheathAudio;
        [SerializeField] protected AudioData[] skillAudio;
        [SerializeField] protected AudioData[] hurtAudio;
        [SerializeField] protected AudioData[] deadAudio;
        
        public MyEventDispatcher UnderView;
        
        [Header("�������")]
        private bool _isHide;
        private int _observerNumber;
        #endregion

        public float MoveSpeedWhenAttack { get => moveSpeedWhenAttack; }
        public float WalkSpeed { get => walkSpeed;}
        public float RunSpeed { get => runSpeed;}
        public float JumpForce { get => jumpForce; }
        public float JumpSpeed { get => jumpSpeed; }
        //public float AttackDamage { get => attackDamage;}

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            rg = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();

            // ע��״̬������״̬����ҪeventDispatcher.
            EventDispatcher = new AnimationEventDispatcher();
            RegisterFsm();
            // HealthController��Ҫ״̬��
            healthController = GetComponent<HealthController>();
            healthController.Init(this);

            UnderView = new MyEventDispatcher();
            sr = GetComponent<SpriteRenderer>();
            _isHide = false;
        }

        void Update()
        {
            //Debug.Log("isOnGround:" + CheckOnGround());
            m_FSM.Execute();
            Recover();
        }
        private bool IsDead{ get; set; }

        public void SetCharacterDie()
        {
            if (IsDead) return;
            Destroy(viewLight.gameObject);
            IsDead = true;
        }
        
        // ������Ѫ
        private void Recover()
        {
            if (IsDead) return;
            healthController.Recover(2 * Time.deltaTime);
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
            Vector2 origin = new Vector2(footPosition.position.x, footPosition.position.y);
            Vector2 rayDir = Vector2.down;
            if (Physics2D.Raycast(origin + footOffset, rayDir, rayDistance, terrainLayer))
            {
#if UNITY_EDITOR
                Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.red);//��㣬������ɫ����ѡ��//�ߵĳ�����������ͬ
#endif
                isOnGround = true;
            }
#if UNITY_EDITOR
            else
            {
                Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.green);
            }
#endif
            if (Physics2D.Raycast(origin - footOffset, rayDir, rayDistance, terrainLayer))
            {
#if UNITY_EDITOR
                Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.red);//��㣬������ɫ����ѡ��//�ߵĳ�����������ͬ
#endif
                isOnGround = true;
            }
#if UNITY_EDITOR
            else
            {
                Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.green);
            }
#endif
            return isOnGround;
        }

        public void OpenAttackCheck(int index)
        {
            if (attackCollider[index].gameObject.activeSelf) return;
            attackCollider[index].gameObject.SetActive(true);
        }

        public bool IsAttacking(int index)
        {
            return attackCollider[index].gameObject.activeSelf;
        }

        public void StopAttack(int index)
        {
            attackCollider[index].gameObject.SetActive(false);
        }

        public bool CheckFall()
        {
            if (!CheckOnGround() && rg.velocity.y < 0)
            {
                return true;
            }
            return false;
        }

        // ������������������������������������������������ �����¼� ������������������������������������������������
        public void AnimationEventCallback(AnimationStage stage)
        {
            EventDispatcher.TriggerAnimationEvent(stage);
        }

        // ������������������������������������������������ ��Ƶ�¼� ������������������������������������������������
        public virtual void AudioCallBack(ActionAudio type)
        {
            SoundManager.Instance.PlaySFX(audioSource, type switch
            {
                ActionAudio.walk => FootStepAudio(),
                ActionAudio.attack1 => attackAudio,
                ActionAudio.sheath => sheathAudio,
                ActionAudio.skill => skillAudio,
                ActionAudio.jump => jumpAudio,
                ActionAudio.touchground => touchGroundAudio,
                ActionAudio.hurt => hurtAudio,
                ActionAudio.dead => deadAudio,
                // ���������Ƶ���͵Ĵ���
                _ => null
            });
        }

        public void SetStepAudio(FootStepAudioType footStepAudioType)
        {
            currentAudioType = footStepAudioType;
        }

        protected AudioData[] FootStepAudio()
        {
            return currentAudioType switch
            {
                FootStepAudioType.Leaves => onLeavesAudio,
                FootStepAudioType.HardSurface => onHardSurfaceAudio,
                FootStepAudioType.Water => stepForwardInWaterAudio,
                _ => null,
            };
        }

        public void SetState(actionType actionType)
        {
            m_FSM.TransitionStatus(actionType);
        }

        protected virtual void RegisterFsm()
        {
        }

        /// <summary>
        /// ��������ȡ����ײ
        /// </summary>
        public void DestroySelf()
        {
            rg.constraints = RigidbodyConstraints2D.FreezeAll;
            CapsuleCollider2D caps = GetComponent<CapsuleCollider2D>();
            caps.enabled = false;
            GameManager.GameOver();
        }

        /// <summary>
        /// ���Խ�������״̬
        /// </summary>
        /// <returns></returns>
        public bool TryHide()
        {
            //û����ע�⣬����Խ�����
            if(_observerNumber != 0)
            {
                //Debug.Log($"{DateTime.Now} ��ǰ���й۲���{foundedNum}, ���ܽ�������");
                return false;
            }
            else
            {
                //Debug.Log($"{DateTime.Now} ��ǰ���й۲���{foundedNum}����������");
            }
            _isHide = true;
            sr.color = Color.black;
            ChangeHideEffect();

            return _isHide;
        }

        /// <summary>
        /// ����ȡ������״̬,ˢ��Player����ײ��
        /// </summary>
        /// <returns></returns>
        public void CancelHideStateWithRefresh()
        {
            if(!_isHide) return;

            _isHide = false;
            sr.color = Color.white;

            InformAllSubscribe();
            ChangeHideEffect();
        }

        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <returns></returns>
        public bool GetHideState()
        {
            return _isHide;
        }

        /// <summary>
        /// ע��۲���
        /// </summary>
        public void RegisterFounded()
        {
            _observerNumber++;
            //Debug.Log($"Found {DateTime.Now} : {foundedNum}");
        }

        public void InformMiss()
        {
            _observerNumber--;
            //Debug.Log($"Miss {DateTime.Now} : {foundedNum}");
        }

        private void InformAllSubscribe()
        {
            UnderView.TriggerInform();
        }

        private void ChangeHideEffect()
        {
            VolumeController.Instance.ChangeHidenEffect();
        }
    }
}
