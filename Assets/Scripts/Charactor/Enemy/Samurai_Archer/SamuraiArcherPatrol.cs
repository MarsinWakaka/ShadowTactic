using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiArcherPatrol : IState
{
    //�������
    SamuraiArcher owner;
    Fsm fsm;

    //�������
    private float attackDist;
    private float timer;
    private float patrolTime;
    private float patrolSpeed;

    public SamuraiArcherPatrol(SamuraiArcher onwer)
    {
        
        this.owner = onwer;
        fsm = owner.m_FSM;

        //������ʼ��
        patrolSpeed = owner.WalkSpeed;
        attackDist = owner.near_range_enabled_dist;
        patrolTime = owner.PatrolTime;
    }

    public void OnEnter()
    {
        //���ò���
        timer = 0;
        owner.anim.Play(AnimTags.RUN);
    }

    public void OnUpdate()
    {
        if (owner.Target == null)//��Ŀ��Ѳ��
        {
            owner.Move(patrolSpeed);

            timer += Time.deltaTime;

            if (timer > patrolTime)
            {
                owner.ChangeOriented();
                fsm.TransitionStatus(actionType.idle);
            }
        }
        else
        {
            float deltaDist = owner.distFromTarget();
            if (deltaDist < owner.near_range_enabled_dist)
            {
                fsm.TransitionStatus(owner.GetAttackIndex());
            }
            else if (deltaDist < owner.rangeWeaponDist)
            {
                fsm.TransitionStatus(actionType.shoot);
            }
            fsm.TransitionStatus(actionType.run);
        }
    }

    public void OnExit()
    {

    }
}