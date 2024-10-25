using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiArcherShoot : IState
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

    public SamuraiArcherShoot(SamuraiArcher onwer)
    {
        //�����ʼ��
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.EventDispatcher;

        //������ʼ��
        moveSpeed = owner.MoveSpeedWhenAttacked;
        distAttack = owner.near_range_enabled_dist;
        distRangedWeapon = owner.rangeWeaponDist;
        isOver = false;
    }

    public void OnEnter()
    {
        //���ò���
        //��ֹ����δ������ת����attackCheckû������
        attackCheck = false;
        isOver = false;

        //door = owner.door;
        owner.anim.Play(AnimTags.ShOOT);
        eventDispatcher.OnAnimationEvent += HandleAnimation;

        owner.rg.velocity = new Vector2 (owner.transform.localScale.x * moveSpeed, owner.rg.velocity.y);
    }

    public void OnUpdate()
    {
        if (attackCheck) //�����ڼ�
        {
            Transform atkTransform = owner.attackPos;
            GameObject go = GameObject.Instantiate(owner.arrow, atkTransform.position, atkTransform.rotation);
            go.transform.localScale = owner.transform.localScale;
            attackCheck = false;
        }
        else //���ڹ����ڼ�
        {
            owner.CheckOriented();

            if (isOver)//�����������Խ��о���
            {
                float deltaDist = owner.distFromTarget();

                if (deltaDist < distAttack)// ��ս������Χ��
                {
                    Fsm.TransitionStatus(owner.GetAttackIndex());
                }
                else if (deltaDist < distRangedWeapon)// Զ�̹�����Χ��
                {
                    Fsm.TransitionStatus(actionType.shoot);
                }
                else
                {
                    Fsm.TransitionStatus(actionType.run); //����Ŀ��׷��
                }
            }
            else
            {
                if( owner.distFromTarget() < distAttack)
                {
                    Fsm.TransitionStatus(owner.GetAttackIndex());
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
        //switch stage => 
        if (stage == AnimationStage.SetCheckPointLeftBorder)
            attackCheck = true;
        else if (stage == AnimationStage.AnimEnd)
            isOver = true;
    }
}
