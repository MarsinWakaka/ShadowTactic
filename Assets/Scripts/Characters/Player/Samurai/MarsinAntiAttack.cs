using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinAntiAttack : IState
    {
        private const int COMBO_INDEX = 3;
        
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        private bool _isPressAttack;
        private bool _isPressDefence;
        private bool _isPressJump;
        private bool _isCheckingAttack;
        private bool _isReadyForNextPhase;
        private bool _isAttackActionOver;
        
        public MarsinAntiAttack(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            // 重置参数
            _isPressAttack = false;
            _isPressDefence = false;
            _isPressJump = false;
            _isCheckingAttack = false;
            _isReadyForNextPhase = false;
            _isAttackActionOver = false;
            
            _owner.anim.Play(AnimTags.ANTI_ATTACK);
            _owner.rg.velocity = new Vector2(_owner.rg.velocity.x / 2, _owner.rg.velocity.y);
            _owner.EventDispatcher.OnAnimationEvent += HandleAnimationAction;
        }

        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput() || _isAttackActionOver)
            {
                _fsm.TransitionStatus(actionType.idle);
                return;
            }
            
            if (Input.GetButtonDown(Trigger.ATTACK)) _isPressAttack = true;
            if (Input.GetButtonDown(Trigger.JUMP)) _isPressJump = true;
            if (Input.GetButtonDown(Trigger.DEFENCE)) _isPressDefence = true;
            
            // 逻辑判断
            // 准备好切换状态
            if (_isReadyForNextPhase)
            {
                if (_isPressAttack)
                {
                    _fsm.TransitionStatus(actionType.attack1);
                }
                else if (_isPressDefence)
                {
                    _fsm.TransitionStatus(actionType.defence);
                }
                else if (_isPressJump)
                {
                    _fsm.TransitionStatus(actionType.jump);
                }
                else if (Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
                {
                    if (Input.GetButton(Trigger.RUN))
                        _owner.m_FSM.TransitionStatus(actionType.run); //奔跑
                    else
                        _owner.m_FSM.TransitionStatus(actionType.walk); //行走
                }
            }
            else if (_isCheckingAttack)
            {
                var moveInput = Input.GetAxisRaw(Axis.HORIZONTAL);
                _owner.OpenAttackCheck(COMBO_INDEX);
                _owner.rg.velocity = new Vector2(moveInput * _owner.MoveSpeedWhenAttack * 2, _owner.rg.velocity.y);
            }
            
            _owner.CheckOriented();
        }

        public void OnExit()
        {
            _owner.attackCollider[COMBO_INDEX].gameObject.SetActive(false);
            _owner.EventDispatcher.OnAnimationEvent -= HandleAnimationAction;
        }
        
        private void HandleAnimationAction(AnimationStage stage)
        {
            switch (stage)
            {
                case AnimationStage.SetCheckPointLeftBorder:
                    _isCheckingAttack = true;
                    break;
                case AnimationStage.SetCheckPointRightBorder:
                    _isCheckingAttack = false;
                    break;
                case AnimationStage.NextStageReady:
                    _isReadyForNextPhase = true;
                    break;
                case AnimationStage.AnimEnd:
                    _isAttackActionOver = true;
                    break;
                default:
                    Debug.LogError("enum value dose not match any stage");
                    break;
            }
        }
    }
}