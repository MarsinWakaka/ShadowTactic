using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinFall : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        private float _lastVelocity;
        
        public MarsinFall(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.anim.Play(AnimTags.FALL);
        }

        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput())
            {
                _fsm.TransitionStatus(actionType.idle);
                return;
            }
            
            if(_owner.CheckOnGround())
            {
                if (_lastVelocity < -10f)
                    _owner.m_FSM.TransitionStatus(actionType.touchGround);
                else if(Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
                {
                    if (Input.GetButton(Trigger.RUN))
                        _owner.m_FSM.TransitionStatus(actionType.run);//奔跑
                    else
                        _owner.m_FSM.TransitionStatus(actionType.walk);//行走
                }
                else
                {
                    _owner.m_FSM.TransitionStatus(actionType.idle);
                }
            }
            
            _lastVelocity = _owner.rg.velocity.y;
        }

        public void OnExit()
        {
            
        }
    }
}