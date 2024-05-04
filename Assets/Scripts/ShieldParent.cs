using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShieldParent : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject child;
    [SerializeField] float downDuration;
    [SerializeField] private Light2D sLight;

    private float r;
    private float g;
    private float b;
    private int change;
    private static float changeRate = 100f;
    private static float bottomVal = 130f;

    void Update()
    {
        transform.Rotate(0f, 0f, Time.deltaTime * rotateSpeed);
        if (change == 0)
        {
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
        else if (change == 3)
        {
            r = Mathf.Min(r + Time.unscaledDeltaTime * changeRate, 255f);
            if (r >= 254.99f)
            {
                change = 4;
            }
        }
        else if (change == 4)
        {
            b = Mathf.Max(b - Time.unscaledDeltaTime * changeRate, bottomVal);
            if (b <= bottomVal + .01f)
            {
                change = 5;
            }
        }
        else if (change == 5)
        {
            g = Mathf.Min(g + Time.unscaledDeltaTime * changeRate, 255f);
            if (g >= 254.99f)
            {
                change = 0;
            }
        }
        sLight.color = new Color(r / 255f, g / 255f, b / 255f);
    }

    public void Activate()
    {
        child.SetActive(true);
    }

    public void OnHit()
    {
        StartCoroutine(disableShiled(downDuration));
    }

    private IEnumerator disableShiled(float duration)
    {
        child.SetActive(false);
        yield return new WaitForSeconds(duration);
        child.SetActive(true);
    }
}
