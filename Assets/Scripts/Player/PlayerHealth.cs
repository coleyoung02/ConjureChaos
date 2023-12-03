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
    [SerializeField] Image hurtOverlay;
    private float hurtTime;
    private static float overlayDuration = 1.2f;
    private static float maxOpacity = .4f;
    [SerializeField] Sprite fullHeartImage;
    [SerializeField] Sprite emptyHeartImage;
    [SerializeField] float invincibilityDuration = 2f;

    int currentHealth = 1;
    bool isInvincible = false;
    private int enemiesKilled = 0;
    
    void Start()
    {
        hurtTime = 0f;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        
        if (hurtTime > 0f)
        {
            float buildDuration = overlayDuration * .05f;
            float steadyDuration = overlayDuration * .3f;
            float dropDuration = overlayDuration - buildDuration - steadyDuration;
            if (hurtTime > overlayDuration - buildDuration)
            {
                Color c = hurtOverlay.color;
                c.a = Mathf.Lerp(0, maxOpacity, (overlayDuration - hurtTime) / buildDuration);
                hurtOverlay.color = c;
            }
            else if (hurtTime <= dropDuration)
            {
                Color c = hurtOverlay.color;
                c.a = Mathf.Lerp(0, maxOpacity, hurtTime / dropDuration);
                hurtOverlay.color = c;
            }
            hurtTime -= Time.deltaTime;
        }
    }

    public void EnemyKilled()
    {
        if (currentHealth != maxHealth)
        {
            enemiesKilled++;
        }
        if (enemiesKilled >= 10 && currentHealth < maxHealth)
        {
            PlayerAddHealth(1);
            enemiesKilled = 0;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isInvincible) {
            return;
        }
        hurtTime = overlayDuration;
        currentHealth -= damage;
        AudioManager.instance.PlayHurtNoise();
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
        if (currentHealth == maxHealth)
        {
            enemiesKilled = 0;
        }
        UpdateHealthUI();
    }

    public void ChangeMaxHealth(int value, bool absolute)
    {
        int change = value;
        if (absolute)
        {
            change = value - maxHealth;
        }
        if (change > 0)
        {
            AddMaxHealth(change);
        }
        else
        {
            SubtractMaxHealth(-change);
        }
        if (currentHealth == maxHealth)
        {
            enemiesKilled = 0;
        }
    }

    public void AddMaxHealth(int value)
    {
        maxHealth += value;
        if (maxHealth > hearts.Length)
        {
            maxHealth = hearts.Length;
        }
        currentHealth += value;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
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

    public void HealToFull()
    {
        currentHealth = maxHealth;
        enemiesKilled = 0;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        RefreshHeartDisplay();
    }

    // use this when you first start the game or when you update 
    // maxHealth so that the UI can display the accurate max health.
    void RefreshHeartDisplay()
    {
        if(fullHeartImage == null || emptyHeartImage == null)
        {
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
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
    }
}
