using UnityEngine;

namespace Characters.Player.MonkNinja
{
    public class MonkNinjaDie : IState
    {
        private readonly BasePlayer _basePlayer;
        private readonly Fsm _mFsm;

        public MonkNinjaDie(BasePlayer basePlayer)
        {
            this._basePlayer = basePlayer;
            _mFsm = basePlayer.m_FSM;
        }

        public void OnEnter()
        {
            _mFsm.CeaseFSM();
            _basePlayer.anim.Play(AnimTags.DIE);
            GameObject go = GameObject.Instantiate(_basePlayer.HurtFX, _basePlayer.transform.position, _basePlayer.transform.rotation);
            go.transform.rotation = new Quaternion(0, 0, UnityEngine.Random.Range(0, 360), 0);

            _basePlayer.SetCharacterDie();
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}
