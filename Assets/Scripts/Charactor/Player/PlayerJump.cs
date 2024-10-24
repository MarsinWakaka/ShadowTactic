using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : IState
{
    Player player;
    Fsm m_FSM;

    private bool hasJump;
    private float jumpForce;
    private float jumpSpeed;

    public PlayerJump(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        jumpForce = player.JumpForce;

        player.anim.Play(AnimTags.JUMP);

        //跳跃一瞬间提供向上的力,获取刚进入跳跃时的速度 
        jumpSpeed = Mathf.Max(Mathf.Abs(player.rg.velocity.x), player.JumpSpeed);
        player.rg.velocity = new Vector2 (player.rg.velocity.x, jumpForce);
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
        if (player.rg.velocity.x < player.RunSpeed)
        {
            float input = Input.GetAxisRaw(Axis.HORIZONTAL);
            player.rg.velocity = new Vector2(input * jumpSpeed, player.rg.velocity.y);
        }
        if (!hasJump && !player.CheckOnGround())//离开地面
            hasJump = true;

        player.CheckOriented();

        if (hasJump && player.CheckOnGround())//起跳后落到地面
        {
            if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//行走
            }
            else
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        else if (player.rg.velocity.y < 0) 
            m_FSM.TransitionStatus(actionType.fall);
    }

    public void OnExit()
    {
        
    }
}
