using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// 健康系统 - 处理生命值、伤害和死亡
public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // 最大生命值
    public int currentHealth; // 当前生命值
    
    [Header("Damage Settings")]
    public float invincibilityDuration = 0.1f; // 无敌时间
    private bool isInvincible = false; // 是否处于无敌状态
    private float invincibilityTimer = 0f; // 无敌计时器
    
    [Header("Visual Effects")]
    public float flashDuration = 0.1f; // 闪烁持续时间
    private SpriteRenderer spriteRenderer; // 精灵渲染器
    private Color originalColor; // 原始颜色
    private Coroutine flashCoroutine; // 闪烁协程
    
    [Header("Death Settings")]
    public Vector3 respawnPosition; // 重生位置
    public float respawnDelay = 2f; // 重生延迟
    
    [Header("Events")]
    public UnityEvent<int> OnHealthChanged; // 生命值改变事件
    public UnityEvent OnDamaged; // 受伤事件
    public UnityEvent OnDeath; // 死亡事件
    public UnityEvent OnRespawn; // 重生事件
    
    private bool isDead = false; // 是否死亡
    
    void Start()
    {
        // 初始化
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        // 设置初始重生位置为当前位置
        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }
        
        // 触发初始生命值事件
        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"[{gameObject.name}] 健康系统初始化完成 - 生命值: {currentHealth}/{maxHealth}");
    }
    
    void Update()
    {
        // 处理无敌时间
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                Debug.Log($"[{gameObject.name}] 无敌状态结束");
            }
        }
        
        // 掉落死亡判定 - Y轴小于-10时死亡
        if (transform.position.y < -10f && !isDead)
        {
            Debug.Log($"[{gameObject.name}] 掉落死亡 - Y轴位置: {transform.position.y}");
            currentHealth = 0;
            OnHealthChanged?.Invoke(currentHealth);
            Die();
        }
    }
    
    // 受到伤害
    public void TakeDamage(int damage, GameObject damageSource = null)
    {
        // 如果已死亡或处于无敌状态，忽略伤害
        if (isDead || isInvincible)
        {
            return;
        }
        
        // 计算伤害
        int actualDamage = Mathf.Min(damage, currentHealth);
        currentHealth -= actualDamage;
        
        // 确保生命值不低于0
        currentHealth = Mathf.Max(currentHealth, 0);
        
        Debug.Log($"[{gameObject.name}] 受到伤害: {actualDamage}, 剩余生命值: {currentHealth}/{maxHealth}");
        
        // 触发事件
        OnHealthChanged?.Invoke(currentHealth);
        OnDamaged?.Invoke();
        
        // 开始无敌时间
        StartInvincibility();
        
        // 播放受伤效果
        StartDamageEffect();
        
        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // 恢复生命值
    public void Heal(int healAmount)
    {
        if (isDead)
        {
            return;
        }
        
        int actualHeal = Mathf.Min(healAmount, maxHealth - currentHealth);
        currentHealth += actualHeal;
        
        Debug.Log($"[{gameObject.name}] 恢复生命值: {actualHeal}, 当前生命值: {currentHealth}/{maxHealth}");
        
        // 触发事件
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    // 开始无敌状态
    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        Debug.Log($"[{gameObject.name}] 开始无敌状态，持续时间: {invincibilityDuration}秒");
    }
    
    // 播放受伤效果
    private void StartDamageEffect()
    {
        if (spriteRenderer != null)
        {
            // 停止之前的闪烁效果
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            
            // 开始新的闪烁效果
            flashCoroutine = StartCoroutine(FlashEffect());
        }
    }
    
    // 闪烁效果协程
    private IEnumerator FlashEffect()
    {
        float flashTimer = 0f;
        bool isFlashing = false;
        
        while (flashTimer < invincibilityDuration)
        {
            // 切换颜色
            if (isFlashing)
            {
                spriteRenderer.color = originalColor;
            }
            else
            {
                spriteRenderer.color = Color.red;
            }
            
            isFlashing = !isFlashing;
            flashTimer += flashDuration;
            
            yield return new WaitForSeconds(flashDuration);
        }
        
        // 恢复原始颜色
        spriteRenderer.color = originalColor;
    }
    
    // 死亡处理
    private void Die()
    {
        if (isDead)
        {
            return;
        }
        
        isDead = true;
        Debug.Log($"[{gameObject.name}] 死亡 - 玩家被删除");
        
        // 触发死亡事件
        OnDeath?.Invoke();
        
        // 直接删除玩家对象
        Destroy(gameObject);
    }
    
    // 重生协程
    private IEnumerator RespawnCoroutine()
    {
        Debug.Log($"[{gameObject.name}] {respawnDelay}秒后重生...");
        
        yield return new WaitForSeconds(respawnDelay);
        
        Respawn();
    }
    
    // 重生
    public void Respawn()
    {
        Debug.Log($"[{gameObject.name}] 重生");
        
        // 重置状态
        isDead = false;
        currentHealth = maxHealth;
        isInvincible = false;
        invincibilityTimer = 0f;
        
        // 重置位置
        transform.position = respawnPosition;
        
        // 重新启用玩家控制
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        // 恢复颜色
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        // 触发事件
        OnHealthChanged?.Invoke(currentHealth);
        OnRespawn?.Invoke();
    }
    
    // 设置重生点
    public void SetRespawnPoint(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
        Debug.Log($"[{gameObject.name}] 设置新的重生点: {respawnPosition}");
    }
    
    // 获取生命值百分比
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
    // 检查是否死亡
    public bool IsDead()
    {
        return isDead;
    }
    
    // 检查是否处于无敌状态
    public bool IsInvincible()
    {
        return isInvincible;
    }
} 