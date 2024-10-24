using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����¼�����Ա
/// </summary>
public class MyEventDispatcher
{
    public event Action Subscriber;

    /// <summary>
    /// �� �¼��� ��֪���� ������
    /// </summary>
    /// <param name="stage"></param>
    public void TriggerInform()
    {
        Subscriber?.Invoke();
    }
}

/// <summary>
/// �����¼�����Ա
/// </summary>
public class AnimationEventDispatcher
{
    public event Action<AnimationStage> OnAnimationEvent;
    /// <summary>
    /// �� �¼��� ��֪���� ������
    /// </summary>
    /// <param name="stage"></param>
    public void TriggerAnimationEvent(AnimationStage stage)
    {
        OnAnimationEvent?.Invoke(stage);
    }
}