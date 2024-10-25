public enum actionType
{
    idle, walk, run, attack1, attack2, attack3, shoot, skill, defence, jump, fall, hurt,stiff, dead, touchGround, antiAttack,
    defenceSuccess
}

//precast允许提前打断动作
//typeAhead允许typeAhead到nextStageReady进行预输入，增强手感
//nextStageReady后可以进行状态的切换，后摇可以被任何按键打断
//animEnd负责意味着的结束，自行对状态的切换
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