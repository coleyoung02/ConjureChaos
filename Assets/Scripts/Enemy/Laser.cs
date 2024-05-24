using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private float speed;
    [SerializeField] private float maxDegs;
    [SerializeField] private float warningTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float lightIntensity;
    [SerializeField] private Light2D l;
    [SerializeField] private SpriteRenderer beam;
    [SerializeField] private Collider2D playerHurter;
    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time * speed) * maxDegs, 0, 0);
    }

    public void Activate(float duration)
    {
        StartCoroutine(FadeIn());
        StartCoroutine(WaitAndDeactivate(duration));
    }

    private IEnumerator WaitAndDeactivate(float duration)
    {
        yield return new WaitForSeconds(duration + warningTime);
        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeIn()
    {
        for (float t = 0; t < warningTime; t += Time.deltaTime)
        {
            beam.color = new Color(1f, 1f, 1f, t / warningTime);
            l.intensity = lightIntensity * t / warningTime;
            transform.localScale = new Vector3(baseScale.x, baseScale.y * t/warningTime, 1);
            yield return new WaitForEndOfFrame();
        }
        beam.color = Color.white;
        l.intensity = lightIntensity;
        transform.localScale = new Vector3(baseScale.x, baseScale.y, 1);
        playerHurter.enabled = true;
        FindAnyObjectByType<BossLasers>().NotifyIn(index);
    }

    private IEnumerator FadeOut()
    {
        FindAnyObjectByType<BossLasers>().NotifyOut(index);
        for (float t = fadeOutTime; t > 0; t -= Time.deltaTime)
        {
            beam.color = new Color(1f, 1f, 1f, t / fadeOutTime);
            l.intensity = lightIntensity * t / fadeOutTime;
            transform.localScale = new Vector3(baseScale.x, baseScale.y * t / fadeOutTime, 1);
            if (t < fadeOutTime / 2)
            {
                playerHurter.enabled = false;
            }
            yield return new WaitForEndOfFrame();
        }
        beam.color = new Color(1f, 1f, 1f, 0f);
        l.intensity = 0f;
        transform.localScale = new Vector3(baseScale.x, 0f, 1f);
        playerHurter.enabled = false;
    }
}
