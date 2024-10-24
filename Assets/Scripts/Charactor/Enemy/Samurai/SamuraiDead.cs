using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SamuraiDead : IState
{
    //�������
    Samurai owner;
    Fsm _Fsm;

    //�������

    public SamuraiDead(Samurai onwer)
    {

        this.owner = onwer;
        _Fsm = owner.m_FSM;

        //������ʼ��
    }

    public void OnEnter()
    {
        _Fsm.CeaseFSM();
        owner.anim.Play(AnimTags.DEAD);

        GameObject go = GameObject.Instantiate(owner.HurtFX, owner.transform.position, owner.transform.rotation);
        // �����ת�Ƕ�
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
