using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 动画事件调度员
/// </summary>
public class MyEventDispatcher
{
    public event Action Subscriber;

    /// <summary>
    /// 将 事件名 告知所有 订阅者
    /// </summary>
    /// <param name="stage"></param>
    public void TriggerInform()
    {
        Subscriber?.Invoke();
    }
}

/// <summary>
/// 动画事件调度员
/// </summary>
public class AnimationEventDispatcher
{
    public event Action<AnimationStage> OnAnimationEvent;
    /// <summary>
    /// 将 事件名 告知所有 订阅者
    /// </summary>
    /// <param name="stage"></param>
    public void TriggerAnimationEvent(AnimationStage stage)
    {
        OnAnimationEvent?.Invoke(stage);
    }
}