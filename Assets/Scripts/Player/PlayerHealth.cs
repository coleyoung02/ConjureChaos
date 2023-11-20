using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeartImage;
    [SerializeField] Sprite emptyHeartImage;
    [SerializeField] float invincibilityDuration = 2f;

    int currentHealth = 1;
    bool isInvincible = false;
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isInvincible) {
            Debug.Log("Player is invincible.");
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SceneManager.LoadScene("LoseScreen");
        }
        isInvincible = true;
        StartCoroutine(InvincibilityTimer(invincibilityDuration));
        UpdateHealthUI();
    }

    public void PlayerAddHealth(int healValue)
    {
        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void AddMaxHealth(int value)
    {
        maxHealth += value;
        if (maxHealth > hearts.Length)
        {
            maxHealth = hearts.Length;
        }
        UpdateHealthUI();
    }

    public void SubtractMaxHealth(int value)
    {
        maxHealth -= value;
        if (maxHealth < 1)
        {
            maxHealth = 1;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth} HP";
        RefreshHeartDisplay();
    }

    // use this when you first start the game or when you update 
    // maxHealth so that the UI can display the accurate max health.
    void RefreshHeartDisplay()
    {
        if(fullHeartImage == null || emptyHeartImage == null)
        {
            Debug.Log($"heart sprites are not set");
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                Debug.Log($"No hearts in index {i}.");
                continue;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
                hearts[i].sprite = (i < currentHealth)? fullHeartImage : emptyHeartImage;
            } 
            else 
            {
                hearts[i].enabled = false;
            }
        }
    }

    IEnumerator InvincibilityTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        Debug.Log("Player is not invincible anymore.");
    }
}
