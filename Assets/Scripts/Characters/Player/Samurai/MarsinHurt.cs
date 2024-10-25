

using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinHurt : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        public MarsinHurt(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.anim.Play(AnimTags.HURT);
            _owner.rg.velocity = new Vector2(_owner.rg.velocity.x, _owner.rg.velocity.y);
            var hitFx = GameObject.Instantiate(_owner.HurtFX, _owner.transform.position, _owner.transform.rotation);
            
            // 随机旋转角度
            float randomRotation = Random.Range(0, 360);
            hitFx.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
            
        }

        // 状态由动画事件调用
        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}