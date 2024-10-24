using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    /// <summary>
    /// 金刚不坏
    /// </summary>
    [HideInInspector] public bool isMortal = false;
    public GameObject HurtFX;

    #region 内置组件
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rg;
    [HideInInspector] public Fsm m_FSM;
    [HideInInspector] public HealthController healthController;
    [HideInInspector] public AnimationEventDispatcher eventDispatcher;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public SpriteRenderer sr;
    #endregion
}
