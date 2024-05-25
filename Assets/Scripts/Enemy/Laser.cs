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
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject graphics;
    private PlayerHealth playerHealth;
    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (index < 0)
        {
            Activate(2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= 0)
        {
            graphics.transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time * speed) * maxDegs, 0, 0);
        }
        else
        {
            graphics.transform.rotation = Quaternion.Euler(0, Mathf.Sin(Time.time * speed) * maxDegs, 90f);
        }
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.PlayerTakeDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerHealth.PlayerTakeDamage(1);
        }
    }


    private IEnumerator FadeIn()
    {
        for (float t = 0; t < warningTime; t += Time.deltaTime)
        {
            float val = t * t / warningTime / warningTime;
            beam.color = new Color(1f, 1f, 1f, val);
            l.intensity = lightIntensity * val;
            graphics.transform.localScale = new Vector3(baseScale.x, baseScale.y * val, 1);
            if (!particles.isPlaying && t > warningTime * .8f)
            {
                particles.Play();
            }
            yield return new WaitForEndOfFrame();
        }
        beam.color = Color.white;
        l.intensity = lightIntensity;
        graphics.transform.localScale = new Vector3(baseScale.x, baseScale.y, 1);
        playerHurter.enabled = true;
        FindAnyObjectByType<BossLasers>().NotifyIn(index);
    }

    private IEnumerator FadeOut()
    {
        if (index >= 0)
        {
            FindAnyObjectByType<BossLasers>().NotifyOut(index);
        }
        for (float t = fadeOutTime; t > 0; t -= Time.deltaTime)
        {
            float val = t * t / fadeOutTime / fadeOutTime;
            beam.color = new Color(1f, 1f, 1f, val);
            l.intensity = lightIntensity * val;
            graphics.transform.localScale = new Vector3(baseScale.x, baseScale.y * val, 1);
            if (t < 3 * fadeOutTime / 4)
            {
                particles.Stop();
                playerHurter.enabled = false;
            }
            yield return new WaitForEndOfFrame();
        }
        beam.color = new Color(1f, 1f, 1f, 0f);
        l.intensity = 0f;
        graphics.transform.localScale = new Vector3(baseScale.x, 0f, 1f);
        playerHurter.enabled = false;
        if (index < 0)
        {
            Destroy(gameObject);
        }
    }
}
