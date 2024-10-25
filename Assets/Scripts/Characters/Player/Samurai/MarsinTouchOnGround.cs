using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinTouchOnGround : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        private bool _isOnGround;
        
        public MarsinTouchOnGround(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
            // _owner.EventDispatcher.OnAnimationEvent += HandleAnimation;
        }


        public void OnEnter()
        {
            _owner.anim.Play(AnimTags.TOUCHGROUND);
        }

        public void OnUpdate()
        {
            if(_isOnGround)
            {
                _fsm.TransitionStatus(actionType.idle);
            }
        }

        public void OnExit()
        {
            // _owner.EventDispatcher.OnAnimationEvent -= HandleAnimation;
        }
        
        // private void HandleAnimation(AnimationStage stage)
        // {
        //     _isOnGround = stage switch
        //     {
        //         AnimationStage.animEnd => true,
        //         _ => throw new System.ArgumentException($"enum value [{stage}] dose not match any stage")
        //     };
        // }
    }
}