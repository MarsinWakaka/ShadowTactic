using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : IState
{
    Player player;
    Fsm m_FSM;
    [SerializeField]
    private float runSpeed = 0f;

    public PlayerRun(Player player)
    {
        this.player = player;
        runSpeed = player.RunSpeed;
        m_FSM = player.m_FSM;
    }

    public bool EnterCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                return true;
        }
        return false;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.RUN);
        player.CancelHideStateWithRefresh();
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
                player.rg.velocity = new Vector2(Input.GetAxis(Axis.HORIZONTAL) * runSpeed, player.rg.velocity.y);//±¼ÅÜ
            else
                player.m_FSM.TransitionStatus(actionType.walk);//ÐÐ×ß
        }
        else
        {
            m_FSM.TransitionStatus(actionType.idle);
        }
        player.CheckOriented();
    }

    public void OnExit()
    {
        
    }
}
