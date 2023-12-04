using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PooledShimmer : MonoBehaviour
{
    private float r;
    private float g;
    private float b;
    private int change;
    private static float changeRate = 100f;
    private static float bottomVal = 130f;
    private List<Image> borders;
    private TextMeshProUGUI[] texts;

    private void Awake()
    {
        r = 255f;
        g = 255f;
        b = bottomVal;
        change = 0;
        ReFetch();
    }

    public void ReFetch()
    {
        texts = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        GameObject[] gos = GameObject.FindGameObjectsWithTag("ShimmerBorder");
        borders = new List<Image>();
        foreach (GameObject go in gos)
        {
            borders.Add(go.GetComponent<Image>());
        }

    }

    private void Update()
    {
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
        foreach (Image i in borders)
        {
            i.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        foreach (TextMeshProUGUI t in texts)
        {
            t.color = new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
