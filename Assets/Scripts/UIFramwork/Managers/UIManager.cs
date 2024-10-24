using Assets.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �洢����UI��Ϣ,�����Դ�����������UI
/// </summary>
public class UIManager
{
    private Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        if (dicUI == null)
            dicUI = new Dictionary<UIType, GameObject>();
        else
            Debug.LogWarning("UIManager�Ѿ�����");
    }

    /// <summary>
    /// ����һ��UI����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIType type)
    {
        GameObject parent = GameObject.Find("Canvas");
        if (!parent)
        {
            Debug.LogError("Canvas�����ڣ�����ϸ���Ҹö���");
            return null;
        }
        if (dicUI.ContainsKey(type))
            return dicUI[type];
        GameObject ui = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), parent.transform);
        ui.name = type.Name;
        dicUI.Add(type, ui);
        return ui;
    }

    /// <summary>
    /// ����һ��UI���󣬲������UIManager��Ӧ���ֵ�
    /// </summary>
    /// <param name="type"></param>
    public void DestoryUI(UIType type)
    {
        if (dicUI.ContainsKey(type))
        {
            GameObject.Destroy(dicUI[type]);
            dicUI.Remove(type);
        }
    }
}
