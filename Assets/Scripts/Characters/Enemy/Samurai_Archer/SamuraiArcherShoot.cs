using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiArcherShoot : IState
{
    //所需组件
    SamuraiArcher owner;
    Fsm Fsm;
    AnimationEventDispatcher eventDispatcher;


    //所需参数
    private bool attackCheck;
    private bool isOver;

    private float distAttack;
    private float distRangedWeapon;
    private float moveSpeed;

    public SamuraiArcherShoot(SamuraiArcher onwer)
    {
        //组件初始化
        this.owner = onwer;
        Fsm = owner.m_FSM;
        eventDispatcher = owner.EventDispatcher;

        //参数初始化
        moveSpeed = owner.MoveSpeedWhenAttacked;
        distAttack = owner.near_range_enabled_dist;
        distRangedWeapon = owner.rangeWeaponDist;
        isOver = false;
    }

    public void OnEnter()
    {
        //重置参数
        //防止动画未结束跳转导致attackCheck没被重置
        attackCheck = false;
        isOver = false;

        //door = owner.door;
        owner.anim.Play(AnimTags.ShOOT);
        eventDispatcher.OnAnimationEvent += HandleAnimation;

        owner.rg.velocity = new Vector2 (owner.transform.localScale.x * moveSpeed, owner.rg.velocity.y);
    }

    public void OnUpdate()
    {
        if (attackCheck) //攻击期间
        {
            Transform atkTransform = owner.attackPos;
            GameObject go = GameObject.Instantiate(owner.arrow, atkTransform.position, atkTransform.rotation);
            go.transform.localScale = owner.transform.localScale;
            attackCheck = false;
        }
        else //不在攻击期间
        {
            owner.CheckOriented();

            if (isOver)//动作结束可以进行决策
            {
                float deltaDist = owner.distFromTarget();

                if (deltaDist < distAttack)// 近战攻击范围内
                {
                    Fsm.TransitionStatus(owner.GetAttackIndex());
                }
                else if (deltaDist < distRangedWeapon)// 远程攻击范围内
                {
                    Fsm.TransitionStatus(actionType.shoot);
                }
                else
                {
                    Fsm.TransitionStatus(actionType.run); //进入目标追击
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
