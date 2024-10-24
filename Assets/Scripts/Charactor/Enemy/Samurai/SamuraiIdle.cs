using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiIdle : IState
{
    //�������
    Samurai owner;

    Fsm fsm;

    //�������
    private float timer;
    private float waitTime;

    public SamuraiIdle(Samurai onwer)
    {
        //������ʼ��
        this.owner = onwer;

        fsm = owner.m_FSM;
        this.waitTime = owner.WaitTime;
    }

    public void OnEnter()
    {
        //���ò���
        timer = 0;
        //door = owner.door;
        owner.anim.Play(AnimTags.IDLE);
    }

    public void OnUpdate()
    {
        if (owner.Target == null)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                fsm.TransitionStatus(actionType.walk);
            }
        }
        else
        {
            float deltaDist = owner.distFromTarget();
            if (deltaDist < owner.near_range_enabled_dist)
            {
                fsm.TransitionStatus(owner.GetAttackIndex());
            }
            fsm.TransitionStatus(actionType.run);
        }
    }

    public void OnExit()
    {

    }
}
;