using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaWalk : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;
    private float walkSpeed = 0f;

    public MonkNinjaWalk(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
        walkSpeed = basePlayer.WalkSpeed;
    }

    public void OnEnter()
    {
        _basePlayer.anim.Play(AnimTags.WALK);
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
        else if (Input.GetButtonDown(Trigger.JUMP) && _basePlayer.CheckOnGround())//��Ծ
        {
            m_FSM.TransitionStatus(actionType.jump);
        }
        else if (Input.GetButtonDown(Trigger.SKILL)) // ����ذ��
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
                _basePlayer.m_FSM.TransitionStatus(actionType.run);//����
            else
                _basePlayer.rg.velocity = new Vector2(Input.GetAxisRaw(Axis.HORIZONTAL) * walkSpeed, _basePlayer.rg.velocity.y);//����
        }
        else
        {
            _basePlayer.m_FSM.TransitionStatus(actionType.idle);
        }
        _basePlayer.CheckOriented();
    }

    public void OnExit()
    {
        
    }
}
