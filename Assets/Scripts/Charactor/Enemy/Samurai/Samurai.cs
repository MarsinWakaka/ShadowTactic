using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : Enemy
{
    [Header("\n�̳�ר��\n___________ ���� ___________")]

    public float near_range_enabled_dist; //��ս��׷���л����ٽ����
    [SerializeField] float defenceCoolTime;
    [SerializeField] AudioData[] defenceAudio;

    public float DefenceCoolTime { get => defenceCoolTime; set => defenceCoolTime = value; }

    protected override void Update()
    {
        m_FSM.Execute();
    }

    /// <summary>
    /// ���������Ĳ���
    /// </summary>
    /// <param name="type"></param>
    public override void AudioCallBack(actionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource, type switch
        {
            actionAudio.walk => patrolAudio,
            actionAudio.run => chaseAudio,
            actionAudio.attack1 => attackAudio,
            actionAudio.defence => defenceAudio,
            actionAudio.hurt => hurtAudio,
            actionAudio.dead => deadAudio,
            // ���������Ƶ���͵Ĵ���
            _ => null
        });
    }

    protected override void RegisterFSM()
    {
        m_FSM = new Fsm();
        m_FSM.Init(this, actionType.idle, new SamuraiIdle(this));
        m_FSM.AddAction(actionType.walk, new SamuraiPatrol(this));
        m_FSM.AddAction(actionType.run, new SamuraiChase(this));
        m_FSM.AddAction(actionType.attack1, new SamuraiAttack1(this));
        m_FSM.AddAction(actionType.attack2, new SamuraiAttack2(this));
        m_FSM.AddAction(actionType.attack3, new SamuraiAttack3(this));
        m_FSM.AddAction(actionType.defence, new SamuraiDefence(this));
        m_FSM.AddAction(actionType.hurt, new SamuraiHurt(this));
        m_FSM.AddAction(actionType.dead, new SamuraiDead(this));
    }

    public bool CheckDefence()
    {
        if(Input.GetButtonDown(Trigger.ATTACK))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [SerializeField] DestoryFX door;

    public void OnDestory()
    {
        if (door == null)
            return;
        door.enabled = true;
    }
}
