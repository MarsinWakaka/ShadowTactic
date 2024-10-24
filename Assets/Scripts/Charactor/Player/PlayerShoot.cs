using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : IState
{
    Player player;
    Fsm m_FSM;
    AnimationEventDispatcher eventDispatcher;

    GameObject kunai;
    private bool isOver;
    private bool isReady;
    private bool isPressShoot;

    float moveSpeedWhenAttack;

    [SerializeField]
    //�ֶ�

    public PlayerShoot(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
        this.eventDispatcher = player.eventDispatcher;

        //������ʼ��
        kunai = player.kunai;
        isOver = false;
        isReady = false;
        isPressShoot = false;

        moveSpeedWhenAttack = player.MoveSpeedWhenAttack;
    }

    public void OnEnter()
    {
        //��������
        isOver = false;
        isReady = false;
        isPressShoot=false;

        player.anim.Play(AnimTags.ShOOT);
        eventDispatcher.OnAnimationEvent += HandleAnimation;

        if(player.kunaiNum > 0)
        {
            //�������
            GameObject kunaiObj = GameObject.Instantiate(kunai, player.transform.position, player.transform.localRotation);
            kunaiObj.transform.localScale = player.transform.localScale;
            player.kunaiNum--;
            player.UpdateUI();
        }
        //player.CheckHit();
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

        //����Ԥ����
        if (Input.GetButtonDown(Trigger.SHOOT))
            isPressShoot = true;

        //�ж�ģ��
        if (isReady)
        {
            if (Input.GetButtonDown(Trigger.JUMP))//��Ծ(���Դ��ȡ����Щ)
            {
                m_FSM.TransitionStatus(actionType.jump);
            }
            else if (Input.GetButtonDown(Trigger.SKILL)) // ����
            {
                m_FSM.TransitionStatus(actionType.skill);
            }
            else if (Input.GetButtonDown(Trigger.ATTACK))
            {
                m_FSM.TransitionStatus(actionType.attack1);//��ת����1
            }
            


            else if (isPressShoot)// ���¹���������ǰ����,������Ԥ���룬�������ȶ���Ҫ���ͣ����д��Ԥ����
            {
                m_FSM.TransitionStatus(actionType.shoot);//�������
            }
            else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);//����
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//����
            }

            else if (isOver)
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        else
        {
            float _inputSpeed = Input.GetAxisRaw(Axis.HORIZONTAL);
            player.rg.velocity = new Vector2(_inputSpeed * moveSpeedWhenAttack, player.rg.velocity.y);
            player.CheckOriented();
        }

        //�����ᴦ��Idle����ת
    }

    public void OnExit()
    {
        eventDispatcher.OnAnimationEvent -= HandleAnimation;
    }

    public void HandleAnimation(AnimationStage stage)
    {
        if (stage.Equals(AnimationStage.nextStageReady))
        {
            isReady = true;
        }
        else if (stage.Equals(AnimationStage.animEnd))
        {
            isOver = true;
        }
            
        // ��֧��������״̬
    }
}