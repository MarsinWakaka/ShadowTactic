using UnityEngine.UI;


public class HelpPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/HelpPanel";

    public HelpPanel() : base(new UIType(path)) { }

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