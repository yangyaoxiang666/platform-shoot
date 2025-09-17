using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// 玩家控制器 - 处理移动、跳跃和平台穿透
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movespeed = 5f; // 移动速度
    
    [Header("Jump Settings")]
    public float maxJumps = 0.5f; // 最大跳跃次数
    private int currentJumps = 0; // 当前跳跃次数
    
    [Header("Ground Check")]
    public Transform groundCheck; // 地面检测点
    public float groundCheckDistance = 0.2f; // 地面检测距离
    public LayerMask groundLayer; // 地面图层
    private bool isGrounded; // 是否在地面
    
    [Header("Head Check")]
    public Transform headCheck; // 头顶检测点
    public float headCheckDistance = 0.2f; // 头顶检测距离
    private bool isHeadGrounded; // 头顶是否碰到平台
    
    [Header("Phase Settings")]
    public float phaseDuration = 0.5f; // 穿透持续时间
    public float minPhaseVelocity = 0.1f; // 最小穿透速度
    private bool isPhasing = false; // 是否正在穿透
    private float phaseTimer = 0f; // 穿透计时器
    
    private Rigidbody2D rb; // 刚体组件
    private Collider2D playerCollider; // 碰撞器组件
    private SpriteRenderer spriteRenderer; // 精灵渲染器
    private Color originalColor; // 原始颜色
    
    void Start()
    {
        // 获取组件引用
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        
        // 检测地面
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        
        // 检测头顶平台
        RaycastHit2D headHit = Physics2D.Raycast(headCheck.position, Vector2.up, headCheckDistance, groundLayer);
        isHeadGrounded = headHit.collider != null;

        // 处理穿透计时
        if (isPhasing)
        {
            phaseTimer -= Time.deltaTime;
            if (phaseTimer <= 0)
            {
                StopPhasing();
            }
        }

        // 手动穿透 - 按S键
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (isGrounded && !isPhasing)
            {
                StartPhasing();
            }
        }
        
        // 头顶碰撞自动穿透
        if (isHeadGrounded && !isPhasing && rb != null && rb.velocity.y > minPhaseVelocity)
        {
            StartPhasing();
        }
        
        // 调试射线显示
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(headCheck.position, Vector2.up * headCheckDistance, isHeadGrounded ? Color.blue : Color.yellow);
        
        // 在地面时重置跳跃次数
        if (isGrounded && !isPhasing)
        {
            currentJumps = 0;
        }

        // 水平移动 - 只使用WASD键
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        
        if (rb != null)
        {
            rb.velocity = new Vector2(horizontalInput * movespeed, rb.velocity.y);
        }

        // 跳跃输入 - 使用W键
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }
    
    void Jump()
    {
        // 检查跳跃次数限制
        if (currentJumps < maxJumps)
        {
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // 重置垂直速度
                rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // 添加跳跃力
                currentJumps++;
            }
        }
    }
    
    void StartPhasing()
    {
        // 开始穿透状态
        isPhasing = true;
        phaseTimer = phaseDuration;
        
        // 禁用碰撞器
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
        
        // 设置半透明效果
        if (spriteRenderer != null)
        {
            Color phasingColor = originalColor;
            phasingColor.a = 0.5f;
            spriteRenderer.color = phasingColor;
        }
        
        Debug.Log("开始穿透平台 - 玩家虚化");
    }
    
    void StopPhasing()
    {
        // 停止穿透状态
        isPhasing = false;
        phaseTimer = 0f;
        
        // 重新启用碰撞器
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
        
        // 恢复原始颜色
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        Debug.Log("停止穿透平台 - 玩家实体化");
    }
}