using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiAttack2 : IState
{
    //所需组件
    Samurai owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;

    //所需参数
    private bool attackCheck;
    private bool isOver;

    private float moveSpeed;
    private float distAttack;
    private bool isReady;

    public SamuraiAttack2(Samurai onwer)
    {
        //组件初始化
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.eventDispatcher;

        //参数初始化
        moveSpeed = owner.MoveSpeedWhenAttacked;
        distAttack = owner.near_range_enabled_dist;

        //通知owner攻击方式增加
        owner.AttackIndexMax++;
    }

    public void OnEnter()
    {
        owner.anim.Play(AnimTags.ATTACK2);
        owner.rg.velocity = new Vector2(0, owner.rg.velocity.y);

        //重置参数
        //防止动画未结束跳转导致attackCheck没被重置，例如受伤导致的强制切换
        attackCheck = false;
        isOver = false;
        isReady = false;

        eventDispatcher.OnAnimationEvent += HandleAnimation;
    }

    public void OnUpdate()
    {
        if (attackCheck) //攻击判定期间
        {
            if (owner.CheckHit())
            {
                attackCheck = false; //只做一次攻击，判定命中直接结束
            }
            else
            {
                owner.Move(moveSpeed);
            }
        }
        //不在攻击判定期间
        else if (isOver) //动画结束
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
        else
        {
            owner.CheckOriented();
            if (isReady)
            {
                if (owner.distFromTarget() < distAttack && owner.CheckDefence())
                {
                    Fsm.TransitionStatus(actionType.defence);
                }
            }
        }
    }

    public void OnExit()
    {
        eventDispatcher.OnAnimationEvent -= HandleAnimation;
    }

    public void HandleAnimation(AnimationStage stage)
    {
        _ = stage switch
        {
            AnimationStage.checkHit => attackCheck = true,
            AnimationStage.stopCheckHit => attackCheck = false,
            AnimationStage.nextStageReady => isReady = true,
            AnimationStage.nextStageEnd => isReady = false,
            AnimationStage.animEnd => isOver = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }
}