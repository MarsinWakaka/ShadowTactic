using UnityEngine;
using Utilities;

namespace Characters.Enemy.Samurai
{
    public class SamuraiHurt : IState
    {
        //�������
        global::Samurai owner;
        Fsm _Fsm;
        AnimationEventDispatcher eventDispatcher;

        //�������
        private float timer;
        private float waitTime;
        private bool isOver;

        public SamuraiHurt(global::Samurai onwer)
        {

            this.owner = onwer;
            _Fsm = owner.m_FSM;
            eventDispatcher = owner.EventDispatcher;

            //������ʼ��
            isOver = false;

        
        }

        public void OnEnter()
        {
            owner.anim.Play(AnimTags.HURT);
            GameObject go = GameObject.Instantiate(owner.HurtFX, owner.transform.position, owner.transform.rotation);
            // �����ת�Ƕ�
            float randomRotation = UnityEngine.Random.Range(0, 360);
            go.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

            eventDispatcher.OnAnimationEvent += HandleAnimation;

            isOver = false;

            //û�ҵ�Ŀ���ܵ��˺�����ת��Ѱ��
            if (owner.Target == null)
            {
                Transform player = GameManager.Instance.GetPlayer().transform;
                if (Mathf.Sign(player.position.x - owner.transform.position.x) * owner.transform.localScale.x < 0)
                    owner.ChangeOriented();
            }
        }

        public void OnUpdate()
        {
            if (isOver)
            {
                if (owner.Target == null)
                {
                    _Fsm.TransitionStatus(actionType.walk);
                }
                else
                {
                    float deltaDist = owner.distFromTarget();
                    if (deltaDist < owner.near_range_enabled_dist)
                    {
                        _Fsm.TransitionStatus(owner.GetAttackIndex());
                    }
                    else
                    {
                        _Fsm.TransitionStatus(actionType.run);
                    }
                }
            }
        }

        public void OnExit()
        {
            eventDispatcher.OnAnimationEvent -= HandleAnimation;
        }

        public void HandleAnimation(AnimationStage stage)
        {
            if (stage == AnimationStage.AnimEnd)
                isOver = true;
        }
    }
}
