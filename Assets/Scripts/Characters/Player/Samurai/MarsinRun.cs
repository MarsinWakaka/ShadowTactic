﻿using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinRun : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        public MarsinRun(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.anim.Play(AnimTags.RUN);
        }

        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput())
            {
                _fsm.TransitionStatus(actionType.idle);
                return;
            }
            
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
                    _owner.rg.velocity = new Vector2(
                        Input.GetAxisRaw(Axis.HORIZONTAL) * _owner.RunSpeed, 
                        _owner.rg.velocity.y);//奔跑
                else
                    _owner.m_FSM.TransitionStatus(actionType.walk);//行走
            }
            else
            {
                _owner.m_FSM.TransitionStatus(actionType.idle);
            }
            _owner.CheckOriented();
        }

        public void OnExit()
        {
            
        }
    }
}