using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiChase : IState
{
    //�������
    Samurai owner;
    Fsm Fsm;

    //�������
    private float near_range_enabled_dist;
    private float chaseSpeed;
    private float timer;
    private float maxChaseTimer;

    public SamuraiChase(Samurai onwer)
    {
        this.owner = onwer;
        Fsm = owner.m_FSM;

        //������ʼ��
        near_range_enabled_dist = owner.near_range_enabled_dist;
        chaseSpeed = owner.ChaseSpeed;
        maxChaseTimer = owner.MaxChaseTime;
    }

    public void OnEnter()
    {
        //���ò���
        timer = 0;
        owner.anim.Play(AnimTags.RUN);
    }

    public void OnUpdate()
    {
        if (owner.Target == null) //��Ŀ��׷��
        {
            timer += Time.deltaTime;
            if (timer > maxChaseTimer)
            {
                Fsm.TransitionStatus(actionType.idle);
            }
            owner.Move(chaseSpeed);
        }
        else //��Ŀ���׷��
        {
            owner.CheckOriented();
            float deltaDist = owner.distFromTarget();
            if (deltaDist < near_range_enabled_dist)
            {
                Fsm.TransitionStatus(actionType.attack1);
            }
            else //����׷��
            {
                owner.Move(chaseSpeed);
            }
        }
    }

    public void OnExit()
    {

    }
}
