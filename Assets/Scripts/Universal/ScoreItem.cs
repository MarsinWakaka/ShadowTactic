using System.Collections;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] int Score;

    void Update()
    {

    }

    private void OnDestroy()
    {
        if (Score > 0)
        {
            if(GameManager.Instance != null)
                GameManager.Instance.AddScore(Score);
        }
    }
}