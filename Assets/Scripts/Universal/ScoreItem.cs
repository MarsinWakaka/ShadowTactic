using System.Collections;
using UnityEngine;
using Utilities;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] int Score;

    private void OnDestroy()
    {
        if (Score > 0)
        {
            if (GameManager.IsInstanceNull()) return;
            if (GameManager.Instance != null) GameManager.Instance.AddScore(Score);
        }
    }
}