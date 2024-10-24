using System;
using UnityEngine;

public class PlayerFall : IState
{
    Player player;
    Fsm m_FSM;
    private float jumpSpeed;
    private float lastVelocity;

    [SerializeField]
    //private bool isOnGround;

    public PlayerFall(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.FALL);
        jumpSpeed = Mathf.Max(Mathf.Abs(player.rg.velocity.x), player.JumpSpeed);
        //isOnGround = false;
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
        player.CheckOriented();

        if(player.CheckOnGround())
        {
            if (lastVelocity < -10f)
                m_FSM.TransitionStatus(actionType.touchGround);
            else if(Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);//奔跑
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//行走
            }
            else
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        //退出条件：TouchGround动画最后一帧的触发事件，调用Player TouchGround函数

        lastVelocity = player.rg.velocity.y;
    }

    public void OnExit()
    {
        
    }
}
