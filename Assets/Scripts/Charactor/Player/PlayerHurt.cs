using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : IState
{
    Player player;
    Fsm m_FSM;

    [SerializeField]
    //private bool isOnGround;

    public PlayerHurt(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        player.anim.Play(AnimTags.HURT);
        player.rg.velocity = new Vector2 (player.rg.velocity.x / 2, player.rg.velocity.y);
        GameObject go = GameObject.Instantiate(player.HurtFX, player.transform.position, player.transform.rotation);

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
