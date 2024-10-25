using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

/// <summary>
/// 管理全局的一些东西，单例
/// </summary>
public class GameRoot : PersistentSingleton<GameRoot>
{
    ///// <summary>
    ///// 场景管理器
    ///// </summary>
    public SceneSystem SceneSystem { get; private set; }

    /// <summary>
    /// 显示一个面板
    /// </summary>
    public UnityAction<BasePanel> Push { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SceneSystem = new SceneSystem();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneSystem.SetScene(new StartScene());
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond); // 使用不同的种子值
        GameManager.Instance.SetFrameRate(60);
    }

    public void SetAction(UnityAction<BasePanel> push)
    {
        Push = push;
    }
}