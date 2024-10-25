// using UnityEngine;
//
// namespace Characters.Player.Samurai
// {
//     public class MarsinAttack2 : IState
//     {
//         private readonly SamuraiMarsin _owner;
//         private readonly Fsm _fsm;
//         
//         private const int ATTACK_ID = 2;
//         
//         private bool _isPressAttack;
//         private bool _isPressDefence;
//         private bool _isPressJump;
//         private bool _isCheckingAttack;
//         private bool _isReadyForNextPhase;
//         private bool _isAttackActionOver;
//
//         public MarsinAttack2(SamuraiMarsin owner)
//         {
//             _owner = owner;
//             _fsm = owner.m_FSM;
//         }
//         
//         public void OnEnter()
//         {
//             _isPressAttack = false;
//             _isPressDefence = false;
//             _isPressJump = false;
//             _isCheckingAttack = false;
//             _isReadyForNextPhase = false;
//             _isAttackActionOver = false;
//             _owner.anim.Play(AnimTags.ATTACK2);
//             
//             _owner.CancelHideStateWithRefresh();
//             _owner.rg.velocity = new Vector2(_owner.rg.velocity.x / 2, _owner.rg.velocity.y);
//             _owner.EventDispatcher.OnAnimationEvent += HandleAnimationAction;
//         }
//         
//         public void OnUpdate()
//         {
//             if (_fsm.IsCeaseInput())
//             {
//                 _fsm.TransitionStatus(actionType.idle);
//                 return;
//             }
//             if (_isAttackActionOver)
//             {
//                 _fsm.TransitionStatus(actionType.idle);
//                 return;
//             }
//             
//             // 动作预输入
//             if (Input.GetButtonDown(Trigger.ATTACK)) _isPressAttack = true;
//             if (Input.GetButtonDown(Trigger.JUMP)) _isPressJump = true;
//             if (Input.GetButtonDown(Trigger.DEFENCE)) _isPressDefence = true;
//             
//             // 逻辑判断
//             // 准备好切换状态
//             if (_isReadyForNextPhase)
//             {
//                 if (_isPressAttack)
//                 {
//                     // TODO ------------------------ 及时替换为3号技能 ------------------------
//                     _fsm.TransitionStatus(actionType.attack1);
//                 }
//                 else if (_isPressDefence)
//                 {
//                     _fsm.TransitionStatus(actionType.defence);
//                 }
//                 else if (_isPressJump)
//                 {
//                     _fsm.TransitionStatus(actionType.jump);
//                 }
//                 else
//                 {
//                     _fsm.TransitionStatus(actionType.idle);
//                 }
//             }
//             // 动作进行
//             else if (_isCheckingAttack)
//             {
//                 // TODO 这里会多次触发，可能需要优化
//                 _owner.OpenAttackCheck(ATTACK_ID);
//                 // 移动
//                 var move = Input.GetAxisRaw(Axis.HORIZONTAL);
//                 _owner.rg.velocity = new Vector2(move * _owner.MoveSpeedWhenAttack, _owner.rg.velocity.y);
//             }
//             
//             _owner.CheckOriented();
//         }
//         
//         public void OnExit()
//         {
//             _owner.attackCollider[ATTACK_ID].gameObject.SetActive(false);
//             _owner.EventDispatcher.OnAnimationEvent -= HandleAnimationAction;
//         }
//         
//         private void HandleAnimationAction(AnimationStage stage)
//         {
//             switch (stage)
//             {
//                 case AnimationStage.checkHit:
//                     _isCheckingAttack = true;
//                     break;
//                 case AnimationStage.stopCheckHit:
//                     _isCheckingAttack = false;
//                     break;
//                 case AnimationStage.nextStageReady:
//                     _isReadyForNextPhase = true;
//                     break;
//                 case AnimationStage.animEnd:
//                     _isAttackActionOver = true;
//                     break;
//                 default:
//                     Debug.LogError("enum value dose not match any stage");
//                     break;
//             }
//         }
//     }
// }