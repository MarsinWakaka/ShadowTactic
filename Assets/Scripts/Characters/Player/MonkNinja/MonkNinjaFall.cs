using Characters.Player;
using UnityEngine;

public class MonkNinjaFall : IState
{
    BasePlayer _basePlayer;
    Fsm m_FSM;
    private float jumpSpeed;
    private float lastVelocity;
    public MonkNinjaFall(BasePlayer basePlayer)
    {
        this._basePlayer = basePlayer;
        m_FSM = basePlayer.m_FSM;
    }

    public void OnEnter()
    {
        _basePlayer.anim.Play(AnimTags.FALL);
        jumpSpeed = Mathf.Max(Mathf.Abs(_basePlayer.rg.velocity.x), _basePlayer.JumpSpeed);
        //isOnGround = false;
    }

    public void OnUpdate()
    {
        if (m_FSM.IsCeaseInput())
        {
            m_FSM.TransitionStatus(actionType.idle);
            return;
        }

        //��Ծ�����ڼ����
        if (_basePlayer.rg.velocity.x < _basePlayer.RunSpeed)
        {
            float input = Input.GetAxisRaw(Axis.HORIZONTAL);
            _basePlayer.rg.velocity = new Vector2(input * jumpSpeed, _basePlayer.rg.velocity.y);
        }
        _basePlayer.CheckOriented();

        if(_basePlayer.CheckOnGround())
        {
            if (lastVelocity < -10f)
                m_FSM.TransitionStatus(actionType.touchGround);
            else if(Input.GetAxisRaw(Axis.HORIZONTAL) != 0)
            {
                if (Input.GetButton(Trigger.RUN))
                    _basePlayer.m_FSM.TransitionStatus(actionType.run);//����
                else
                    _basePlayer.m_FSM.TransitionStatus(actionType.walk);//����
            }
            else
            {
                m_FSM.TransitionStatus(actionType.idle);
            }
        }
        //�˳�������TouchGround�������һ֡�Ĵ����¼�������Player TouchGround����

        lastVelocity = _basePlayer.rg.velocity.y;
    }

    public void OnExit()
    {
        
    }
}
