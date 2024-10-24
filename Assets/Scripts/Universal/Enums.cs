public enum actionType
{
    idle, walk, run, attack1, attack2, attack3, shoot, skill, defence, jump, fall, hurt, dead, touchGround,
}

//precast������ǰ��϶���
//typeAhead����typeAhead��nextStageReady����Ԥ���룬��ǿ�ָ�
//nextStageReady����Խ���״̬���л�����ҡ���Ա��κΰ������
//animEnd������ζ�ŵĽ��������ж�״̬���л�
public enum AnimationStage
{
    precastEnd, typeAhead, nextStageReady, animEnd, checkHit, stopCheckHit, nextStageEnd,
}

public enum actionAudio
{
    idle, walk, run, attack1, attack2, attack3, sheath, bow, shoot, skill, defence, jump, fall, touchground, hurt, dead,
}

public interface IState 
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public enum StepAudio
{
    Shuye, HardGround, Water
}