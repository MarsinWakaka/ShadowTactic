using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public int TargetFrameRate = 60;

    private int frameCount;
    private float deltaTime;
    private float updateRate = 0.5f; // 更新帧率的频率

    [SerializeField] Image BlackBG;

    #region 分数系统

    private int LastScore;
    private int Score;
    [SerializeField] Transform ScoreSystem;

    private Image ScoreBG;
    private TextMeshProUGUI ScoreText;

    [Header(" ————————效果参数—————————")] [SerializeField]
    float FontSizeMultipler;

    Color OriginalColor;
    [SerializeField] Color EffectColor;
    private float fontScale;
    private float finalFontScale;

    private float scoreTimer;
    [SerializeField] float scoreTransitTime;
    Coroutine UIcoroutinue;

    #endregion

    //全局变量
    float sinValueByTime;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        SetFrameRate(TargetFrameRate);
        SoundManager.Instance.PlayBGM();
        //StartCoroutine(UpdateFramerate());

        if (ScoreSystem != null)
        {
            ScoreBG = ScoreSystem.GetComponentInChildren<Image>();
            ScoreText = ScoreSystem.GetComponentInChildren<TextMeshProUGUI>();
            OriginalColor = ScoreBG.color;
            fontScale = ScoreText.fontSize;
            finalFontScale = fontScale * FontSizeMultipler;

            if (BlackBG != null)
            {
                if (!BlackBG.enabled)
                    BlackBG.enabled = true;
                StartSceneFX();
            }
        }
    }

    private void Update()
    {
        sinValueByTime = Mathf.Sin(Time.time * 2);
    }

    void StartSceneFX()
    {
        if (BlackBG != null)
            StartCoroutine(SceneLoadedFX());
    }

    private IEnumerator SceneLoadedFX()
    {
        float timer = 0;
        float maxTime = 2;
        float rate;
        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            rate = (maxTime - timer) / maxTime;
            BlackBG.color = new Vector4(0, 0, 0, rate);
            yield return null;
        }
    }

    public void AddScore(int score)
    {
        LastScore = Score;
        Score += score;
        UpdateUI();
    }

    public void ClearScore()
    {
        LastScore = Score = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (UIcoroutinue == null)
        {
            UIcoroutinue = StartCoroutine(AddScoreFX());
        }
        else
        {
            StopCoroutine(UIcoroutinue);
            ResetUI();
            UIcoroutinue = StartCoroutine(AddScoreFX());
        }

        ScoreText.text = Score.ToString();
    }

    IEnumerator AddScoreFX()
    {
        float rate;
        float value;
        scoreTimer = 0f;
        while (scoreTimer < scoreTransitTime)
        {
            rate = scoreTimer / scoreTransitTime;
            ScoreText.text = Mathf.RoundToInt(Mathf.Lerp(LastScore, Score, rate)).ToString();
            ScoreText.fontSize = Mathf.Lerp(finalFontScale, fontScale, rate);
            value = Mathf.Lerp(0, 1, rate);
            //ScoreBG.color = new Color(1, value, value, 1);
            ScoreBG.color = MyMath.Lerp(EffectColor, OriginalColor, rate);

            scoreTimer += Time.deltaTime;
            yield return null;

            //Debug.Log($"{DateTime.Now}更新中,Score:{ScoreText.text}");
        }

        ScoreText.text = Score.ToString();
        UIcoroutinue = null;
        ResetUI();
    }

    private void ResetUI()
    {
        ScoreText.fontSize = fontScale;
        if (ScoreBG != null)
            ScoreBG.color = OriginalColor;
    }

    public void NextScene()
    {
        SoundManager.Instance.PlayNextLevelSFX();
        StopAllCoroutines();
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        float timer = 0;
        float rate = 0;
        //gameObject.GetComponent<Volume>().vi
        while (timer < 3)
        {
            timer += Time.deltaTime;
            rate = timer / 3;
            BlackBG.color = new Vector4(0, 0, 0, rate);
            yield return null;
        }


        int index = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        switch (index)
        {
            case 0:
                GameRoot.Instance.SceneSystem.SetScene(new StartScene());
                break;
            case 1:
                GameRoot.Instance.SceneSystem.SetScene(new Level1Scene());
                break;
            case 2:
                GameRoot.Instance.SceneSystem.SetScene(new Level2Scene());
                break;
            case 3:
                GameRoot.Instance.SceneSystem.SetScene(new Level3Scene());
                break;
            case 4:
                GameRoot.Instance.SceneSystem.SetScene(new Level4Scene());
                break;
        }
    }

    public void GameRestart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        switch (index)
        {
            case 0:
                GameRoot.Instance.SceneSystem.SetScene(new StartScene());
                break;
            case 1:
                GameRoot.Instance.SceneSystem.SetScene(new Level1Scene());
                break;
            case 2:
                GameRoot.Instance.SceneSystem.SetScene(new Level2Scene());
                break;
            case 3:
                GameRoot.Instance.SceneSystem.SetScene(new Level3Scene());
                break;
            case 4:
                GameRoot.Instance.SceneSystem.SetScene(new Level4Scene());
                break;
        }
    }

    public void SetFrameRate(int rate)
    {
        TargetFrameRate = rate;
        Application.targetFrameRate = TargetFrameRate;
    }

    private IEnumerator UpdateFramerate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            frameCount++;
            deltaTime += Time.unscaledDeltaTime;

            if (deltaTime > updateRate)
            {
                float framerate = frameCount / deltaTime;
                Debug.Log("Current framerate: " + framerate.ToString("F2"));

                frameCount = 0;
                deltaTime -= updateRate;
            }
        }
    }

    // 游戏结束
    internal static void GameOver()
    {
        // 通知各个敌人恢复idle状态
        GameRoot.Instance.Push(new DeadPanel());
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().m_FSM.CeaseFSM();
    }

    public void CeaseInput()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().m_FSM.CeaseFSM();
    }

    public void StartInput()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().m_FSM.StartFSM();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().m_FSM.StartFSM();
    }

    public float SinValueByTime()
    {
        return sinValueByTime;
    }

    public float SinValueWithNormalize()
    {
        return (sinValueByTime + 1) / 2;
    }
}