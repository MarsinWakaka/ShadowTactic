using System;
using System.Collections;
using UnityEngine;

public class EnemyViewer : MonoBehaviour
{
    private Enemy owner;
    Player player;
    GameObject Player;

    public void Register(Enemy owner)
    {
        if (owner == null)
            Debug.LogWarning("owner can't be null");

        this.owner = owner;
    }

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (player == null)
                player = collision.gameObject.GetComponent<Player>();
            //订阅 通知 事件
            player.underView.Subscriber += FoundPlayer;

            if(player.GetHideState())
                return;

            FoundPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //取消订阅 通知 事件
            player.underView.Subscriber -= FoundPlayer;

            if (owner.Target != null)
                MissPlayer();
        }
    }

    private void FoundPlayer()
    {
        owner.Target = Player;
        player.RegisterFounded();
    }

    private void MissPlayer()
    {
        player.InformMiss();
        owner.Target = null;
    }
}
