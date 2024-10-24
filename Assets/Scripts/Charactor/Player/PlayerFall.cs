using System;
using UnityEngine;

public class PlayerFall : IState
{
    Player player;
    Fsm m_FSM;
    private float jumpSpeed;
    private float lastVelocity;

    [SerializeField]
    //private bool isOnGround;

    public PlayerFall(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.FALL);
        jumpSpeed = Mathf.Max(Mathf.Abs(player.rg.velocity.x), player.JumpSpeed);
        //isOnGround = false;
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

        //��Ծ�����ڼ����
        if (player.rg.velocity.x < player.RunSpeed)
        {
            float input = Input.GetAxisRaw(Axis.HORIZONTAL);
            player.rg.velocity = new Vector2(input * jumpSpeed, player.rg.velocity.y);
        }
        player.CheckOriented();

        if(player.CheckOnGround())
        {
            if (lastVelocity < -10f)
                m_FSM.TransitionStatus(actionType.touchGround);
            else if(Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    player.m_FSM.TransitionStatus(actionType.run);//����
                else
                    player.m_FSM.TransitionStatus(actionType.walk);//����
            }
            else
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        //�˳�������TouchGround�������һ֡�Ĵ����¼�������Player TouchGround����

        lastVelocity = player.rg.velocity.y;
    }

    public void OnExit()
    {
        
    }
}
