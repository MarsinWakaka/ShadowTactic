using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDefence : IState
{
    //所需组件
    Samurai owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;


    //所需参数
    private bool isOver;
    private bool isReady;
    private float distAttack;

    public SamuraiDefence(Samurai onwer)
    {
        //组件初始化
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.eventDispatcher;

        //参数初始化
        distAttack = owner.near_range_enabled_dist;

        //通知owner攻击方式增加
        owner.AttackIndexMax++;
    }

    public void OnEnter()
    {
        owner.anim.Play(AnimTags.DEFENCE);

        //重置参数
        owner.isMortal = true;
        isReady = false;
        isOver = false;

        eventDispatcher.OnAnimationEvent += HandleAnimation;
    }

    public void OnUpdate()
    {
        if (isOver) //动画结束
        {
            float deltaDist = owner.distFromTarget();
            if (deltaDist < distAttack)// 玩家仍在攻击范围内
            {
                Fsm.TransitionStatus(owner.GetAttackIndex());
            }
            else
            {
                Fsm.TransitionStatus(actionType.run);
            }
        }
        else if (isReady) //已经准备好了期间
        {
            owner.CheckDefence();
        }
        else //不在攻击判定期间
        {
            owner.CheckOriented();
        }
    }

    public void OnExit()
    {
        owner.isMortal = false;
        eventDispatcher.OnAnimationEvent -= HandleAnimation;
    }

    public void HandleAnimation(AnimationStage stage)
    {
        _ = stage switch
        {
            AnimationStage.nextStageReady => isReady = true,
            AnimationStage.animEnd => isOver = true,
            AnimationStage.nextStageEnd => isReady = false,
            _ => throw new ArgumentException(message: $"enum value {stage} dosen't match any stage"),
        };
    }
}
