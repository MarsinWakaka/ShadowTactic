using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinJump : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        private bool _hasOffGround;
        
        public MarsinJump(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _owner.anim.Play(AnimTags.JUMP);
            _owner.rg.velocity = new Vector2(_owner.rg.velocity.x, _owner.JumpForce);
            _hasOffGround = false;
        }
        
        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput())
            {
                _fsm.TransitionStatus(actionType.idle);
                return;
            }
            
            if (_owner.rg.velocity.x < _owner.RunSpeed)
            {
                float input = Input.GetAxisRaw(Axis.HORIZONTAL);
                float jumpSpeedX = Mathf.Max(Mathf.Abs(_owner.rg.velocity.x), _owner.JumpSpeed);
                _owner.rg.velocity = new Vector2(input *jumpSpeedX, _owner.rg.velocity.y);
            }
            
            // 判断是否离开地面
            if (!_hasOffGround && !_owner.CheckOnGround())
                _hasOffGround = true;
            
            if (_hasOffGround && _owner.CheckOnGround())
            {
                _owner.m_FSM.TransitionStatus(actionType.idle);
            } 
            else if (_owner.CheckFall())
            {
                _fsm.TransitionStatus(actionType.fall);
            }
            _owner.CheckOriented();
        }

        public void OnExit()
        {
            
        }
    }
}