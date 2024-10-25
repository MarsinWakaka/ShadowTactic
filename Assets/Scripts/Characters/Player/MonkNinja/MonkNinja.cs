using TMPro;
using UnityEngine;
using Universal.AudioSystem;

namespace Characters.Player.MonkNinja
{
    public class MonkNinja : BasePlayer
    {
        [Header("[僧侣忍者特有]")]
        public GameObject kunai;
        public int kunaiNum = 10;
        [SerializeField] TextMeshProUGUI kunaiNumUI;
        [SerializeField] AudioData[] castAudio;

        protected override void Awake()
        {
            base.Awake();
            UpdateUI();
        }
        
        protected override void RegisterFsm()
        {
            base.RegisterFsm();
            m_FSM = new Fsm();
            m_FSM.Init(this, actionType.idle, new MonkNinjaIdle(this));
            m_FSM.AddAction(actionType.walk, new MonkNinjaWalk(this));
            m_FSM.AddAction(actionType.run, new MonkNinjaRun(this));
            m_FSM.AddAction(actionType.jump, new MonkNinjaJump(this));
            m_FSM.AddAction(actionType.fall, new MonkNinjaFall(this));
            m_FSM.AddAction(actionType.touchGround, new MonkNinjaTouchGround(this));
            m_FSM.AddAction(actionType.attack1, new MonkNinjaAttack1(this));
            m_FSM.AddAction(actionType.attack2, new MonkNinjaAttack2(this));
            m_FSM.AddAction(actionType.shoot, new MonkNinjaShoot(this)); //远程攻击;
            m_FSM.AddAction(actionType.skill, new MonkNinjaSkill(this));
            m_FSM.AddAction(actionType.hurt, new  MonkNinjaHurt(this));
            m_FSM.AddAction(actionType.dead, new MonkNinjaDie(this));
        }
        
        public override void AudioCallBack(ActionAudio type)
        {
            SoundManager.Instance.PlaySFX(audioSource, type switch
            {
                ActionAudio.walk => FootStepAudio(),
                ActionAudio.attack1 => attackAudio,
                ActionAudio.sheath => sheathAudio,
                ActionAudio.skill => skillAudio,
                ActionAudio.shoot => castAudio,
                ActionAudio.jump => jumpAudio,
                ActionAudio.touchground => touchGroundAudio,
                ActionAudio.hurt => hurtAudio,
                ActionAudio.dead => deadAudio,
                // 添加其他音频类型的处理
                _ => null
            });
        }
        
        public void UpdateUI()
        {
            kunaiNumUI.text = kunaiNum.ToString();
        }
    }
}