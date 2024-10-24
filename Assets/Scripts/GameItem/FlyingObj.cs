using UnityEditor;
using UnityEngine;

public class FlyingObj : MonoBehaviour
{
    //定义通用的拥有远程攻击的伤害类
    //GameObject owner;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float flySpeed;

    [SerializeField]
    protected float lifetime;

    [SerializeField] string targetTag;

    protected Rigidbody2D rg;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        rg.velocity = new Vector2(flySpeed * transform.localScale.x, 0.2f);

        Destroy(gameObject, lifetime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag.ToString()))
        {
            var obj = collision.gameObject.GetComponent<HealthController>();
            CauseDamage(obj);
            //击中特效

            //摧毁自己

            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Terrian"))
        {

            //击中特效

            //摧毁自己

            Destroy(this.gameObject);
        }
    }

    public virtual void CauseDamage(HealthController hc)
    {
        hc.Hurt(damage);
    }
}
