using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public abstract class SceneState
{
    /// <summary>
    /// ���������ǲ���
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// ��������ʱ����
    /// </summary>
    public abstract void OnExit();
}
