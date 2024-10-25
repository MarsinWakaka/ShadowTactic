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

    //����
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

        //������ʼ��

        isReadyForNextPhase = false;
        isPressAttack = false;
        isOver = false;
    }

    public void OnEnter()
    {
        //��������
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

        //����Ԥ����
        if (!isPressAttack && Input.GetButtonDown(Trigger.ATTACK))
            isPressAttack = true;
        if (!isPressJump && Input.GetButtonDown(Trigger.JUMP))
            isPressJump = true;

        //�ж�ģ��
        if (isOver)
        {
            m_FSM.TransitionStatus(actionType.idle);
        }
        else if (isReadyForNextPhase)
        {
            if (isPressJump)       // ��Ծ(���Դ��ȡ����Щ)
            {
                m_FSM.TransitionStatus(actionType.jump);
            }
            else if (Input.GetButtonDown(Trigger.SKILL)) // ����
            {
                m_FSM.TransitionStatus(actionType.skill);
            }
            else if (Input.GetButtonDown(Trigger.SHOOT)) // ���
            {
                m_FSM.TransitionStatus(actionType.shoot);
            }
            else if (isPressAttack)// ���¹���������ǰ����
            {
                m_FSM.TransitionStatus(actionType.attack1);//��ת����2
            }
            else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    _basePlayer.m_FSM.TransitionStatus(actionType.run);//����
                else
                    _basePlayer.m_FSM.TransitionStatus(actionType.walk);//����
            }
        }
        else if (!attackCheck && _basePlayer.attackCollider[attackID].gameObject.activeSelf)
        {
            _basePlayer.attackCollider[attackID].gameObject.SetActive(false);
        }
        else if (attackCheck) // �����׶ο��Ի����ƶ�
        {
            //�������
            _basePlayer.OpenAttackCheck(attackID);

            _inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);
            _basePlayer.rg.velocity = new Vector2(_inputSpeed * moveSpeedWhenAttack, _basePlayer.rg.velocity.y);
        }

        _basePlayer.CheckOriented();
        //�����ᴦ��Idle����ת
    }

    public void OnExit()
    {
        //��ֹ���˵��½�ɫû�йرչ������
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
