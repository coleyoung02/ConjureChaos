using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FancyButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hovered = false;
    private Vector3 scale;
    private float scaleMult = 1f;
    private float maxScale = 1.05f;
    [SerializeField] private float maxScaleMult = 1f;
    private float sizeChangeRate = .5f;
    private RectTransform rt;
    [SerializeField] private Animator animator;
    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool pool;
    private float r;
    private float g;
    private float b;
    private Button button;
    private int change;

    private void OnEnable()
    {
        hovered = false;
    }

    private void Awake()
    {
        maxScale = 1 + (maxScale - 1) * maxScaleMult;
        sizeChangeRate *= maxScaleMult;
        hovered = false;
        rt = GetComponent<RectTransform>();
        scale = transform.localScale;
        if (animator != null)
        {
            animator.speed = 0;
        }
        r = 255f;
        g = 255f;
        b = PooledShimmer.bottomVal;
        change = 0;
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonSound);
    }

    private void ButtonSound()
    {
        AudioManager.instance.PlayUISoundClick();
    }

    private void Update()
    {
        if (animator != null)
        {
            ColorShift();
        }
        if (hovered)
        {
            scaleMult = Mathf.Clamp(scaleMult + sizeChangeRate * Time.unscaledDeltaTime, 1, maxScale);
        }
        else
        {
            scaleMult = Mathf.Clamp(scaleMult - sizeChangeRate * Time.unscaledDeltaTime, 1, maxScale);
        }
        rt.localScale = scale * scaleMult;
    }

    private void ColorShift()
    {

        if (pool)
            return;
        if (change == 0)
        {
            r = Mathf.Max(r - Time.unscaledDeltaTime * PooledShimmer.changeRate, PooledShimmer.bottomVal);
            if (r <= PooledShimmer.bottomVal + .01f)
            {
                change = 1;
            }
        }
        else if (change == 1)
        {
            b = Mathf.Min(b + Time.unscaledDeltaTime * PooledShimmer.changeRate, 255f);
            if (b >= 254.99f)
            {
                change = 2;
            }
        }
        else if (change == 2)
        {
            g = Mathf.Max(g - Time.unscaledDeltaTime * PooledShimmer.changeRate, PooledShimmer.bottomVal);
            if (g <= PooledShimmer.bottomVal + .01f)
            {
                change = 3;
            }
        }
        else if (change == 3)
        {
            r = Mathf.Min(r + Time.unscaledDeltaTime * PooledShimmer.changeRate, 255f);
            if (r >= 254.99f)
            {
                change = 4;
            }
        }
        else if (change == 4)
        {
            b = Mathf.Max(b - Time.unscaledDeltaTime * PooledShimmer.changeRate, PooledShimmer.bottomVal);
            if (b <= PooledShimmer.bottomVal + .01f)
            {
                change = 5;
            }
        }
        else if (change == 5)
        {
            g = Mathf.Min(g + Time.unscaledDeltaTime * PooledShimmer.changeRate, 255f);
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
        hovered = true;
        AudioManager.instance.PlayUISoundHover();
        if (animator != null)
        {
            animator.speed = 1f;
        }
    }

    private void OnDisable()
    {
        scaleMult = 1;
        hovered = false;
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        if (animator != null)
        {
            animator.speed = 0;
        }
    }
}
