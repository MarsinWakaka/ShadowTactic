using UnityEngine;

namespace Characters.Player.MonkNinja
{
    public class MonkNinjaShoot : IState
    {
        private readonly MonkNinja _owner;
        private readonly Fsm _mFsm;
        readonly AnimationEventDispatcher _eventDispatcher;

        private readonly GameObject _kunai;
        private bool _isOver;
        private bool _isReady;
        private bool _isPressShoot;
        private readonly float _moveSpeedWhenAttack;

        //�ֶ�

        public MonkNinjaShoot(MonkNinja owner)
        {
            _owner = owner;
            
            _mFsm = owner.m_FSM;
            _eventDispatcher = owner.EventDispatcher;
            _kunai = owner.kunai;
            _isOver = false;
            _isReady = false;
            _isPressShoot = false;
            _moveSpeedWhenAttack = owner.MoveSpeedWhenAttack;
        }

        public void OnEnter()
        {
            //��������
            _isOver = false;
            _isReady = false;
            _isPressShoot=false;

            _owner.anim.Play(AnimTags.ShOOT);
            _eventDispatcher.OnAnimationEvent += HandleAnimation;

            if(_owner.kunaiNum > 0)
            {
                //�������
                GameObject kunaiObj = GameObject.Instantiate(_kunai, _owner.transform.position, _owner.transform.localRotation);
                kunaiObj.transform.localScale = _owner.transform.localScale;
                _owner.kunaiNum--;
                _owner.UpdateUI();
            }
            //player.CheckHit();
        }

        public void OnUpdate()
        {
            if (_mFsm.IsCeaseInput())
            {
                _mFsm.TransitionStatus(actionType.idle);
                return;
            }

            //����Ԥ����
            if (Input.GetButtonDown(Trigger.SHOOT))
                _isPressShoot = true;

            //�ж�ģ��
            if (_isReady)
            {
                if (Input.GetButtonDown(Trigger.JUMP))//��Ծ(���Դ��ȡ����Щ)
                {
                    _mFsm.TransitionStatus(actionType.jump);
                }
                else if (Input.GetButtonDown(Trigger.SKILL)) // ����
                {
                    _mFsm.TransitionStatus(actionType.skill);
                }
                else if (Input.GetButtonDown(Trigger.ATTACK))
                {
                    _mFsm.TransitionStatus(actionType.attack1);//��ת����1
                }
                else if (_isPressShoot)// ���¹���������ǰ����,������Ԥ���룬�������ȶ���Ҫ���ͣ����д��Ԥ����
                {
                    _mFsm.TransitionStatus(actionType.shoot);//�������
                }
                else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
                {
                    if (Input.GetButton(Trigger.RUN))
                        _owner.m_FSM.TransitionStatus(actionType.run);//����
                    else
                        _owner.m_FSM.TransitionStatus(actionType.walk);//����
                }

                else if (_isOver)
                {
                    _mFsm.TransitionStatus(actionType.idle);
                }
            }
            else
            {
                float _inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);
                _owner.rg.velocity = new Vector2(_inputSpeed * _moveSpeedWhenAttack, _owner.rg.velocity.y);
                _owner.CheckOriented();
            }

            //�����ᴦ��Idle����ת
        }

        public void OnExit()
        {
            _eventDispatcher.OnAnimationEvent -= HandleAnimation;
        }

        public void HandleAnimation(AnimationStage stage)
        {
            if (stage.Equals(AnimationStage.NextStageReady))
            {
                _isReady = true;
            }
            else if (stage.Equals(AnimationStage.AnimEnd))
            {
                _isOver = true;
            }
            
            // ��֧��������״̬
        }
    }
}