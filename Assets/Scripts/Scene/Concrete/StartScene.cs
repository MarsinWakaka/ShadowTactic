using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : SceneState
{
    /// <summary>
    /// 场景名字
    /// </summary>
    readonly string sceneName = "Start";

    PanelManager panelManager;

    public override void OnEnter()
    {
        panelManager = new PanelManager();
        //if (SceneManager.GetActiveScene().name != sceneName)
        //{
        //    SceneManager.LoadScene(sceneName);
        //    SceneManager.sceneLoaded += SceneLoaded;
        //}
        //else
        //{
        //    panelManager.Push(new StartPanel());
        //    GameRoot.Instance.SetAction(panelManager.Push);
        //}
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        panelManager.PopAll();
    }

    /// <summary>
    /// 场景加载完毕之后执行的方法
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    public void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        panelManager.Push(new StartPanel());
        GameRoot.Instance.SetAction(panelManager.Push);
    }
}