using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Health Controller
public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _initHP;
    [SerializeField]
    private float _maxHP;

    public Image hp;
    public Image bufferHp;
    [SerializeField]
    private float bufferTime;

    HealthBar _healthBar;
    Health _health;

    Fsm _fsm;
    Charactor owner;


    private Coroutine _coroutine;

    //private event Action OnAnimEvent; //声明事件

    // 注意继承MonoBehaviour类，构造函数会有些不同，初始化建议改为这种
    public void Init(Charactor charactor)
    {
        _health = new Health(_initHP, _maxHP);
        _healthBar = new HealthBar(hp, bufferHp, bufferTime);
        owner = charactor;
        _fsm = charactor.m_FSM;

        if (_health == null || _healthBar == null) Debug.LogWarning("_health == null || _healthBar == null");
        if (_fsm == null)
        {
            Debug.LogWarning("注意，HealthController并没有获取到状态机，请检查状态机是否初始化完成");
        }

        //_health.OnAnimEvent += HandleOnwerDeath;//订阅 死亡事件
    }


    /// <summary>
    /// 处理角色受伤逻辑
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(float damage)
    {
        //if(_health == null) Debug.LogWarning("是否未完成初始化");

        //金刚不坏
        if (owner.isMortal)
            return;

        //伤害处理
        _health.Hurt(damage);

        //归一化处理
        float normalizedHp = _health.Hp / _maxHP;

        //负责状态切换
        if (normalizedHp > 0)
            _fsm.TransitionStatus(actionType.hurt);
        else
            _fsm.TransitionStatus(actionType.dead);
        
        //通知UI变化
        InformUI(_health.Hp / _maxHP);
    }


    /// <summary>
    /// 处理角色恢复逻辑
    /// </summary>
    /// <param name="medic"></param>
    public void Recover(float medic)
    {
        _health.Recover(medic);
        InformUI(_health.Hp / _maxHP);
    }


    /// <summary>
    /// 用于告知UI界面更新数据
    /// </summary>
    /// <param name="health"></param>
    public void InformUI(float health)
    {
        _healthBar.UpdateData(health);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine =  StartCoroutine(_healthBar.HPBuffer());
    }
}
