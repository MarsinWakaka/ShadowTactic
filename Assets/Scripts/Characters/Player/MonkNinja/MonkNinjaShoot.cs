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

        //字段

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
            //变量重置
            _isOver = false;
            _isReady = false;
            _isPressShoot=false;

            _owner.anim.Play(AnimTags.ShOOT);
            _eventDispatcher.OnAnimationEvent += HandleAnimation;

            if(_owner.kunaiNum > 0)
            {
                //发射飞镖
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

            //动作预输入
            if (Input.GetButtonDown(Trigger.SHOOT))
                _isPressShoot = true;

            //判断模块
            if (_isReady)
            {
                if (Input.GetButtonDown(Trigger.JUMP))//跳跃(可以打断取消这些)
                {
                    _mFsm.TransitionStatus(actionType.jump);
                }
                else if (Input.GetButtonDown(Trigger.SKILL)) // 大招
                {
                    _mFsm.TransitionStatus(actionType.skill);
                }
                else if (Input.GetButtonDown(Trigger.ATTACK))
                {
                    _mFsm.TransitionStatus(actionType.attack1);//跳转动作1
                }
                else if (_isPressShoot)// 按下攻击键或提前按下,由于是预输入，所以优先度需要降低，运行打断预输入
                {
                    _mFsm.TransitionStatus(actionType.shoot);//继续射击
                }
                else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
                {
                    if (Input.GetButton(Trigger.RUN))
                        _owner.m_FSM.TransitionStatus(actionType.run);//奔跑
                    else
                        _owner.m_FSM.TransitionStatus(actionType.walk);//行走
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

            //动画会处理Idle的跳转
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
            
            // 分支处理其它状态
        }
    }
}