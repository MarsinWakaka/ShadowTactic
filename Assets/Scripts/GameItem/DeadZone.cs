using Assets.Scripts.GameItem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Universal.HealthSystem;
using static Unity.VisualScripting.Member;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().TryTakeDamage(9999);
        }
    }
}