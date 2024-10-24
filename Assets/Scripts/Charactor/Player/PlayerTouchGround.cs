using System;
using UnityEngine;

public class PlayerTouchGround : IState
{
    Player player;
    private Fsm m_FSM;
    AnimationEventDispatcher m_EventDispatcher;

    private bool isOnGround;

    public PlayerTouchGround(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
        m_EventDispatcher = player.eventDispatcher;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.TOUCHGROUND);
        m_EventDispatcher.OnAnimationEvent += HandleAnimation;

        isOnGround = false;
    }

    public void OnUpdate()
    {
        if(isOnGround)
        {
            m_FSM.TransitionStatus(actionType.idle);
        }
    }

    public void OnExit()
    {
        m_EventDispatcher.OnAnimationEvent -= HandleAnimation;
    }

    public void HandleAnimation(AnimationStage stage)
    {
        _ = stage switch
        {
            AnimationStage.animEnd => isOnGround = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }
}
