using System;
using System.Collections;
using Characters.Enemy;
using Characters.Player;
using Characters.Player.MonkNinja;
using UnityEngine;
using Utilities;

public class EnemyViewer : MonoBehaviour
{
    private Enemy owner;
    BasePlayer _basePlayer;
    GameObject Player;

    public void Register(Enemy owner)
    {
        if (owner == null)
            Debug.LogWarning("owner can't be null");

        this.owner = owner;
    }

    private void Start()
    {
        Player = GameManager.Instance.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (_basePlayer == null)
                _basePlayer = collision.gameObject.GetComponent<BasePlayer>();
            //���� ֪ͨ �¼�
            _basePlayer.UnderView.Subscriber += FoundPlayer;

            if(_basePlayer.GetHideState())
                return;

            FoundPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //ȡ������ ֪ͨ �¼�
            _basePlayer.UnderView.Subscriber -= FoundPlayer;

            if (owner.Target != null)
                MissPlayer();
        }
    }

    private void FoundPlayer()
    {
        owner.Target = Player;
        _basePlayer.RegisterFounded();
    }

    private void MissPlayer()
    {
        _basePlayer.InformMiss();
        owner.Target = null;
    }
}
