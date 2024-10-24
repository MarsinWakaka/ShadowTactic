using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 开始主面板
/// </summary>
public class DeadPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/DeadPanel";

    public DeadPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Restart");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameManager.Instance.GameRestart();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Exit");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameRoot.Instance.SceneSystem.SetScene(new StartScene());
        });
    }

    protected void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}
