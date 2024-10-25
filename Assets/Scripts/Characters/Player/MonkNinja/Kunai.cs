using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.HealthSystem;

public class Kunai : FlyingObj
{
    public override void CauseDamage(HealthController hc)
    {
        //同向，认为属于背刺，两倍伤害
        if(rg.velocity.x  * hc.gameObject.transform.localScale.x > 1)
        {
            int mul = Mathf.RoundToInt(Random.Range(2f, 4f));
            hc.TryTakeDamage(damage * mul);
        }
        else
        {
            hc.TryTakeDamage(damage);
        }
    }
}
