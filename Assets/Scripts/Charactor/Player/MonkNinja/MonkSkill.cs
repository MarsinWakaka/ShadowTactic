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
            if(go.localScale.x * owner.transform.localScale.x > 0)//����һ��������Ϊ�Ǳ���͵Ϯ
            {
                collision.GetComponent<HealthController>().Hurt(attackDamage * 10);
            }
            else
            {
                collision.GetComponent<HealthController>().Hurt(attackDamage);
            }
        }
        else if (collision.CompareTag("PlayerInteract"))// û�е��˱�ǩ
        {
            Destroy(collision.gameObject);
        }
    }
}