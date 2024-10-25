
using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinDefenceSuccess : IState
    {
        private SamuraiMarsin _owner;
        
        public MarsinDefenceSuccess(SamuraiMarsin owner)
        {
            _owner = owner;
        }
        
        private bool _isNextStageReady, _isAnimEnd;
        private bool _isPressAttack, _isPressJump;
        
        public void OnEnter()
        {
            _owner.isMortal = true;
            
            _isPressAttack = _isPressJump = false;
            _isNextStageReady = _isAnimEnd = false;
            
            _owner.anim.Play(AnimTags.DEFENCE_SUCCESS);
            _owner.EventDispatcher.OnAnimationEvent += AnimationStageHandler;
        }
        
        public void OnUpdate()
        {
            if (_isAnimEnd) _owner.m_FSM.TransitionStatus(actionType.idle);
            
            // 动作预输入
            if (Input.GetButtonDown(Trigger.ATTACK)) _isPressAttack = true;
            
            if (_isNextStageReady)
            {
                if (_isPressAttack)
                {
                    _owner.m_FSM.TransitionStatus(actionType.antiAttack);
                }
                else if (_isPressJump)
                {
                    _owner.m_FSM.TransitionStatus(actionType.jump);
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
            _owner.EventDispatcher.OnAnimationEvent -= AnimationStageHandler;
        }
        
        private void AnimationStageHandler(AnimationStage stage)
        {
            switch (stage)
            {
                case AnimationStage.SetCheckPointRightBorder:
                    _owner.isMortal = false;
                    break;
                case AnimationStage.NextStageReady:
                    _isNextStageReady = true;
                    break;
                case AnimationStage.AnimEnd:
                    _isAnimEnd = true;
                    break;
            }
        }
    }
}