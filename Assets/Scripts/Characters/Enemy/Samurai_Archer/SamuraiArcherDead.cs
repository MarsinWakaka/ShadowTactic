using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SamuraiArcherDead : IState
{
    //所需组件
    SamuraiArcher owner;
    Fsm _Fsm;

    //所需参数

    public SamuraiArcherDead(SamuraiArcher onwer)
    {

        this.owner = onwer;
        _Fsm = owner.m_FSM;

        //参数初始化
    }

    public void OnEnter()
    {
        _Fsm.CeaseFSM();
        owner.anim.Play(AnimTags.DIE);

        GameObject go = GameObject.Instantiate(owner.HurtFX, owner.transform.position, owner.transform.rotation);
        // 随机旋转角度
        float randomRotation = UnityEngine.Random.Range(0, 360);
        go.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        owner.viewer.GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
