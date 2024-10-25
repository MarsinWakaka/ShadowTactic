using Characters.Player;
using UnityEngine;

public class MonkNinjaHurt : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;

    public MonkNinjaHurt(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
    }

    public void OnEnter()
    {
        _basePlayer.anim.Play(AnimTags.HURT);
        _basePlayer.rg.velocity = new Vector2 (_basePlayer.rg.velocity.x / 2, _basePlayer.rg.velocity.y);
        GameObject go = GameObject.Instantiate(_basePlayer.HurtFX, _basePlayer.transform.position, _basePlayer.transform.rotation);

        // Ëæ»úÐý×ª½Ç¶È
        float randomRotation = UnityEngine.Random.Range(0, 360);
        go.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
    }
}
