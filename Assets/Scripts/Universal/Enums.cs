public enum actionType
{
    idle, walk, run, attack1, attack2, attack3, shoot, skill, defence, jump, fall, hurt,stiff, dead, touchGround, antiAttack,
    defenceSuccess
}

//precast������ǰ��϶���
//typeAhead����typeAhead��nextStageReady����Ԥ���룬��ǿ�ָ�
//nextStageReady����Խ���״̬���л�����ҡ���Ա��κΰ������
//animEnd������ζ�ŵĽ��������ж�״̬���л�
public enum AnimationStage
{
    precastEnd, typeAhead, NextStageReady, AnimEnd, SetCheckPointLeftBorder, SetCheckPointRightBorder, nextStageEnd,
}

public enum ActionAudio
{
    idle, walk, run, attack1, attack2, attack3, sheath, bow, shoot, skill, defence, jump, fall, touchground, hurt, dead,
}

public interface IState 
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public enum FootStepAudioType
{
    Leaves, HardSurface, Water
}