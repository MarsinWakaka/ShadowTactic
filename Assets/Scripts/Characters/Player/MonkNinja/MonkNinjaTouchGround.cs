using System;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;

public class MonkNinjaTouchGround : IState
{
    BasePlayer _basePlayer;
    private Fsm m_FSM;
    AnimationEventDispatcher m_EventDispatcher;

    private bool isOnGround;

    public MonkNinjaTouchGround(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
        m_EventDispatcher = basePlayer.EventDispatcher;
    }

    public void OnEnter()
    {
        _basePlayer.anim.Play(AnimTags.TOUCHGROUND);
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
            AnimationStage.AnimEnd => isOnGround = true,
            _ => throw new ArgumentException(message: "enum value dosen't match any stage"),
        };
    }
}
