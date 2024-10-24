using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//Health View
public class HealthBar
{
    private Image hp;
    private Image bufferHp;
    // HP显示组件

    //[Range(0f, 1f)]
    float _hp;
    float _bufferHp;
    // 组件对应的数据

    float _bufferTime;
    private Coroutine _coroutine;
    private float timer;

    //其它

    public HealthBar(Image hp, Image bufferHp, float bufferTime)
    {
        if (hp == null)
            Debug.LogWarning("Image : hp can't not be null");
        if (bufferHp == null)
            Debug.LogWarning("Image : bufferHp can't not be null");
        this.hp = hp;
        this.bufferHp = bufferHp;
        _hp = hp.fillAmount;
        _bufferHp = bufferHp.fillAmount;
        _bufferTime = bufferTime;
    }

    public void UpdateData(float health)
    {
        _hp = health;
        //hp.fillAmount = _hp;//暂时关闭协程功能
    }

    public IEnumerator HPBuffer()
    {
        //当前扣血时为负数
        float timer = 0f;
        float rate = 0f;
        float originHP = _bufferHp;
        do
        {
            timer += Time.deltaTime;
            rate = timer / _bufferTime;
            if (rate > 1)
                rate = 1;
            _bufferHp = Mathf.Lerp(originHP, _hp, rate);
            UpdateView();
            yield return new WaitForEndOfFrame();

        } while (rate < 1);
    }

    //更新函数

    //刷新函数
    public void UpdateView()
    {
        if(_hp < _bufferHp)
        {
            hp.fillAmount = _hp;
            bufferHp.fillAmount = _bufferHp;
        }
        else
        {
            hp.fillAmount = _bufferHp;
            bufferHp.fillAmount = _hp;
        }
        
    }
}
