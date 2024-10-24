using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//注意states可能没法正常序列化
[Serializable]
public class Fsm
{
    private MonoBehaviour owner;
    Dictionary<actionType, IState> states;
    private bool isCeaseInput = true;
    private IState currentState;

    /// <summary>
    /// 状态机的初始化,需要默认参数,
    /// </summary>
    /// <param name="defaultState"></param>
    public void Init(MonoBehaviour owner,actionType defaultAction, IState defaultState)
    {
        if (defaultState == null)
        {
            Debug.LogWarning("defaultState can't be null");
        }
        if(owner == null)
        {
            Debug.LogWarning("owner can't be null");
        }
        this.owner = owner;

        states = new Dictionary<actionType, IState>();
        AddAction(defaultAction, defaultState);
        currentState = defaultState;

        StartFSM();
    }

    /// <summary>
    /// 往状态机里添加行为
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public void AddAction(actionType action, IState state)
    {
        if (isContained(action))
            Debug.LogWarning("already poccessed nextState of" +  state.ToString());

        states.Add(action, state);
    }

    public void Execute()
    {
        //if (isCeaseInput)// 确保完成初始化
        //    return;

        currentState.OnUpdate();
    }

    public bool IsCeaseInput()
    {
        return isCeaseInput;
    }

    /// <summary>
    /// 执行状态的切换，输入想要切换为的状态
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionStatus(actionType nextState)
    {
        //if (owner is not Player)
        //    Debug.Log($"{owner}\t的当前状态 ：" + nextState);
        //else if (nextState == actionType.attack1)
        //    Debug.Log("Player attack");

        if (!isContained(nextState))
        {
            Debug.LogWarning($"{owner}'s currentState is {currentState}, Transition status {nextState.ToString()} fail");
        }
        currentState.OnExit();
        currentState = states[nextState];
        currentState.OnEnter();
    }

    public IState GetState(actionType action)
    {
        return states[action];
    }

    /// <summary>
    /// 检查是否含有重复状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private bool isContained(actionType state)
    {
        if(states.ContainsKey(state))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 停止状态机
    /// </summary>
    public void CeaseFSM()
    {
        isCeaseInput = true;
    }

    /// <summary>
    /// 启动状态机
    /// </summary>
    public void StartFSM()
    {
        isCeaseInput = false;
    }
}
