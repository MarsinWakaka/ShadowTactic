using System;
using UnityEngine;

//Health ,可序列化
[Serializable]
public class Health
{
    //public event Action OnAnimEvent; //声明事件

    private float hp;
    private float maxHp;
    public float Hp { get => hp; set => hp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }

    public Health(float hp, float maxHp)
    {
        if (hp > 0 && maxHp > 0)
        {
            this.Hp = hp;
            this.maxHp = maxHp;
        }
        else
        {
            Debug.LogWarning("hp or maxHP must greater than 0");
        }
    }

    public void Hurt(float damage)
    {
        if (damage < 0)
            return;

        if (damage < Hp)
        {
            Hp -= damage;
        }
        else
        {
            Hp = 0;
            //HandleDeath();
        }
    }

    public void Recover(float medic)
    {
        if (medic < 0)
            return;

        hp += medic;

        if (hp > maxHp)
            hp = maxHp;
    }

    //告诉 观察者 死亡
    //void HandleDeath()
    //{
    //    OnAnimEvent?.Invoke();
    //}
}
