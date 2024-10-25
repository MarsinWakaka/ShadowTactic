using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinDie : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        public MarsinDie(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _fsm.CeaseFSM();
            _owner.anim.Play(AnimTags.DIE);
            var go = GameObject.Instantiate(_owner.HurtFX, _owner.transform.position, _owner.transform.rotation);
            go.transform.rotation = new Quaternion(0, 0, UnityEngine.Random.Range(0, 360), 0);
            
            _owner.SetCharacterDie();
        }

        public void OnUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}