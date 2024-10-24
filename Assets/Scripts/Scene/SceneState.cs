using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景管理器
/// </summary>
public abstract class SceneState
{
    /// <summary>
    /// 场景加载是操作
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 场景结束时操作
    /// </summary>
    public abstract void OnExit();
}
