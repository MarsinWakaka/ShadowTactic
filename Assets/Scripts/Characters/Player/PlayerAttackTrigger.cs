using System;
using UnityEngine;
using Universal.HealthSystem;

namespace Characters.Player
{
    // 使此组件可被多选择时，统一修改多组件的属性
    public class PlayerAttackTrigger : MonoBehaviour
    {
        [SerializeField] int attackDamage;
         float width = 1f;
         float height = .8f;
        
        private void OnEnable()
        {
            // 检测在攻击范围内的碰撞体
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(width, height), 0);
#if UNITY_EDITOR
            int dir = transform.parent.localScale.x > 0 ? 1 : -1;
            var startPosition = new Vector2(transform.position.x - dir * width / 2, transform.position.y);
            var endPosition = new Vector2(transform.position.x + dir * width / 2, transform.position.y);
            // 绘制
            Debug.DrawLine(startPosition, endPosition, Color.red, 1f);
            // 射线末端垂直绘制
            Debug.DrawLine(
                endPosition + new Vector2(0, height / 2), 
                endPosition - new Vector2(0, height / 2)
                , Color.red, 1f);
#endif
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Debug.Log("PlayerAttackTrigger: " + collider.name);
                    collider.GetComponent<HealthController>().TryTakeDamage(attackDamage);
                }
                else if (collider.CompareTag("PlayerInteract")) // 没有敌人标签
                {
                    var destroyFX = collider.gameObject.GetComponent<DestoryFX>();
                    if (destroyFX.enabled) destroyFX.DestroyItem();
                }
            }
        }
    }
}