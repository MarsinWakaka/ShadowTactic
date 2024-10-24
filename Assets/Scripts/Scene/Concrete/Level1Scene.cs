using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Scene : SceneState
{
    /// <summary>
    /// 场景名字
    /// </summary>
    readonly string sceneName = "Level1";

    PanelManager panelManager;

    public override void OnEnter()
    {
        panelManager = new PanelManager();
        //SceneManager.LoadScene(sceneName);
        //if (SceneManager.GetActiveScene().name != sceneName)
        //{
        //    SceneManager.LoadScene(sceneName);
        //    SceneManager.sceneLoaded += SceneLoaded;
        //}
        //else
        //{
        //    panelManager.Push(new MainPanel());
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
        panelManager.Push(new MainPanel());
        GameRoot.Instance.SetAction(panelManager.Push);

        //场景加载完毕后进入对话系统
        //panelManager.Push(new DialogPanel());
    }
}