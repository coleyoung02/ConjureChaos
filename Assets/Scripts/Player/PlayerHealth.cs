using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
    [SerializeField] private GameObject invulIndicator;
    [SerializeField] private GameObject invulIndicator2;
    [SerializeField] Image[] hearts;
    [SerializeField] Image hurtOverlay;
    private float hurtTime;
    private static float overlayDuration = 1.2f;
    private static float maxOpacity = .4f;
    [SerializeField] Sprite fullHeartImage;
    [SerializeField] Sprite emptyHeartImage;
    [SerializeField] private Slider lifeStealSlider;
    private float lifeStealSliderTarget;
    [SerializeField] private GameObject lifeStealFullnessIndicator;
    [SerializeField] float invincibilityDuration = 2f;
    private CameraManager camMan;

    private static int lifeStealKills = 10;

    int currentHealth = 1;
    bool isInvincible = false;
    private int enemiesKilled = 0;
    
    void Start()
    {
        hurtTime = 0f;
        SetHealth(maxHealth);
        UpdateHealthUI();
        camMan = FindAnyObjectByType<CameraManager>();
    }

    private void Update()
    {
        if (lifeStealSlider.value < lifeStealSliderTarget)
        {
            lifeStealSlider.value = Mathf.Min(lifeStealSliderTarget, lifeStealSlider.value + Time.deltaTime * .6f);
        }
        else if (lifeStealSlider.value > lifeStealSliderTarget)
        {
            lifeStealSlider.value = lifeStealSliderTarget;
        }
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
            hurtTime -= Time.unscaledDeltaTime;
        }
    }

    public void ActivateLifeSteal()
    {
        lifeStealSlider.gameObject.SetActive(true);
        lifeStealFullnessIndicator.gameObject.SetActive(currentHealth == maxHealth);
    }

    public void EnemyKilled()
    {
        if (currentHealth != maxHealth)
        {
            enemiesKilled++;
            lifeStealSliderTarget = enemiesKilled / (float)lifeStealKills;
        }
        if (enemiesKilled >= lifeStealKills && currentHealth < maxHealth)
        {
            PlayerAddHealth(1);
            enemiesKilled = 0;
            lifeStealSlider.value = 0;
            lifeStealFullnessIndicator.SetActive(true);
        }
    }

    private void SetHealth(int health)
    {
        currentHealth = health;
        lifeStealFullnessIndicator.SetActive(currentHealth == maxHealth);
        if (currentHealth == maxHealth)
        {
            enemiesKilled = 0;
        }
        UpdateHealthUI();
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isInvincible) {
            return;
        }
        hurtTime = overlayDuration;
        SetHealth(currentHealth - damage);
        AudioManager.instance.PlayHurtNoise();
        if (currentHealth <= 0)
        {
            SetHealth(0);
            SceneManager.LoadScene("LoseScreen");
        }
        else if (currentHealth == maxHealth - 1)
        {
            enemiesKilled = 0;
            lifeStealSliderTarget = 0;
        }
        isInvincible = true;
        lifeStealFullnessIndicator.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(InvincibilityTimer(invincibilityDuration));
        StartCoroutine(InvincibilityIndicator(invincibilityDuration - .1f));
        UpdateHealthUI();
        camMan.TakeDamage();
    }

    public void PlayerAddHealth(int healValue)
    {
        SetHealth(currentHealth + healValue);
        if (currentHealth > maxHealth)
        {
            SetHealth(maxHealth);
        }
        if (currentHealth == maxHealth)
        {
            enemiesKilled = 0;
            lifeStealSlider.value = 0;
            lifeStealFullnessIndicator.SetActive(true);
        }
        if (currentHealth == maxHealth)
        {
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
            lifeStealSlider.value = 0;
            lifeStealFullnessIndicator.SetActive(true);
        }
        else
        {
            lifeStealFullnessIndicator.SetActive(false);
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
        currentHealth += value;
        SetHealth(Mathf.Min(currentHealth, maxHealth));
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
            SetHealth(maxHealth);
        }
        if (currentHealth == maxHealth)
        {
            enemiesKilled = 0;
            lifeStealSlider.value = 0;
            lifeStealFullnessIndicator.SetActive(true);
        }
        UpdateHealthUI();
    }

    public void HealToFull()
    {
        SetHealth(maxHealth);
        enemiesKilled = 0;
        lifeStealSlider.value = 0;
        lifeStealFullnessIndicator.SetActive(true);
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

    IEnumerator InvincibilityIndicator(float duration)
    {
        yield return new WaitForSeconds(.1f);
        invulIndicator.SetActive(true);
        invulIndicator2.SetActive(true);
        float multiplier = 1f;
        for (float f = .1f; f < duration; f += Time.deltaTime)
        {
            invulIndicator.transform.Rotate(new Vector3(0, 0, -100f * Time.deltaTime));
            invulIndicator.transform.localScale = new Vector3(Mathf.Cos(Time.time * 12f) * .125f  + 1.125f * multiplier, Mathf.Sin(Time.time * 12f) * .125f + 1.125f * multiplier, 1);
            invulIndicator2.transform.Rotate(new Vector3(0, 0, 100f * Time.deltaTime));
            invulIndicator2.transform.localScale = new Vector3(Mathf.Sin(Time.time * 12f) * .125f + 1.125f * multiplier, Mathf.Cos(Time.time * 12f) * .125f + 1.125f * multiplier, 1);
            if (f > duration - .5f)
            {
                multiplier = Mathf.Lerp(1, .65f, (f - (duration - .5f)) / .5f);
                if (f % .1f > .05f)
                {
                    invulIndicator.SetActive(true);
                    invulIndicator2.SetActive(true);
                }
                else
                {
                    invulIndicator.SetActive(false);
                    invulIndicator2.SetActive(false);
                }
            }
            yield return new WaitForEndOfFrame();
        }
        invulIndicator.SetActive(false);
        invulIndicator2.SetActive(false);
        isInvincible = false;
    }
}
