using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 用于获取当前激活面板及其子对象的组件
/// </summary>
public class UITool
{
    GameObject activePanel;

    /// <summary>
    /// 创建一个UI管理工具
    /// </summary>
    /// <param name="panel"></param>
    public UITool(GameObject panel)
    {
        activePanel = panel;
    }

    /// <summary>
    /// 给当前的活动面板获取或者添加一个组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddCompoent<T>() where T : Component
    {
        if(activePanel.GetComponent<T>() == null)
            activePanel.AddComponent<T>();
        return activePanel.GetComponent<T>();
    }

    /// <summary>
    /// 通过名字找到对应子对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject FindChildGameObject(string name)
    {
        Transform[] trans = activePanel.GetComponentsInChildren<Transform>();

        foreach(Transform item in trans)
        {
            if(item.name == name)
            {
                return item.gameObject;
            }
        }

        Debug.LogWarning($"{activePanel.name}里找不到名为{name}的对象");
        return null;
    }

    /// <summary>
    /// 根据名称获取一个子对象的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddComponentInChildren<T>(string name) where T : Component
    {
        GameObject child = FindChildGameObject(name);
        if(child != null)
        {
            if(child.GetComponent<T>() == null)
                child.AddComponent<T>();
            return child.GetComponent<T>();
        }
        return null;
    }

    public T[] GetAllComponentInChildren<T>() where T : Component
    {
        T[] childs = activePanel.GetComponentsInChildren<T>();
        return childs;
    }

    public void AddOnEnterListener(Button button, UnityEngine.Events.UnityAction onEnterAction)
    {
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
        if(eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry eventEntry = new EventTrigger.Entry();
        eventEntry.eventID = EventTriggerType.PointerEnter;
        eventEntry.callback.AddListener((data) => { onEnterAction.Invoke(); });
        eventTrigger.triggers.Add(eventEntry);
    }
}