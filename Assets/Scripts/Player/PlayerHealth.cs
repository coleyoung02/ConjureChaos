using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthIcon;

    int health = 100;
    
    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    public void PlayerTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player is Dead.");
        }
        UpdateHealthUI();
    }

    public void PlayerAddHealth(int healValue)
    {
        health += healValue;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = $"{health} HP";
        healthIcon.fillAmount = (health * 1.0f) / maxHealth;
    }
}
