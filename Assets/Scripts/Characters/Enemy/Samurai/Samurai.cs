using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using Characters.Enemy.Samurai;
using UnityEngine;
using Universal.AudioSystem;

public class Samurai : Enemy
{
    [Header("\n继承专属\n___________ 防御 ___________")]

    public float near_range_enabled_dist; //近战，追逐切换的临界距离
    [SerializeField] float defenceCoolTime;
    [SerializeField] AudioData[] defenceAudio;

    public float DefenceCoolTime { get => defenceCoolTime; set => defenceCoolTime = value; }

    protected override void Update()
    {
        m_FSM.Execute();
    }

    /// <summary>
    /// 控制声音的播放
    /// </summary>
    /// <param name="type"></param>
    public override void AudioCallBack(ActionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource, type switch
        {
            ActionAudio.walk => patrolAudio,
            ActionAudio.run => chaseAudio,
            ActionAudio.attack1 => attackAudio,
            ActionAudio.defence => defenceAudio,
            ActionAudio.hurt => hurtAudio,
            ActionAudio.dead => deadAudio,
            // 添加其他音频类型的处理
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
        // 添加僵直
        m_FSM.AddAction(actionType.stiff, new EnemyStiff(this));
    }

    public bool CheckDefence()
    {
        if(Input.GetButtonDown(Trigger.ATTACK))
        {
            return Random.Range(0, 3) <= 1;
        }
        return false;
    }

    [SerializeField] DestoryFX door;

    public void OnDestroy()
    {
        if (door == null)
            return;
        door.enabled = true;
    }
}
