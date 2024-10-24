using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiArcherChase : IState
{
    //所需组件
    SamuraiArcher owner;
    Fsm Fsm;

    //所需参数
    private float near_range_enabled_dist;
    private float chaseSpeed;
    private float timer;
    private float maxChaseTimer;

    public SamuraiArcherChase(SamuraiArcher onwer)
    {
        this.owner = onwer;
        Fsm = owner.m_FSM;

        //参数初始化
        near_range_enabled_dist = owner.near_range_enabled_dist;
        chaseSpeed = owner.ChaseSpeed;
        maxChaseTimer = owner.MaxChaseTime;
    }

    public void OnEnter()
    {
        //重置参数
        timer = 0;
        owner.anim.Play(AnimTags.RUN);
    }

    public void OnUpdate()
    {
        if (owner.Target == null) //无目标追击
        {
            timer += Time.deltaTime;
            if(timer > maxChaseTimer)
            {
                Fsm.TransitionStatus(actionType.idle);
            }
            owner.Move(chaseSpeed);
        }
        else //有目标的追击
        {
            owner.CheckOriented();
            float deltaDist = owner.distFromTarget();
            if(deltaDist < owner.rangeWeaponDist * 0.7) //进入一半射程开始射击
            {
                Fsm.TransitionStatus(actionType.shoot);
            }
            else if (deltaDist < near_range_enabled_dist)
            {
                Fsm.TransitionStatus(actionType.attack1);
            }
            else //继续追击
            {
                owner.Move(chaseSpeed);
            }
        }
    }

    public void OnExit()
    {

    }
}
