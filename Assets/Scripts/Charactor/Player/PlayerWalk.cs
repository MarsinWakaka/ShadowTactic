using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : IState
{
    Player player;
    Fsm m_FSM;
    [SerializeField]
    private float walkSpeed = 0f;

    public PlayerWalk(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
        walkSpeed = player.WalkSpeed;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.WALK);
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

        if (player.CheckFall())
        {
            m_FSM.TransitionStatus(actionType.fall);
        }
        else if (Input.GetButtonDown(Trigger.JUMP) && player.CheckOnGround())//ÌøÔ¾
        {
            m_FSM.TransitionStatus(actionType.jump);
        }
        else if (Input.GetButtonDown(Trigger.SKILL)) // ´óÕÐ
        {
            m_FSM.TransitionStatus(actionType.skill);
        }
        else if (Input.GetButtonDown(Trigger.SHOOT))
        {
            m_FSM.TransitionStatus(actionType.shoot);
        }
        else if (Input.GetButtonDown(Trigger.ATTACK))
        {
            m_FSM.TransitionStatus(actionType.attack1);
        }
        else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
        {
            if (Input.GetButton(Trigger.RUN))
                player.m_FSM.TransitionStatus(actionType.run);//±¼ÅÜ
            else
                player.rg.velocity = new Vector2(Input.GetAxisRaw(Axis.HORIZONTAL) * walkSpeed, player.rg.velocity.y);//ÐÐ×ß
        }
        else
        {
            player.m_FSM.TransitionStatus(actionType.idle);
        }
        player.CheckOriented();
    }

    public void OnExit()
    {
        
    }
}
