using System;
using Characters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//Health Controller
namespace Universal.HealthSystem
{
    public class HealthController : MonoBehaviour
    {
        public Action OnHit;
    
        [FormerlySerializedAs("_initHP")] [SerializeField]
        private float initHp;
        [FormerlySerializedAs("_maxHP")] [SerializeField]
        private float maxHp;

        public Image hp;
        public Image bufferHp;
        [SerializeField]
        private float bufferTime;

        HealthBar _healthBar;
        Health _health;

        Fsm _fsm;
        BaseCharacter _owner;


        private Coroutine _coroutine;

        //private event Action OnAnimEvent; //声明事件

        // 注意继承MonoBehaviour类，构造函数会有些不同，初始化建议改为这种
        public void Init(BaseCharacter character)
        {
            _health = new Health(initHp, maxHp);
            _healthBar = new HealthBar(hp, bufferHp, bufferTime);
            _owner = character;
            _fsm = character.m_FSM;

            if (_health == null || _healthBar == null) Debug.LogWarning("_health == null || _healthBar == null");
            if (_fsm == null)
            {
                Debug.LogWarning("注意，HealthController并没有获取到状态机，请检查状态机是否初始化完成");
            }
        }


        /// <summary>
        /// 处理角色受伤逻辑
        /// </summary>
        /// <param name="damage"></param>
        public bool TryTakeDamage(float damage)
        {
            //通知受伤事件
            OnHit?.Invoke();
            //金刚不坏
            if (_owner.isMortal)
                return false;

            //伤害处理
            _health.Hurt(damage);

            //归一化处理
            float normalizedHp = _health.Hp / maxHp;

            //负责状态切换
            if (normalizedHp > 0)
                _fsm.TransitionStatus(actionType.hurt);
            else
                _fsm.TransitionStatus(actionType.dead);
        
            //通知UI变化
            InformUI(_health.Hp / maxHp);

            return true;
        }


        /// <summary>
        /// 处理角色恢复逻辑
        /// </summary>
        /// <param name="medic"></param>
        public void Recover(float medic)
        {
            _health.Recover(medic);
            InformUI(_health.Hp / maxHp);
        }


        /// <summary>
        /// 用于告知UI界面更新数据
        /// </summary>
        /// <param name="health"></param>
        private void InformUI(float health)
        {
            _healthBar.UpdateData(health);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine =  StartCoroutine(_healthBar.HPBuffer());
        }
    }
}
