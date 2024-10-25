using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板管理器，用栈存储UI
/// </summary>
public class PanelManager
{
    /// <summary>
    /// 存储 BasePanel 的栈
    /// </summary>
    private Stack<BasePanel> stackPanel;

    private UIManager uiManager;
    private BasePanel panel;

    public PanelManager()
    {
        stackPanel = new Stack<BasePanel>();
        uiManager = new UIManager();
    }

    /// <summary>
    /// UI的入栈操作，此操作界面会显示一个面板
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel)
    {
        //如果栈内有其它Panel，先使顶部的面板停止，然后进行入栈操作
        if (stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.OnPause();
        }
        stackPanel.Push(nextPanel);

        //初始化
        //获取nextPanel对应的GameObject，仅仅是为了使用UITool的GetComponent组件           ？？？？
        GameObject panelGo = uiManager.GetSingleUI(nextPanel.UIType);
        nextPanel.Initialize(new UITool(panelGo), this, uiManager);

        nextPanel.OnEnter();
    }

    /// <summary>
    /// 执行面板的出栈操作， 执行面板的OneExit()方法
    /// </summary>
    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnExit();//先执行栈顶的退出函数
            stackPanel.Pop();//弹出栈顶元素
        }

        if (stackPanel.Count > 0)
            stackPanel.Peek().OnResume();
    }

    public void PopAll()
    {
        stackPanel?.Clear();
        //Debug.Log($"stackPanel清空后个数:{stackPanel.Count}");
        //while (stackPanel.Count > 0) stackPanel.Pop();
    }
}
