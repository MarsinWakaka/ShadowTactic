using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSkill : MonoBehaviour
{
    Player owner;
    [SerializeField] int attackDamage;

    void Start()
    {
        owner = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var go = collision.transform;
            if(go.localScale.x * owner.transform.localScale.x > 0)//朝向一致我们认为是背后偷袭
            {
                collision.GetComponent<HealthController>().Hurt(attackDamage * 10);
            }
            else
            {
                collision.GetComponent<HealthController>().Hurt(attackDamage);
            }
        }
        else if (collision.CompareTag("PlayerInteract"))// 没有敌人标签
        {
            Destroy(collision.gameObject);
        }
    }
}