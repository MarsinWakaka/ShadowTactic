using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : IState
{
    Player player;
    Fsm m_FSM;
    AnimationEventDispatcher eventDispatcher;

    //����
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
                    player.m_FSM.TransitionStatus(actionType.run);//����
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//����
            }
        }
        else if (!attackCheck && player.AttackRange[attackID].gameObject.activeSelf)
        {
            player.AttackRange[attackID].gameObject.SetActive(false);
            player.rg.velocity = new Vector2(player.rg.velocity.x / 2, player.rg.velocity.y);
        }
        else if (attackCheck) // �����׶ο��Ի����ƶ�
        {
            //�������
            player.CheckHit(attackID);

            //_inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);

            player.rg.velocity = new Vector2(5 * player.transform.localScale.x, player.rg.velocity.y);

            //player.rg.velocity = new Vector2(_inputSpeed * moveSpeedWhenAttack, player.rg.velocity.y);

        }

        player.CheckOriented();
        //�����ᴦ��Idle����ת
    }

    public void OnExit()
    {
        //��ֹ���˵��½�ɫû�йرչ������
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
