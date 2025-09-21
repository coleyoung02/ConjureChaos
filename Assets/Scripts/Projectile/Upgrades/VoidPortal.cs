using System.Collections;
using UnityEngine;

public class VoidPortal : MonoBehaviour
{
    public const float ImmunityTime = .25f;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Transform visuals;
    private bool isOpened;
    private bool isClosing;
    private Vector3 originalScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = visuals.localScale;
        isOpened = false;
        isClosing = false;
        StartCoroutine(Anim(true, .3f));
        StartCoroutine(ClosePortalAfterDelay(15f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened && collision.gameObject.CompareTag("Enemy"))
        {
            if (!isClosing)
            {
                if (!ps.main.loop)
                {
                    var main = ps.main;
                    main.loop = true;
                }
                ps.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOpened && !col.IsTouchingLayers(enemyLayers))
        {
            var main = ps.main;
            main.loop = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.PortalDamage();
        }
    }

    private IEnumerator ClosePortalAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        var main = ps.main;
        main.loop = false;
        isClosing = true;
        StartCoroutine(Anim(false, .5f));
    }

    private IEnumerator Anim(bool opening, float duration)
    {
        for (float f = 0f; f < duration; f += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            if (opening)
            {
                visuals.localScale = (f / duration) * originalScale;
            }
            else
            {
                visuals.localScale = ((duration - f) / duration) * originalScale;
            }
        }
        if (opening)
        {
            visuals.localScale = originalScale;
            isOpened = true;
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
