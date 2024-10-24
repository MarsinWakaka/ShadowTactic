using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : IState
{
    Player player;
    Fsm m_FSM;

    [SerializeField]
    //private bool isOnGround;

    public PlayerDie(Player player)
    {
        this.player = player;
        m_FSM = player.m_FSM;
    }

    public void OnEnter()
    {
        m_FSM.CeaseFSM();
        player.anim.Play(AnimTags.DEAD);
        GameObject go = GameObject.Instantiate(player.HurtFX, player.transform.position, player.transform.rotation);
        go.transform.rotation = new Quaternion(0, 0, UnityEngine.Random.Range(0, 360), 0);

        player.viewLight.gameObject.SetActive(false);
        //player.GetComponentInChildren<>().enabled = false;
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
