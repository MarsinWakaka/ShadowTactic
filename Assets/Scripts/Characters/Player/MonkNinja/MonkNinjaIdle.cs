using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaIdle : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;
    [SerializeField]

    public MonkNinjaIdle(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
    }

    public void OnEnter()
    {
        _basePlayer.rg.velocity = new Vector2 (0, _basePlayer.rg.velocity.y);
        _basePlayer.anim.Play(AnimTags.IDLE);
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
            return;

        if (_basePlayer.CheckFall())
        {
            m_FSM.TransitionStatus(actionType.fall);
        }
        else if (Input.GetButtonDown(Trigger.JUMP) && _basePlayer.CheckOnGround())//ÌøÔ¾
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
                _basePlayer.m_FSM.TransitionStatus(actionType.run);//±¼ÅÜ
            else
                _basePlayer.m_FSM.TransitionStatus(actionType.walk);//ÐÐ×ß
        }
    }

    public void OnExit()
    {
        
    }
}
