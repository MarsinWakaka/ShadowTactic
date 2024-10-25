using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有UI面板的父类，包含UI面板的状态信息
/// </summary>
public class BasePanel
{
    /// <summary>
    /// UI信息
    /// </summary>
    public UIType UIType { get; private set; }
    /// <summary>
    /// UI管理工具
    /// </summary>
    public UITool UITool { get; private set; }
    /// <summary>
    /// 面板管理器
    /// </summary>
    public PanelManager PanelManager { get; private set; }
    /// <summary>
    /// UI管理器
    /// </summary>
    public UIManager UIManager { get; private set; }

    public BasePanel(UIType uIType)
    {
        UIType = uIType;
    }

    /// <summary>
    /// 初始化UITool
    /// </summary>
    /// <param name="tool"></param>
    public void Initialize(UITool tool, PanelManager panelManager, UIManager uIManager)
    {
        UITool = tool;
        PanelManager = panelManager;
        UIManager = uIManager;
    }

    ///// <summary>
    ///// 初始化面板管理器
    ///// </summary>
    ///// <param name="panelManager"></param>
    //public void Initialize(PanelManager panelManager)
    //{
    //}

    ///// <summary>
    ///// 初始化UI管理器
    ///// </summary>
    ///// <param name="uIManager"></param>
    //public void Initialize(UIManager uIManager)
    //{
    //    UIManager = uIManager;
    //}

    /// <summary>
    /// UI进入时执行的操作
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// UI进入时暂停的操作
    /// </summary>
    public virtual void OnPause() 
    {
        UITool.GetOrAddCompoent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// UI继续时执行的操作
    /// </summary>
    public virtual void OnResume() 
    {
        UITool.GetOrAddCompoent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// UI退出时执行的操作
    /// </summary>
    public virtual void OnExit()
    {
        UIManager.DestoryUI(UIType);
    }

    public void Push(BasePanel basePanel) => PanelManager?.Push(basePanel);

    public void Pop() => PanelManager?.Pop();
}
