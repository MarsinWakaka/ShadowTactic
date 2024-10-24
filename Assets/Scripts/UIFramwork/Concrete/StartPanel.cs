using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 开始主面板
/// </summary>
public class StartPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/StartPanel";

    public StartPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Setting");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() => 
        {
            Push(new SettingPanel());
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Level1");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameRoot.Instance.SceneSystem.SetScene(new Level1Scene());
            SoundManager.Instance.PlayUIChooseSFX();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Level2");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameRoot.Instance.SceneSystem.SetScene(new Level2Scene());
            SoundManager.Instance.PlayUIChooseSFX();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Level3");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameRoot.Instance.SceneSystem.SetScene(new Level3Scene());
            SoundManager.Instance.PlayUIChooseSFX();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Boss");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            GameRoot.Instance.SceneSystem.SetScene(new Level4Scene());
            SoundManager.Instance.PlayUIChooseSFX();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Exit");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}
