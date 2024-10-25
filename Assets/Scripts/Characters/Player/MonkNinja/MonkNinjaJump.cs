using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaJump : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;

    private bool hasJump;
    private float jumpForce;
    private float jumpSpeed;

    public MonkNinjaJump(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
    }

    public void OnEnter()
    {
        jumpForce = _basePlayer.JumpForce;

        _basePlayer.anim.Play(AnimTags.JUMP);

        //跳跃一瞬间提供向上的力,获取刚进入跳跃时的速度 
        jumpSpeed = Mathf.Max(Mathf.Abs(_basePlayer.rg.velocity.x), _basePlayer.JumpSpeed);
        _basePlayer.rg.velocity = new Vector2 (_basePlayer.rg.velocity.x, jumpForce);
        hasJump = false;
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

        //跳跃持续期间可以
        if (_basePlayer.rg.velocity.x < _basePlayer.RunSpeed)
        {
            float input = Input.GetAxisRaw(Axis.HORIZONTAL);
            _basePlayer.rg.velocity = new Vector2(input * jumpSpeed, _basePlayer.rg.velocity.y);
        }
        if (!hasJump && !_basePlayer.CheckOnGround())//离开地面
            hasJump = true;

        _basePlayer.CheckOriented();

        if (hasJump && _basePlayer.CheckOnGround())//起跳后落到地面
        {
            if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    _basePlayer.m_FSM.TransitionStatus(actionType.run);
                else
                    _basePlayer.m_FSM.TransitionStatus(actionType.walk);//行走
            }
            else
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        else if (_basePlayer.rg.velocity.y < 0) 
            m_FSM.TransitionStatus(actionType.fall);
    }

    public void OnExit()
    {
        
    }
}
