using UnityEngine;

public class BulletCollision : MonoBehaviour 
{
    public GameObject shooter; // 记录发射者
    
    void OnTriggerEnter2D(Collider2D other)
    {
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
            
            // 这里预留伤害处理接口，你后面可以添加
            // TODO: 添加伤害处理逻辑
            
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
            
            // 预留伤害处理接口
            // TODO: 添加伤害处理逻辑
            
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