namespace Characters.Enemy
{
    public class EnemyStiff : IState
    {
        private readonly Enemy _enemy;
        
        public EnemyStiff(Enemy enemy)
        {
            _enemy = enemy;
        }

        private bool _isAnimEnd;
        
        public void OnEnter()
        {
            _isAnimEnd = false;
            _enemy.anim.Play(AnimTags.STIFF);
            _enemy.EventDispatcher.OnAnimationEvent += AnimationStageHandler;
        }

        public void OnUpdate()
        {
            if (_isAnimEnd)
            {
                _enemy.m_FSM.TransitionStatus(actionType.idle);
            }
        }

        public void OnExit()
        {
            _enemy.EventDispatcher.OnAnimationEvent -= AnimationStageHandler;
        }
        
        private void AnimationStageHandler(AnimationStage stage)
        {
            if (stage == AnimationStage.AnimEnd) _isAnimEnd = true;
        }
    }
}