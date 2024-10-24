using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ע��states����û���������л�
[Serializable]
public class Fsm
{
    private MonoBehaviour owner;
    Dictionary<actionType, IState> states;
    private bool isCeaseInput = true;
    private IState currentState;

    /// <summary>
    /// ״̬���ĳ�ʼ��,��ҪĬ�ϲ���,
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
    /// ��״̬���������Ϊ
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
        //if (isCeaseInput)// ȷ����ɳ�ʼ��
        //    return;

        currentState.OnUpdate();
    }

    public bool IsCeaseInput()
    {
        return isCeaseInput;
    }

    /// <summary>
    /// ִ��״̬���л���������Ҫ�л�Ϊ��״̬
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionStatus(actionType nextState)
    {
        //if (owner is not Player)
        //    Debug.Log($"{owner}\t�ĵ�ǰ״̬ ��" + nextState);
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
    /// ����Ƿ����ظ�״̬
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
    /// ֹͣ״̬��
    /// </summary>
    public void CeaseFSM()
    {
        isCeaseInput = true;
    }

    /// <summary>
    /// ����״̬��
    /// </summary>
    public void StartFSM()
    {
        isCeaseInput = false;
    }
}
