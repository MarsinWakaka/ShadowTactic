using Characters.Enemy;
using UnityEngine;
using Universal.AudioSystem;

public class SamuraiArcher : Enemy
{
    [Header("\n�̳�ר��\n___________ ���� ___________")]
    public GameObject arrow;
    public float near_range_enabled_dist; //��ս��Զ���л����ٽ����
    [SerializeField] float distRangeWeapon;
    [SerializeField] AudioData[] pullBowAudio;
    [SerializeField] AudioData[] shootAudio;
    [Header("����")]
    [SerializeField] AudioData[] defenceAudio;

    public float rangeWeaponDist { get => distRangeWeapon; set => distRangeWeapon = value; }

    public new void AnimationEventCallback(AnimationStage stage)
    {
        EventDispatcher.TriggerAnimationEvent(stage);
    }

    /// <summary>
    /// ���������Ĳ���
    /// </summary>
    /// <param name="type"></param>
    public override void AudioCallBack(ActionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource, type switch
        {
            ActionAudio.walk => patrolAudio,
            ActionAudio.run => chaseAudio,
            ActionAudio.attack1 => attackAudio,
            ActionAudio.bow => pullBowAudio,
            ActionAudio.shoot => shootAudio,
            ActionAudio.defence => defenceAudio,
            ActionAudio.hurt => hurtAudio,
            ActionAudio.dead => deadAudio,
            // ���������Ƶ���͵Ĵ���
            _ => null
        });
    }

    protected override void RegisterFSM()
    {
        m_FSM = new Fsm();
        m_FSM.Init(this, actionType.idle, new SamuraiArcherIdle(this));
        m_FSM.AddAction(actionType.walk, new SamuraiArcherPatrol(this));
        m_FSM.AddAction(actionType.run, new SamuraiArcherChase(this));
        m_FSM.AddAction(actionType.attack1, new SamuraiArcherAttack1(this));
        m_FSM.AddAction(actionType.attack2, new SamuraiArcherAttack2(this));
        m_FSM.AddAction(actionType.attack3, new SamuraiArcherAttack3(this));
        m_FSM.AddAction(actionType.shoot, new SamuraiArcherShoot(this));
        m_FSM.AddAction(actionType.hurt, new SamuraiArcherHurt(this));
        m_FSM.AddAction(actionType.dead, new  SamuraiArcherDead(this));
        
        // ��ӽ�ֱ
        m_FSM.AddAction(actionType.stiff, new EnemyStiff(this));
    }
}
