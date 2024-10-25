using System.Collections;
using Characters.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Universal.AudioSystem;

namespace Utilities
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                }
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
                return instance;
            }
        }

        public static bool IsInstanceNull() => instance == null;

        public int targetFrameRate = 60;

        /// <summary>
        /// 游戏结束时的黑色背景
        /// </summary>
        private Image _blackBg;

        #region 分数系统
        private int _lastScore;
        private int _score;
        private Transform _scoreSystem;
        private Image _scoreBg;
        private TextMeshProUGUI _scoreText;

        [Header(" ————————效果参数—————————")] 
        private Color _originalColor;
        [SerializeField] Color EffectColor;
        private readonly float _fontSizeMultipler = 1.2f;
        private float _fontScale = 72;
        private float _finalFontScale;
        private float _scoreTimer;
        private readonly float _scoreTransitTime = 2f;
        Coroutine UIcoroutinue;

        #endregion

        //全局变量
        float sinValueByTime;
    
        private GameObject _player;
        public GameObject GetPlayer()
        {
            if (_player == null) _player = GameObject.FindWithTag("Player");
            return _player;
        }
    
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            SetFrameRate(targetFrameRate);
        }

        private void Start()
        {
            SoundManager.Instance.PlayBGM();
        }

        private int _frameRate;
        private int _frameCount;
        private float _accTime;

        private void Update()
        {
            sinValueByTime = Mathf.Sin(Time.time * 2);
        
            _frameCount++;
            _accTime += Time.deltaTime;
            if (_accTime > 1)
            {
                _frameRate = _frameCount;
                _accTime = 0;
                _frameCount = 0;
            }
        }
    
        // 通过OnGUI显示帧率, 要求字体清晰，有背景
        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 15;
            style.normal.textColor = new Color(0.11f, 0.36f, 0.06f);
            GUI.Label(new Rect(5, 5, 100, 100), $"FPS: {_frameRate}", style);
        }


        public void AddScore(int score)
        {
            _lastScore = _score;
            _score += score;
            UpdateScoreUI();
        }

        private void InitScoreSystemUI()
        {
            var scoreGo = GameObject.Find("ScoreSystem");
            if (scoreGo != null)
            {
                _scoreSystem = scoreGo.transform;
                _scoreBg = _scoreSystem.GetComponentInChildren<Image>();
                _scoreText = _scoreSystem.GetComponentInChildren<TextMeshProUGUI>();
                _originalColor = _scoreBg.color;
                _fontScale = _scoreText.fontSize;
                _finalFontScale = _fontScale * _fontSizeMultipler;
            }
        }

        private void UpdateScoreUI()
        {
            if (_scoreSystem == null) InitScoreSystemUI();
            if (_scoreSystem == null) return;
            
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

            _scoreText.text = _score.ToString();
        }

        private IEnumerator AddScoreFX()
        {
            float rate;
            // float value;
            _scoreTimer = 0f;
            while (_scoreTimer < _scoreTransitTime)
            {
                rate = _scoreTimer / _scoreTransitTime;
                _scoreText.text = Mathf.RoundToInt(Mathf.Lerp(_lastScore, _score, rate)).ToString();
                _scoreText.fontSize = Mathf.Lerp(_finalFontScale, _fontScale, rate);
                // value = Mathf.Lerp(0, 1, rate);
                if (_scoreBg != null) _scoreBg.color = MyMath.Lerp(EffectColor, _originalColor, rate);
                _scoreTimer += Time.deltaTime;
                yield return null;
            }

            _scoreText.text = _score.ToString();
            UIcoroutinue = null;
            ResetUI();
        }

        private void ResetUI()
        {
            _scoreText.fontSize = _fontScale;
            if (_scoreBg != null) _scoreBg.color = _originalColor;
        }
        
        /// <summary>
        /// 每当场景被加载时，调用此方法
        /// </summary>
        public void StartNewSceneAction()
        {
            if (_blackBg == null) _blackBg = GameObject.Find("BlackBackGround").GetComponent<Image>();
            if (_blackBg != null) StartCoroutine(SceneLoadedFX());
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
                _blackBg.color = new Vector4(0, 0, 0, rate);
                yield return null;
            }
        }
        
        /// <summary>
        /// 要想加载下一关时，调用此方法
        /// </summary>
        public void ToLoadNextScene()
        {
            if (_blackBg == null) 
                _blackBg = GameObject.Find("BlackBackGround").GetComponent<Image>();
            
            SoundManager.Instance.PlayNextLevelSFX();
            StopAllCoroutines();
            StartCoroutine(LoadNextSceneCoroutine());
        }

        private IEnumerator LoadNextSceneCoroutine()
        {
            float timer = 0;
            float rate = 0;
            _blackBg.enabled = true;
            //gameObject.GetComponent<Volume>().vi
            while (timer < 3)
            {
                timer += Time.deltaTime;
                rate = timer / 3;
                _blackBg.color = new Vector4(0, 0, 0, rate);
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
            targetFrameRate = rate;
            Application.targetFrameRate = targetFrameRate;
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
            _player.GetComponent<BasePlayer>().m_FSM.CeaseFSM();
        }

        public void CeaseInput()
        {
            _player.GetComponent<BasePlayer>().m_FSM.CeaseFSM();
        }

        public void StartInput()
        {
            _player.GetComponent<BasePlayer>().m_FSM.StartFSM();
        }

        public void ContinueGame()
        {
            Time.timeScale = 1f;
            _player.GetComponent<BasePlayer>().m_FSM.StartFSM();
        }

        public float SinValueByTime()
        {
            return sinValueByTime;
        }

        public float SinValueWithNormalize()
        {
            return (sinValueByTime + 1) / 2;
        }

    
        public GameObject[] players;
        private int _characterSelectedIndex;
        public void SetCharacterSelected(int slotIndex)
        {
            _characterSelectedIndex = slotIndex;
        }
    
        private string ninjaPath = "Prefabs/Characters/MonkNinja";
        private string samuraiPath = "Prefabs/Characters/SamuraiMarsin";
        
        public void CreatePlayer(Vector2 position)
        {
            if (_player != null) Destroy(_player);
            var playerLoaded = Resources.Load<GameObject>(_characterSelectedIndex == 0 ? ninjaPath : samuraiPath);
            _player = Instantiate(playerLoaded);
            _player.transform.position = position;
        }
    }
}