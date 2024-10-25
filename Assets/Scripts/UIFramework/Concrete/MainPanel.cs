using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Universal.AudioSystem;
using Utilities;

public class MainPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/MainPanel";

    public MainPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Setting");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Push(new SettingPanel());
        });
    }

    public override void OnPause()
    {
        base.OnPause();
        GameManager.Instance.CeaseInput();
    }

    public override void OnResume()
    {
        base.OnResume();
        GameManager.Instance.StartInput();
    }

    private void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}