using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaAttack2 : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;
    AnimationEventDispatcher eventDispatcher;

    //参数
    private const int attackID = 1;
    private float moveSpeedWhenAttack = 0f;
    float _inputSpeed;

    private bool isReadyForNextPhase;
    private bool isPressAttack;
    private bool isOver;
    private bool attackCheck;
    private bool isPressJump;

    public MonkNinjaAttack2(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
        moveSpeedWhenAttack = basePlayer.MoveSpeedWhenAttack;
        this.eventDispatcher = basePlayer.EventDispatcher;

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

        _basePlayer.anim.Play(AnimTags.ATTACK2);
        _basePlayer.CancelHideStateWithRefresh();
        _basePlayer.rg.velocity = new Vector2(0, _basePlayer.rg.velocity.y);
        eventDispatcher.OnAnimationEvent += HandleAnimationAction;

        //player.InformAllSubescribe();
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
                    _basePlayer.m_FSM.TransitionStatus(actionType.run);//奔跑
                else
                    _basePlayer.m_FSM.TransitionStatus(actionType.walk);//行走
            }
        }
        else if (!attackCheck && _basePlayer.attackCollider[attackID].gameObject.activeSelf)
        {
            _basePlayer.attackCollider[attackID].gameObject.SetActive(false);
        }
        else if (attackCheck) // 攻击阶段可以缓慢移动
        {
            //攻击检测
            _basePlayer.OpenAttackCheck(attackID);

            _inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);
            _basePlayer.rg.velocity = new Vector2(_inputSpeed * moveSpeedWhenAttack, _basePlayer.rg.velocity.y);
        }

        _basePlayer.CheckOriented();
        //动画会处理Idle的跳转
    }

    public void OnExit()
    {
        //防止受伤导致角色没有关闭攻击检测
        _basePlayer.attackCollider[attackID].gameObject.SetActive(false);
        eventDispatcher.OnAnimationEvent -= HandleAnimationAction;
    }

    public void HandleAnimationAction(AnimationStage stage)
    {
        _ = stage switch
        {
            AnimationStage.SetCheckPointLeftBorder => attackCheck = true,
            AnimationStage.SetCheckPointRightBorder => attackCheck = false,
            AnimationStage.NextStageReady => isReadyForNextPhase = true,
            AnimationStage.AnimEnd => isOver = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }
}
