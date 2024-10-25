using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinIdle : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        public MarsinIdle(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.rg.velocity = new Vector2(0, _owner.rg.velocity.y);
            _owner.anim.Play(AnimTags.IDLE);
        }

        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput())
                return;

            if (_owner.CheckFall())
            {
                _fsm.TransitionStatus(actionType.fall);
            }
            else if (Input.GetButtonDown(Trigger.JUMP) && _owner.CheckOnGround())//跳跃
            {
                _fsm.TransitionStatus(actionType.jump);
            }
            else if (Input.GetButtonDown(Trigger.DEFENCE)) // 防御
            {
                _fsm.TransitionStatus(actionType.defence);
            }
            else if (Input.GetButtonDown(Trigger.ATTACK))
            {
                _fsm.TransitionStatus(actionType.attack1);
            }
            else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    _owner.m_FSM.TransitionStatus(actionType.run);//奔跑
                else
                    _owner.m_FSM.TransitionStatus(actionType.walk);//行走
            }
        }

        public void OnExit()
        {
            
        }
    }
}