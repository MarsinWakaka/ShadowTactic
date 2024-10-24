using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Player : Charactor {
    #region ����
    [Header("________ IDLE ________")]
    [SerializeField] Transform footPos;
    [SerializeField] Vector2 footOffset;

    [Header("________ WALK ________")]
    [SerializeField] float walkSpeed;
    [SerializeField] StepAudio currentAudio;
    [SerializeField] AudioData[] OnShuYeAudio;
    [SerializeField] AudioData[] OnHardGroundAudio;
    [SerializeField] AudioData[] InWaterAudio;

    [Header("________ Run ________")]
    [SerializeField] float runSpeed;

    [Header("________ JUMP ________")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] AudioData[] jumpAudio;
    [SerializeField] AudioData[] touchGroundAudio;

    [Header("________ ATTACK ________")]
    [SerializeField] float moveSpeedWhenAttack;
    //[SerializeField] float attackDamage;
    public Transform[] AttackRange;
    //[SerializeField] float attackRange;
    //[SerializeField] Vector2 attackOffset;
    [SerializeField] AudioData[] attackAudio;
    [SerializeField] AudioData[] sheathAudio;

    [Header("________ SKILL _________")]
    [SerializeField] AudioData[] skillAudio;

    [Header("________ HURT _________")]
    [SerializeField] AudioData[] hurtAudio;

    [Header("________ DEAD _________")]
    [SerializeField] AudioData[] deadAudio;

    [Header("________ ���� _________")]
    [SerializeField] LayerMask terrianLayer;
    [SerializeField] LayerMask EnemyLayer;
    public Transform viewLight;
    public MyEventDispatcher underView;

    [Header("________ Cast ________")]
    public GameObject kunai;
    public int kunaiNum = 10;
    [SerializeField] TextMeshProUGUI kunaiNumUI;
    [SerializeField] AudioData[] castAudio;

    [Header("________ Hide _________")]
    public bool isHide;
    public int foundedNum;
    #endregion

    public float MoveSpeedWhenAttack { get => moveSpeedWhenAttack; }
    public float WalkSpeed { get => walkSpeed;}
    public float RunSpeed { get => runSpeed;}
    public float JumpForce { get => jumpForce; }
    public float JumpSpeed { get => jumpSpeed; }
    //public float AttackDamage { get => attackDamage;}

    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // ע��״̬������״̬����ҪeventDispatcher.
        eventDispatcher = new AnimationEventDispatcher();
        RegisterFSM();
        // HealthCOntroller��Ҫ״̬��
        healthController = GetComponent<HealthController>();
        healthController.Init(this);

        underView = new MyEventDispatcher();
        sr = GetComponent<SpriteRenderer>();
        isHide = false;

        UpdateUI();
    }

    void Update()
    {
        //Debug.Log("isOnGround:" + CheckOnGround());
        m_FSM.Execute();
        Recover();
    }
    
    // ������Ѫ
    private void Recover()
    {
        healthController.Recover(1 * Time.deltaTime);
    }

    public void CheckOriented()
    {
        if (rg.velocity.x > 0.001f && transform.localScale.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rg.velocity.x < -0.001f && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public bool CheckOnGround()
    {
        bool isOnGround = false;
        float rayDistance = 0.2f;
        Vector2 origin = new Vector2(footPos.position.x, footPos.position.y);
        Vector2 rayDir = Vector2.down;
        if (Physics2D.Raycast(origin + footOffset, rayDir, rayDistance, terrianLayer))
        {
            Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.red);//��㣬������ɫ����ѡ��//�ߵĳ�����������ͬ
            isOnGround = true;
        }
        else
        {
            Debug.DrawRay(origin + footOffset, rayDir * rayDistance, Color.green);
        }

        if (Physics2D.Raycast(origin - footOffset, rayDir, rayDistance, terrianLayer))
        {
            Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.red);//��㣬������ɫ����ѡ��//�ߵĳ�����������ͬ
            isOnGround = true;
        }
        else
        {
            Debug.DrawRay(origin - footOffset, rayDir * rayDistance, Color.green);
        }
        return isOnGround;
    }

    public void CheckHit(int index)
    {
        AttackRange[index].gameObject.SetActive(true);
    }

    public bool isCheckHit(int index)
    {
        return AttackRange[index].gameObject.activeSelf;
    }

    public void StopCheckHit(int index)
    {
        AttackRange[index].gameObject.SetActive(false);
    }

    public bool CheckFall()
    {
        if (!CheckOnGround() && rg.velocity.y < 0)
        {
            return true;
        }
        return false;
    }

    // ������������������������������������������������ �����¼� ������������������������������������������������
    public void AnimationEventCallback(AnimationStage stage)
    {
        eventDispatcher.TriggerAnimationEvent(stage);
    }

    // ������������������������������������������������ ��Ƶ�¼� ������������������������������������������������
    public void AudioCallBack(actionAudio type)
    {
        SoundManager.Instance.PlaySFX(audioSource, type switch
        {
            actionAudio.walk => FootStepAudio(),
            actionAudio.attack1 => attackAudio,
            actionAudio.sheath => sheathAudio,
            actionAudio.skill => skillAudio,
            actionAudio.shoot => castAudio,
            actionAudio.jump => jumpAudio,
            actionAudio.touchground => touchGroundAudio,
            actionAudio.hurt => hurtAudio,
            actionAudio.dead => deadAudio,
            // ���������Ƶ���͵Ĵ���
            _ => null
        });
    }

    public void SetStepAudio(StepAudio stepAudio)
    {
        currentAudio = stepAudio;
    }

    AudioData[] FootStepAudio()
    {
        return currentAudio switch
        {
            StepAudio.Shuye => OnShuYeAudio,
            StepAudio.HardGround => OnHardGroundAudio,
            StepAudio.Water => InWaterAudio,
            _ => null,
        };
    }

    public void SetState(actionType actionType)
    {
        m_FSM.TransitionStatus(actionType);
    }

    private void RegisterFSM()
    {
        m_FSM = new Fsm();
        m_FSM.Init(this, actionType.idle, new PlayerIdle(this));
        m_FSM.AddAction(actionType.walk, new PlayerWalk(this));
        m_FSM.AddAction(actionType.run, new PlayerRun(this));
        m_FSM.AddAction(actionType.jump, new PlayerJump(this));
        m_FSM.AddAction(actionType.fall, new PlayerFall(this));
        m_FSM.AddAction(actionType.touchGround, new PlayerTouchGround(this));
        m_FSM.AddAction(actionType.attack1, new PlayerAttack1(this));
        m_FSM.AddAction(actionType.attack2, new PlayerAttack2(this));
        m_FSM.AddAction(actionType.shoot, new PlayerShoot(this)); //Զ�̹���;
        m_FSM.AddAction(actionType.skill, new PlayerSkill(this));
        m_FSM.AddAction(actionType.hurt, new  PlayerHurt(this));
        m_FSM.AddAction(actionType.dead, new PlayerDie(this));
    }

    /// <summary>
    /// ��������ȡ����ײ
    /// </summary>
    public void DestroySelf()
    {
        //rg.constraints = RigidbodyConstraints2D.FreezePosition;
        rg.constraints = RigidbodyConstraints2D.FreezeAll;
        CapsuleCollider2D caps = GetComponent<CapsuleCollider2D>();
        caps.enabled = false;
        GameManager.GameOver();
    }

    public void UpdateUI()
    {
        kunaiNumUI.text = kunaiNum.ToString();
    }

    /// <summary>
    /// ���Խ�������״̬
    /// </summary>
    /// <returns></returns>
    public bool TryHide()
    {
        //û����ע�⣬����Խ�����
        //if(foundedNum != 0)
        //    return false;
        if(foundedNum != 0)
        {
            //Debug.Log($"{DateTime.Now} ��ǰ���й۲���{foundedNum}, ���ܽ�������");
            return false;
        }
        else
        {
            //Debug.Log($"{DateTime.Now} ��ǰ���й۲���{foundedNum}����������");
        }

        isHide = true;
        sr.color = Color.black;
        ChangeHideEffect();

        return isHide;
    }

    /// <summary>
    /// ����ȡ������״̬,ˢ��Player����ײ��
    /// </summary>
    /// <returns></returns>
    public void CancelHideStateWithRefresh()
    {
        if(!isHide) return;

        isHide = false;
        sr.color = Color.white;

        InformAllSubscribe();
        ChangeHideEffect();
    }

    // /// <summary>
    // /// ����ȡ������״̬,����ˢ��Player����ײ��
    // /// </summary>
    // /// <returns></returns>
    //public void CancelHideState()
    //{
    //    if (!isHide) return;

    //    isHide = false;
    //    sr.color = Color.white;
    //    ChangeHideEffect();
    //}

    /// <summary>
    /// ��������״̬
    /// </summary>
    /// <returns></returns>
    public bool GetHideState()
    {
        return isHide;
    }

    /// <summary>
    /// ע��۲���
    /// </summary>
    public void RegisterFounded()
    {
        foundedNum++;
        //Debug.Log($"Found {DateTime.Now} : {foundedNum}");
    }

    public void InformMiss()
    {
        foundedNum--;
        //Debug.Log($"Miss {DateTime.Now} : {foundedNum}");
    }

    public void InformAllSubscribe()
    {
        underView.TriggerInform();
    }

    private void ChangeHideEffect()
    {
        VolumeController.Instance.ChangeHidenEffect();
    }

}
