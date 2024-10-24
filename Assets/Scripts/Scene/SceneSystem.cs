using System.Collections;
using UnityEngine;

/// <summary>
/// 场景的状态管理系统
/// </summary>
public class SceneSystem
{
    /// <summary>
    /// 场景状态类
    /// </summary>
    SceneState scenetate;

    public SceneState Scenetate { get; private set; }

    /// <summary>
    /// 设置当前场景并进入当前场景
    /// </summary>
    /// <param name="state"></param>
    public void SetScene(SceneState state)
    {
        //Debug.Log($"HashCode为 {this.GetHashCode()} 的场景正要执行SetScene函数,下一场景为{state}");
        Scenetate?.OnExit();//点击StartMenu后为什么没有MainMennu.OnExit()
        Scenetate = state;
        Scenetate?.OnEnter();
    }
}