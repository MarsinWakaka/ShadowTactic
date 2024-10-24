using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    //Player owner;
    [SerializeField] int attackDamage;

    //public static int count;
    //private float time;

    //void Start()
    //{
    //    owner = gameObject.GetComponentInParent<Player>();
    //}

    //public void Update()
    //{
    //    if (time < 3)
    //    {
    //        time += Time.deltaTime;
    //    }
    //    else
    //    {
    //        count = 0;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<HealthController>().Hurt(attackDamage);
            //time = 0;
            //Debug.Log($"击中了{collision.ToString()} {++count}次");
        }
        else if (collision.CompareTag("PlayerInteract"))// 没有敌人标签
        {
            var destoryFX = collision.gameObject.GetComponent<DestoryFX>();
            if (destoryFX.enabled)
            {
                destoryFX.Destory();
            }
        }
    }
}