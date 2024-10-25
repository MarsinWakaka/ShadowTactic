using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaRun : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;
    [SerializeField]
    private float runSpeed = 0f;

    public MonkNinjaRun(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        runSpeed = basePlayer.RunSpeed;
        m_FSM = basePlayer.m_FSM;
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
        _basePlayer.anim.Play(AnimTags.RUN);
        _basePlayer.CancelHideStateWithRefresh();
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

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
                _basePlayer.rg.velocity = new Vector2(Input.GetAxis(Axis.HORIZONTAL) * runSpeed, _basePlayer.rg.velocity.y);//±¼ÅÜ
            else
                _basePlayer.m_FSM.TransitionStatus(actionType.walk);//ÐÐ×ß
        }
        else
        {
            m_FSM.TransitionStatus(actionType.idle);
        }
        _basePlayer.CheckOriented();
    }

    public void OnExit()
    {
        
    }
}
