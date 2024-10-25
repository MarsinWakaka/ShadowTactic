﻿using UnityEngine.UI;
using Universal.AudioSystem;

public class VideoPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/VideoPanel";

    public VideoPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Close");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            Pop();
        });
    }

    private void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}