using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class NextSceneEnter : MonoBehaviour
{
    //[SerializeField] string NextSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.ToLoadNextScene();
    }
}
