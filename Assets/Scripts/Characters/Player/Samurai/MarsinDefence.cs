using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinDefence : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;

        private bool _isPressAttack;
        private bool _isPressDefence;
        private bool _isPressJump;
        private bool _isCheckingAttack;
        // private bool _canReleaseAntiAttack;
        private bool _isReadyForNextPhase;
        private bool _isAnimationEnd;
        
        public MarsinDefence(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.EventDispatcher.OnAnimationEvent += AnimationEventHandler;
            _owner.anim.Play(AnimTags.DEFENCE);

            _isAnimationEnd = false;
            _isPressAttack = false;
            _isPressJump = false;
            // _canReleaseAntiAttack = false;
            _isReadyForNextPhase = false;
        }

        public void OnUpdate()
        {
            if (_isAnimationEnd) _fsm.TransitionStatus(actionType.idle);
            
            // 动作预输入
            if (Input.GetButtonDown(Trigger.ATTACK)) _isPressAttack = true;
            if (Input.GetButtonDown(Trigger.JUMP)) _isPressJump = true;
            if (Input.GetButtonDown(Trigger.DEFENCE)) _isPressDefence = true;

            if (_isReadyForNextPhase)
            {
                if (_isPressAttack)
                {
                    // _fsm.TransitionStatus(_canReleaseAntiAttack ? actionType.antiAttack : actionType.attack1);
                    _fsm.TransitionStatus(actionType.attack1);
                }
                else if (_isPressDefence)
                {
                    _fsm.TransitionStatus(actionType.defence);
                }
                else if (_isPressJump)
                {
                    _fsm.TransitionStatus(actionType.jump);
                }
                else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
                {
                    if (Input.GetButton(Trigger.RUN))
                        _owner.m_FSM.TransitionStatus(actionType.run); //奔跑
                    else
                        _owner.m_FSM.TransitionStatus(actionType.walk); //行走
                }
            }
        }

        public void OnExit()
        {
            _owner.isMortal = false;
            ReadyForDefence(false);
            _owner.EventDispatcher.OnAnimationEvent -= AnimationEventHandler;
        }

        private void ReadyForDefence(bool flag)
        {
            _owner.isMortal = flag;
            if (flag)
            {
                _owner.healthController.OnHit += BeHitWhenDefenceHandle;
            }
            else
            {
                _owner.healthController.OnHit -= BeHitWhenDefenceHandle;
            }
        }
        
        private void BeHitWhenDefenceHandle()
        {
            // _canReleaseAntiAttack = true;
            _fsm.TransitionStatus(actionType.defenceSuccess);
            // Debug.Log("玩家防御成功");
        }

        private void AnimationEventHandler(AnimationStage stage)
        {
            switch (stage)
            {
                case AnimationStage.SetCheckPointLeftBorder:
                    ReadyForDefence(true);
                    break; 
                case AnimationStage.SetCheckPointRightBorder:
                    ReadyForDefence(false);
                    break;
                case AnimationStage.NextStageReady:
                    _isReadyForNextPhase = true;
                    break;
                case AnimationStage.AnimEnd:
                    _isAnimationEnd = true;
                    break;
                default:
                    Debug.LogError($"enum value {stage} dose not match any stage");
                    break;
            }
        }
    }
}