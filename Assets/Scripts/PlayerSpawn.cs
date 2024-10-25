using UnityEngine;
using Utilities;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.CreatePlayer(transform.position);
    }
}