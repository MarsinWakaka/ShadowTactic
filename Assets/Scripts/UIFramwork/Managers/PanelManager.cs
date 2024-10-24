using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ջ�洢UI
/// </summary>
public class PanelManager
{
    /// <summary>
    /// �洢 BasePanel ��ջ
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
    /// UI����ջ�������˲����������ʾһ�����
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel)
    {
        //���ջ��������Panel����ʹ���������ֹͣ��Ȼ�������ջ����
        if (stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.OnPause();
        }
        stackPanel.Push(nextPanel);

        //��ʼ��
        //��ȡnextPanel��Ӧ��GameObject��������Ϊ��ʹ��UITool��GetComponent���           ��������
        GameObject panelGo = uiManager.GetSingleUI(nextPanel.UIType);
        nextPanel.Initialize(new UITool(panelGo), this, uiManager);

        nextPanel.OnEnter();
    }

    /// <summary>
    /// ִ�����ĳ�ջ������ ִ������OneExit()����
    /// </summary>
    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnExit();//��ִ��ջ�����˳�����
            stackPanel.Pop();//����ջ��Ԫ��
        }

        if (stackPanel.Count > 0)
            stackPanel.Peek().OnResume();
    }

    public void PopAll()
    {
        stackPanel?.Clear();
        //Debug.Log($"stackPanel��պ����:{stackPanel.Count}");
        //while (stackPanel.Count > 0) stackPanel.Pop();
    }
}
