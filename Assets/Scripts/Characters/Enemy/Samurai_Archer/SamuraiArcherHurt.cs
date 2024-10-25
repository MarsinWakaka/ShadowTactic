using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class SamuraiArcherHurt : IState
{
    //所需组件
    SamuraiArcher owner;
    Fsm _Fsm;
    AnimationEventDispatcher eventDispatcher;

    //所需参数
    private float timer;
    private float waitTime;
    private bool isOver;

    public SamuraiArcherHurt(SamuraiArcher onwer)
    {

        this.owner = onwer;
        _Fsm = owner.m_FSM;
        eventDispatcher = owner.EventDispatcher;

        //参数初始化
        isOver = false;

        
    }

    public void OnEnter()
    {
        owner.anim.Play(AnimTags.HURT);
        GameObject go = GameObject.Instantiate(owner.HurtFX, owner.transform.position, owner.transform.rotation);

        // 随机旋转角度
        float randomRotation = Random.Range(0, 360);
        go.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        eventDispatcher.OnAnimationEvent += HandleAnimation;

        isOver = false;

        //没找到目标受到伤害，则转身寻找
        if (owner.Target == null)
        {
            Transform player = GameManager.Instance.GetPlayer().transform;
            if (Mathf.Sign(player.position.x - owner.transform.position.x) * owner.transform.localScale.x < 0)
                owner.ChangeOriented();
        }
    }

    public void OnUpdate()
    {
        if (isOver)
        {
            if (owner.Target == null)
            {
                _Fsm.TransitionStatus(actionType.walk);
            }
            else
            {
                float deltaDist = owner.distFromTarget();
                if (deltaDist < owner.near_range_enabled_dist)
                {
                    _Fsm.TransitionStatus(owner.GetAttackIndex());
                }
                else if (deltaDist < owner.rangeWeaponDist)
                {
                    _Fsm.TransitionStatus(actionType.shoot);
                }
                else
                {
                    _Fsm.TransitionStatus(actionType.run);
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
        if (stage == AnimationStage.AnimEnd)
            isOver = true;
    }
}
