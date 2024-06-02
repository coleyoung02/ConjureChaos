using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FancyButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool pool;
    private float r;
    private float g;
    private float b;
    private Button button;
    private int change;
    private static float changeRate = 100f;
    private static float bottomVal = 130f;

    private void Awake()
    {
        animator.speed = 0f;
        r = 255f;
        g = 255f;
        b = bottomVal;
        change = 0;
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonSound);
    }

    private void ButtonSound()
    {
        animator.speed = 0;
        AudioManager.instance.PlayUISound(clip);
    }

    private void Update()
    {
        if (pool)
            return;
        if (change == 0) { 
            r = Mathf.Max(r - Time.unscaledDeltaTime * changeRate, bottomVal);
            if (r <= bottomVal + .01f)
            {
                change = 1;
            }
        }
        else if (change == 1)
        {
            b = Mathf.Min(b + Time.unscaledDeltaTime * changeRate, 255f);
            if (b >= 254.99f)
            {
                change = 2;
            }
        }
        else if (change == 2)
        {
            g = Mathf.Max(g - Time.unscaledDeltaTime * changeRate, bottomVal);
            if (g <= bottomVal + .01f)
            {
                change = 3;
            }
        }
        else if(change == 3)
        {
            r = Mathf.Min(r + Time.unscaledDeltaTime * changeRate, 255f);
            if (r >= 254.99f)
            {
                change = 4;
            }
        }
        else if(change == 4)
        {
            b = Mathf.Max(b - Time.unscaledDeltaTime * changeRate, bottomVal);
            if (b <= bottomVal + .01f)
            {
                change = 5;
            }
        }
        else if(change == 5)
        {
            g = Mathf.Min(g + Time.unscaledDeltaTime * changeRate, 255f);
            if (g >= 254.99f)
            {
                change = 0;
            }
        }
        text.color = new Color(r / 255f, g / 255f, b / 255f);
        border.color = new Color(r / 255f, g / 255f, b / 255f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.speed = 1f;
    }

    private void OnDisable()
    {
        animator.speed = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.speed = 0f;
    }
}
