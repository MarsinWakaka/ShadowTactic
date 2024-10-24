using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IState
{
    Player player;
    Fsm m_FSM;
    [SerializeField]

    public PlayerIdle(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        player.rg.velocity = new Vector2 (0, player.rg.velocity.y);
        player.anim.Play(AnimTags.IDLE);
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
            return;

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
                player.m_FSM.TransitionStatus(actionType.walk);//ÐÐ×ß
        }
    }

    public void OnExit()
    {
        
    }
}
