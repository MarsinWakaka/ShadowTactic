using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiArcherAttack2 : IState
{
    //�������
    SamuraiArcher owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;


    //�������
    private bool attackCheck;
    private bool isOver;

    private float distAttack;
    private float distRangedWeapon;
    private float moveSpeed;

    public SamuraiArcherAttack2(SamuraiArcher onwer)
    {
        //�����ʼ��
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.EventDispatcher;

        //������ʼ��
        moveSpeed = owner.MoveSpeedWhenAttacked;
        distAttack = owner.near_range_enabled_dist;
        distRangedWeapon = owner.rangeWeaponDist;

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
        else //���ڹ����ж��ڼ�
        {
            owner.CheckOriented();
            if (isOver) //��������
            {
                float deltaDist = owner.distFromTarget();
                if (deltaDist < distAttack)// ������ڹ�����Χ��
                {
                    Fsm.TransitionStatus(owner.GetAttackIndex());
                }
                else if (deltaDist < distRangedWeapon)
                {
                    Fsm.TransitionStatus(actionType.shoot);
                }
                else
                {
                    Fsm.TransitionStatus(actionType.run);
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
            AnimationStage.SetCheckPointLeftBorder => attackCheck = true,
            AnimationStage.SetCheckPointRightBorder => attackCheck = false,
            AnimationStage.AnimEnd => isOver = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }
}
