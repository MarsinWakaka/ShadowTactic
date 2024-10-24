using Assets.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 存储所有UI信息,并可以创建或者销毁UI
/// </summary>
public class UIManager
{
    private Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        if (dicUI == null)
            dicUI = new Dictionary<UIType, GameObject>();
        else
            Debug.LogWarning("UIManager已经存在");
    }

    /// <summary>
    /// 创建一个UI对象
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIType type)
    {
        GameObject parent = GameObject.Find("Canvas");
        if (!parent)
        {
            Debug.LogError("Canvas不存在，请仔细查找该对象");
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
    /// 销毁一个UI对象，并且清除UIManager对应的字典
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
