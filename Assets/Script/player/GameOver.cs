using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public HealthSystem Player1Health;
    public HealthSystem Player2Health;
    public GameObject GameOverUI;
    private bool isGameOver = false;
    void Start()
    {
        if (GameOverUI != null)
        {
            GameOverUI.SetActive(false);
        }
    }
    void Update()
    {
        if ((!isGameOver && Player1Health.currentHealth <= 0)||(!isGameOver && Player2Health.currentHealth <= 0))
        {
            GameOverCheck();
        }
        
    }

    public void GameOverCheck()
    {
        if (Player1Health.currentHealth <= 0|| Player2Health.currentHealth <= 0)
        {
            isGameOver = true;
            Time.timeScale = 0f;//暂停游戏
            GameOverUI.SetActive(true);
            Debug.Log("Game Over");
         }    
    } 

}
