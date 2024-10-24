using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始主面板
/// </summary>
public class SettingPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/SettingPanel";

    public SettingPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Close");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Pop();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Video");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Push(new VideoPanel());
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Audio");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Push(new AudioPanel());
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Help");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Push(new HelpPanel());
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Exit");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            if(GameRoot.Instance.SceneSystem.Scenetate is not StartScene)
                GameRoot.Instance.SceneSystem.SetScene(new StartScene());
            else
                Pop();
        });
    }

    protected void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}
