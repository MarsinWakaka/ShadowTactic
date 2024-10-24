using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiAttack2 : IState
{
    //�������
    Samurai owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;

    //�������
    private bool attackCheck;
    private bool isOver;

    private float moveSpeed;
    private float distAttack;
    private bool isReady;

    public SamuraiAttack2(Samurai onwer)
    {
        //�����ʼ��
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.eventDispatcher;

        //������ʼ��
        moveSpeed = owner.MoveSpeedWhenAttacked;
        distAttack = owner.near_range_enabled_dist;

        //֪ͨowner������ʽ����
        owner.AttackIndexMax++;
    }

    public void OnEnter()
    {
        owner.anim.Play(AnimTags.ATTACK2);
        owner.rg.velocity = new Vector2(0, owner.rg.velocity.y);

        //���ò���
        //��ֹ����δ������ת����attackCheckû�����ã��������˵��µ�ǿ���л�
        attackCheck = false;
        isOver = false;
        isReady = false;

        eventDispatcher.OnAnimationEvent += HandleAnimation;
    }

    public void OnUpdate()
    {
        if (attackCheck) //�����ж��ڼ�
        {
            if (owner.CheckHit())
            {
                attackCheck = false; //ֻ��һ�ι������ж�����ֱ�ӽ���
            }
            else
            {
                owner.Move(moveSpeed);
            }
        }
        //���ڹ����ж��ڼ�
        else if (isOver) //��������
        {
            float deltaDist = owner.distFromTarget();
            if (deltaDist < distAttack)// ������ڹ�����Χ��
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