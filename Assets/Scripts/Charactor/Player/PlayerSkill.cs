using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : IState
{
    Player player;
    Fsm m_FSM;
    AnimationEventDispatcher eventDispatcher;

    //参数
    private const int attackID = 2;
    private float moveSpeedWhenAttack = 0f;
    float _inputSpeed;

    private bool isReadyForNextPhase;
    private bool isPressAttack;
    private bool isOver;
    private bool attackCheck;
    private bool isPressJump;

    public PlayerSkill(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
        moveSpeedWhenAttack = player.MoveSpeedWhenAttack;
        this.eventDispatcher = player.eventDispatcher;

        //变量初始化

        isReadyForNextPhase = false;
        isPressAttack = false;
        isOver = false;
    }

    public void OnEnter()
    {
        //变量重置
        isReadyForNextPhase = false;
        isPressAttack = false;
        isPressJump = false;
        isOver = false;
        attackCheck = false;

        player.anim.Play(AnimTags.SKILL);
        player.CancelHideStateWithRefresh();
        player.rg.velocity = new Vector2(0, player.rg.velocity.y);
        eventDispatcher.OnAnimationEvent += HandleAnimationAction;

        //player.UpdateBodyState();
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }
        //动作预输入
        if (!isPressAttack && Input.GetButtonDown(Trigger.ATTACK))
            isPressAttack = true;
        if (!isPressJump && Input.GetButtonDown(Trigger.JUMP))
            isPressJump = true;

        //判断模块
        if (isOver)
        {
            m_FSM.TransitionStatus(actionType.idle);
        }
        else if (isReadyForNextPhase)
        {
            if (isPressJump)       // 跳跃(可以打断取消这些)
            {
                m_FSM.TransitionStatus(actionType.jump);
            }
            else if (Input.GetButtonDown(Trigger.SKILL)) // 大招
            {
                m_FSM.TransitionStatus(actionType.skill);
            }
            else if (Input.GetButtonDown(Trigger.SHOOT)) // 射击
            {
                m_FSM.TransitionStatus(actionType.shoot);
            }
            else if (isPressAttack)// 按下攻击键或提前按下
            {
                m_FSM.TransitionStatus(actionType.attack1);//跳转动作2
            }
            else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);//奔跑
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//行走
            }
        }
        else if (!attackCheck && player.AttackRange[attackID].gameObject.activeSelf)
        {
            player.AttackRange[attackID].gameObject.SetActive(false);
            player.rg.velocity = new Vector2(player.rg.velocity.x / 2, player.rg.velocity.y);
        }
        else if (attackCheck) // 攻击阶段可以缓慢移动
        {
            //攻击检测
            player.CheckHit(attackID);

            //_inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);

            player.rg.velocity = new Vector2(5 * player.transform.localScale.x, player.rg.velocity.y);

            //player.rg.velocity = new Vector2(_inputSpeed * moveSpeedWhenAttack, player.rg.velocity.y);

        }

        player.CheckOriented();
        //动画会处理Idle的跳转
    }

    public void OnExit()
    {
        //防止受伤导致角色没有关闭攻击检测
        player.AttackRange[attackID].gameObject.SetActive(false);
        eventDispatcher.OnAnimationEvent -= HandleAnimationAction;
    }

    public void HandleAnimationAction(AnimationStage stage)
    {
        _ = stage switch
        {
            AnimationStage.checkHit => attackCheck = true,
            AnimationStage.stopCheckHit => attackCheck = false,
            AnimationStage.nextStageReady => isReadyForNextPhase = true,
            AnimationStage.animEnd => isOver = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }

    public virtual void Skill()
    {

    }
}
