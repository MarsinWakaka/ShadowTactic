using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiIdle : IState
{
    //所需组件
    Samurai owner;

    Fsm fsm;

    //所需参数
    private float timer;
    private float waitTime;

    public SamuraiIdle(Samurai onwer)
    {
        //参数初始化
        this.owner = onwer;

        fsm = owner.m_FSM;
        this.waitTime = owner.WaitTime;
    }

    public void OnEnter()
    {
        //重置参数
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