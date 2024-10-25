using UnityEngine;
using Universal.AudioSystem;

namespace Characters.Player.Samurai
{
    public class SamuraiMarsin : BasePlayer
    {
        [SerializeField] protected AudioData[] defenceAudio;
        
        protected override void RegisterFsm()
        {
            base.RegisterFsm();
            m_FSM = new Fsm();
            m_FSM.Init(this, actionType.idle, new MarsinIdle(this));
            m_FSM.AddAction(actionType.walk, new MarsinWalk(this));
            m_FSM.AddAction(actionType.run, new MarsinRun(this));
            m_FSM.AddAction(actionType.jump, new MarsinJump(this));
            m_FSM.AddAction(actionType.fall, new MarsinFall(this));
            m_FSM.AddAction(actionType.touchGround, new MarsinTouchOnGround(this));
            m_FSM.AddAction(actionType.hurt, new MarsinHurt(this));
            m_FSM.AddAction(actionType.dead, new MarsinDie(this));
            m_FSM.AddAction(actionType.attack1, new MarsinCombo(this));
            m_FSM.AddAction(actionType.defence, new MarsinDefence(this));
            m_FSM.AddAction(actionType.defenceSuccess, new MarsinDefenceSuccess(this));
            m_FSM.AddAction(actionType.antiAttack, new MarsinAntiAttack(this));
        }
        
        public override void AudioCallBack(ActionAudio type)
        {
            SoundManager.Instance.PlaySFX(audioSource, type switch
            {
                ActionAudio.walk => FootStepAudio(),
                ActionAudio.attack1 => attackAudio,
                ActionAudio.sheath => sheathAudio,
                ActionAudio.skill => skillAudio,
                // actionAudio.shoot => castAudio,
                ActionAudio.jump => jumpAudio,
                ActionAudio.touchground => touchGroundAudio,
                ActionAudio.hurt => hurtAudio,
                ActionAudio.dead => deadAudio,
                ActionAudio.defence => defenceAudio,
                // 添加其他音频类型的处理
                _ => null
            });
        }
    }
}