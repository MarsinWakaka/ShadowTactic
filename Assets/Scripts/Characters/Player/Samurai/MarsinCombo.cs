using UnityEngine;

namespace Characters.Player.Samurai
{
    public class MarsinCombo : IState
    {
        private readonly SamuraiMarsin _owner;
        private readonly Fsm _fsm;
        
        private int _comboIndex = 0;
        private const int MAX_COMBO_INDEX = 3;
        
        private bool _isPressAttack;
        private bool _isPressDefence;
        private bool _isPressJump;
        private bool _isCheckingAttack;
        private bool _isReadyForNextPhase;
        private bool _isAttackActionOver;
        
        public MarsinCombo(SamuraiMarsin owner)
        {
            _owner = owner;
            _fsm = owner.m_FSM;
        }
        
        public void OnEnter()
        {
            _comboIndex = 0;
            Reset();
            _owner.anim.Play(AnimTags.ATTACK1);
            
            _owner.CancelHideStateWithRefresh();
            _owner.rg.velocity = new Vector2(_owner.rg.velocity.x / 2, _owner.rg.velocity.y);
            _owner.EventDispatcher.OnAnimationEvent += HandleAnimationAction;
        }
        
        private void Reset()
        {
            _isPressAttack = false;
            _isPressDefence = false;
            _isPressJump = false;
            _isCheckingAttack = false;
            _isReadyForNextPhase = false;
            _isAttackActionOver = false;
        }
        
        public void OnUpdate()
        {
            if (_fsm.IsCeaseInput() || _isAttackActionOver)
            {
                _fsm.TransitionStatus(actionType.idle);
                return;
            }
            
            // 动作预输入
            if (Input.GetButtonDown(Trigger.ATTACK)) _isPressAttack = true;
            if (Input.GetButtonDown(Trigger.JUMP)) _isPressJump = true;
            if (Input.GetButtonDown(Trigger.DEFENCE)) _isPressDefence = true;
            
            // 逻辑判断
            // 准备好切换状态
            if (_isReadyForNextPhase)
            {
                if (_isPressAttack)
                {
                    // 进入下一段攻击
                    _owner.attackCollider[_comboIndex].gameObject.SetActive(false);
                    _comboIndex = (_comboIndex + 1) % MAX_COMBO_INDEX;
                    Reset();
                    switch (_comboIndex)
                    {
                        case 0:
                            _owner.anim.Play(AnimTags.ATTACK1);
                            break;
                        case 1:
                            _owner.anim.Play(AnimTags.ATTACK2);
                            break;
                        case 2:
                            _owner.anim.Play(AnimTags.ATTACK3);
                            break;
                    }
                    // _fsm.TransitionStatus(actionType.attack2);
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
            // 动作进行
            else if (_isCheckingAttack)
            {
                // TODO 这里会多次触发，可能需要优化
                _owner.OpenAttackCheck(_comboIndex);
                // 移动
                var move = Input.GetAxisRaw(Axis.HORIZONTAL);
                _owner.rg.velocity = new Vector2(move * _owner.MoveSpeedWhenAttack, _owner.rg.velocity.y);
            }
            
            _owner.CheckOriented();
        }
        
        public void OnExit()
        {
            // _basePlayer.attackCollider[attackID].gameObject.SetActive(false);
            // eventDispatcher.OnAnimationEvent -= HandleAnimationAction;
            _owner.attackCollider[_comboIndex].gameObject.SetActive(false);
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