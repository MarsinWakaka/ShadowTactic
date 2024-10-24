using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDefence : IState
{
    //�������
    Samurai owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;


    //�������
    private bool isOver;
    private bool isReady;
    private float distAttack;

    public SamuraiDefence(Samurai onwer)
    {
        //�����ʼ��
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.eventDispatcher;

        //������ʼ��
        distAttack = owner.near_range_enabled_dist;

        //֪ͨowner������ʽ����
        owner.AttackIndexMax++;
    }

    public void OnEnter()
    {
        owner.anim.Play(AnimTags.DEFENCE);

        //���ò���
        owner.isMortal = true;
        isReady = false;
        isOver = false;

        eventDispatcher.OnAnimationEvent += HandleAnimation;
    }

    public void OnUpdate()
    {
        if (isOver) //��������
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
        else if (isReady) //�Ѿ�׼�������ڼ�
        {
            owner.CheckDefence();
        }
        else //���ڹ����ж��ڼ�
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
