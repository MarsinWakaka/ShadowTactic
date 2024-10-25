using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����UI���ĸ��࣬����UI����״̬��Ϣ
/// </summary>
public class BasePanel
{
    /// <summary>
    /// UI��Ϣ
    /// </summary>
    public UIType UIType { get; private set; }
    /// <summary>
    /// UI������
    /// </summary>
    public UITool UITool { get; private set; }
    /// <summary>
    /// ��������
    /// </summary>
    public PanelManager PanelManager { get; private set; }
    /// <summary>
    /// UI������
    /// </summary>
    public UIManager UIManager { get; private set; }

    public BasePanel(UIType uIType)
    {
        UIType = uIType;
    }

    /// <summary>
    /// ��ʼ��UITool
    /// </summary>
    /// <param name="tool"></param>
    public void Initialize(UITool tool, PanelManager panelManager, UIManager uIManager)
    {
        UITool = tool;
        PanelManager = panelManager;
        UIManager = uIManager;
    }

    ///// <summary>
    ///// ��ʼ����������
    ///// </summary>
    ///// <param name="panelManager"></param>
    //public void Initialize(PanelManager panelManager)
    //{
    //}

    ///// <summary>
    ///// ��ʼ��UI������
    ///// </summary>
    ///// <param name="uIManager"></param>
    //public void Initialize(UIManager uIManager)
    //{
    //    UIManager = uIManager;
    //}

    /// <summary>
    /// UI����ʱִ�еĲ���
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// UI����ʱ��ͣ�Ĳ���
    /// </summary>
    public virtual void OnPause() 
    {
        UITool.GetOrAddCompoent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// UI����ʱִ�еĲ���
    /// </summary>
    public virtual void OnResume() 
    {
        UITool.GetOrAddCompoent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// UI�˳�ʱִ�еĲ���
    /// </summary>
    public virtual void OnExit()
    {
        UIManager.DestoryUI(UIType);
    }

    public void Push(BasePanel basePanel) => PanelManager?.Push(basePanel);

    public void Pop() => PanelManager?.Pop();
}
