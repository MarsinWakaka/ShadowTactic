using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiPatrol : IState
{
    //所需组件
    Samurai owner;
    Fsm fsm;

    //所需参数
    private float attackDist;
    private float timer;
    private float patrolTime;
    private float patrolSpeed;

    public SamuraiPatrol(Samurai onwer)
    {

        this.owner = onwer;
        fsm = owner.m_FSM;

        //参数初始化
        patrolSpeed = owner.WalkSpeed;
        attackDist = owner.near_range_enabled_dist;
        patrolTime = owner.PatrolTime;
    }

    public void OnEnter()
    {
        //重置参数
        timer = 0;
        owner.anim.Play(AnimTags.WALK);
    }

    public void OnUpdate()
    {
        if (owner.Target == null)//无目标巡逻
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
            fsm.TransitionStatus(actionType.run);
        }
    }

    public void OnExit()
    {

    }
}
