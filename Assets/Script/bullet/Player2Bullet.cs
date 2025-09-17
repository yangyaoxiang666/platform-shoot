using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player2专用射击脚本 - 使用鼠标左键攻击，方向键转向
public class Player2Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;//子弹预制体
    public Transform firePoint;//子弹发射点
    public float bullspeed = 10f;//子弹速度
    public float lifeTime = 5f;//子弹生命周期
    public float fireRate = 0.5f;//子弹发射频率
    private float nextFireTime; //下次发射时间
    private bool facingRight = true; //角色面向方向，默认向右
    
    void Start()
    {
        // 在开始时检查必要的组件
        CheckComponents();
    }

    void CheckComponents()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError($"[{gameObject.name}] 子弹预制体未设置！请在Inspector中拖拽子弹预制体到bulletPrefab字段。");
        }
        
        if (firePoint == null)
        {
            Debug.LogError($"[{gameObject.name}] 发射点未设置！请在Inspector中拖拽发射点Transform到firePoint字段。");
        }
        
        Debug.Log($"[{gameObject.name}] Player2射击器初始化完成。");
    }

    void Update()
    {
        // Player2攻击键 - 鼠标左键
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;//更新下一次发射时间
        }
        
        // Player2转向 - 左右方向键
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            facingRight = false;
            Debug.Log($"[{gameObject.name}] Player2转向左侧");
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            facingRight = true;
            Debug.Log($"[{gameObject.name}] Player2转向右侧");
        }
    }
    
    void Shoot()
    {
        // 详细的空引用检查
        if (bulletPrefab == null) 
        {
            Debug.LogError($"[{gameObject.name}] 无法射击：子弹预制体为空！请在Inspector中设置bulletPrefab。");
            return;
        }
        
        if (firePoint == null)
        {
            Debug.LogError($"[{gameObject.name}] 无法射击：发射点为空！请在Inspector中设置firePoint。");
            return;
        }

        try
        {
            // 使用发射点位置和旋转
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            if (bullet == null)
            {
                Debug.LogError($"[{gameObject.name}] 子弹实例化失败！");
                return;
            }
            
            // 设置发射者标记，防止击中自己
            BulletCollision bulletCollision = bullet.GetComponent<BulletCollision>();
            if (bulletCollision != null)
            {
                bulletCollision.shooter = this.gameObject;
                Debug.Log($"[{gameObject.name}] Player2设置子弹发射者为: {this.gameObject.name}");
            }
            
            // 先尝试获取Rigidbody2D组件（2D物理）
            Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                // 使用2D物理
                rb2D.gravityScale = 0; // 关闭重力
                rb2D.drag = 0; // 无阻力
                rb2D.angularDrag = 0; // 无角阻力
                
                // 根据面向方向设置2D速度
                Vector2 direction = facingRight ? firePoint.right : -firePoint.right;
                rb2D.velocity = direction * bullspeed;
                
                string directionText = facingRight ? "右" : "左";
                Debug.Log($"[{gameObject.name}] Player2使用2D物理发射子弹！速度: {bullspeed}, 方向: {directionText}");
            }
            else
            {
                // 尝试获取Rigidbody组件（3D物理）
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = bullet.AddComponent<Rigidbody>();
                    Debug.Log($"[{gameObject.name}] 为子弹添加了Rigidbody组件。");
                }
                
                // 使用3D物理
                rb.useGravity = false; // 关闭重力
                rb.drag = 0; // 无空气阻力
                rb.angularDrag = 0; // 无角阻力
                
                // 根据面向方向设置3D速度
                Vector3 direction = facingRight ? firePoint.right : -firePoint.right;
                rb.velocity = direction * bullspeed;
                
                string directionText = facingRight ? "右" : "左";
                Debug.Log($"[{gameObject.name}] Player2使用3D物理发射子弹！速度: {bullspeed}, 方向: {directionText}");
            }
            
            // 自动销毁子弹
            Destroy(bullet, lifeTime);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[{gameObject.name}] Player2射击时发生错误: {e.Message}");
        }
    }
} 