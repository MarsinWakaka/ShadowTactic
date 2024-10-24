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
    //字段

    public PlayerShoot(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
        this.eventDispatcher = player.eventDispatcher;

        //变量初始化
        kunai = player.kunai;
        isOver = false;
        isReady = false;
        isPressShoot = false;

        moveSpeedWhenAttack = player.MoveSpeedWhenAttack;
    }

    public void OnEnter()
    {
        //变量重置
        isOver = false;
        isReady = false;
        isPressShoot=false;

        player.anim.Play(AnimTags.ShOOT);
        eventDispatcher.OnAnimationEvent += HandleAnimation;

        if(player.kunaiNum > 0)
        {
            //发射飞镖
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

        //动作预输入
        if (Input.GetButtonDown(Trigger.SHOOT))
            isPressShoot = true;

        //判断模块
        if (isReady)
        {
            if (Input.GetButtonDown(Trigger.JUMP))//跳跃(可以打断取消这些)
            {
                m_FSM.TransitionStatus(actionType.jump);
            }
            else if (Input.GetButtonDown(Trigger.SKILL)) // 大招
            {
                m_FSM.TransitionStatus(actionType.skill);
            }
            else if (Input.GetButtonDown(Trigger.ATTACK))
            {
                m_FSM.TransitionStatus(actionType.attack1);//跳转动作1
            }
            


            else if (isPressShoot)// 按下攻击键或提前按下,由于是预输入，所以优先度需要降低，运行打断预输入
            {
                m_FSM.TransitionStatus(actionType.shoot);//继续射击
            }
            else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);//奔跑
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//行走
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

        //动画会处理Idle的跳转
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
            
        // 分支处理其它状态
    }
}