using UnityEngine;
using UnityEngine.UI;
using Universal.AudioSystem;
using Utilities;

/// <summary>
/// 开始主面板
/// </summary>
public class DialogPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/DialogPanel";

    private Text chatBox;

    public DialogPanel() : base(new UIType(path)) { }

    public override void OnEnter()
    {
        //对话框弹出时，暂停游戏
        GameManager.Instance.StopGame();

        chatBox = UITool.GetOrAddComponentInChildren<Text>("文字生成");
        chatBox.text = DialogueManager.Instance.GetCurrentText();

        Button btn;
        btn = UITool.GetOrAddComponentInChildren<Button>("Next");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            if (!DialogueManager.Instance.NextPage(chatBox))
                Pop();
        });

        btn = UITool.GetOrAddComponentInChildren<Button>("Pre");
        UITool.AddOnEnterListener(btn, OnEnterAction);
        btn.onClick.AddListener(() =>
        {
            string text = DialogueManager.Instance.PreviousPage();
            if (text != null)
                chatBox.text = text;
        });
    }

    /// <summary>
    /// UI退出时执行的操作
    /// </summary>
    public override void OnExit()
    {
        //
        GameManager.Instance.ContinueGame();

        UIManager.DestoryUI(UIType);
    }

    private void OnEnterAction()
    {
        SoundManager.Instance.PlayUIEnterSFX();
    }
}
