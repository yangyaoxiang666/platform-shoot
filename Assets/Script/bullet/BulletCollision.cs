using UnityEngine;

public class BulletCollision : MonoBehaviour 
{
    public GameObject shooter; // 记录发射者
    public int damage = 25; // 子弹伤害值
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[调试] 子弹碰到了: {other.name}, Tag: {other.tag}");
        
        // 如果击中的是发射者自己，忽略
        if (other.gameObject == shooter) 
        {
            Debug.Log("子弹击中发射者，忽略");
            return;
        }
            
        // 如果击中的是其他玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log($"子弹击中了 {other.name}");
            
            // 获取玩家的健康系统组件
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 造成伤害
                healthSystem.TakeDamage(damage, shooter);
                Debug.Log($"对 {other.name} 造成 {damage} 点伤害");
            }
            else
            {
                Debug.LogWarning($"玩家 {other.name} 没有HealthSystem组件！");
            }
            
            // 销毁子弹
            Destroy(gameObject);
        }
        
        // 如果击中墙壁或其他障碍物
        if (other.CompareTag("Wall"))
        {
            Debug.Log("子弹击中墙壁");
            Destroy(gameObject);
        }
    }
    
    // 可选：如果使用普通碰撞体而不是Trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;
        Debug.Log($"[调试] 子弹碰撞了: {other.name}, Tag: {other.tag}");
        
        // 如果击中的是发射者自己，忽略
        if (other.gameObject == shooter) 
        {
            Debug.Log("子弹击中发射者，忽略");
            return;
        }
            
        // 如果击中的是其他玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log($"子弹击中了 {other.name}");
            
            // 获取玩家的健康系统组件
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 造成伤害
                healthSystem.TakeDamage(damage, shooter);
                Debug.Log($"对 {other.name} 造成 {damage} 点伤害");
            }
            else
            {
                Debug.LogWarning($"玩家 {other.name} 没有HealthSystem组件！");
            }
            
            // 销毁子弹
            Destroy(gameObject);
        }
        
        // 如果击中墙壁或其他障碍物
        if (other.CompareTag("Wall"))
        {
            Debug.Log("子弹击中墙壁");
            Destroy(gameObject);
        }
    }
} 