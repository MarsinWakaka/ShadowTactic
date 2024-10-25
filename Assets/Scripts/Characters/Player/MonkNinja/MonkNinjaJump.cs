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

        //��Ծһ˲���ṩ���ϵ���,��ȡ�ս�����Ծʱ���ٶ� 
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

        //��Ծ�����ڼ����
        if (_basePlayer.rg.velocity.x < _basePlayer.RunSpeed)
        {
            float input = Input.GetAxisRaw(Axis.HORIZONTAL);
            _basePlayer.rg.velocity = new Vector2(input * jumpSpeed, _basePlayer.rg.velocity.y);
        }
        if (!hasJump && !_basePlayer.CheckOnGround())//�뿪����
            hasJump = true;

        _basePlayer.CheckOriented();

        if (hasJump && _basePlayer.CheckOnGround())//�������䵽����
        {
            if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    _basePlayer.m_FSM.TransitionStatus(actionType.run);
                else
                    _basePlayer.m_FSM.TransitionStatus(actionType.walk);//����
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
