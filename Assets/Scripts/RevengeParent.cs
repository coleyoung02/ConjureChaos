using System.Collections;
using UnityEngine;

public class RevengeParent : MonoBehaviour
{
    [SerializeField] private GameObject circleCollider;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float duration;
    [SerializeField] private float maxScale;
    [SerializeField] private AudioSource killSource;
    private AudioManager am;

    private void Start()
    {
        am = FindAnyObjectByType<AudioManager>();
    }

    public void Activate()
    {
        StartCoroutine(ExpandCollider());
    }

    private IEnumerator ExpandCollider()
    {
        yield return new WaitForSeconds(.025f);
        circleCollider.transform.localScale = .025f * Vector3.one;
        circleCollider.SetActive(true);
        particles.Play();
        for (float f = 0; f < duration;  f += Time.deltaTime)
        {
            circleCollider.transform.localScale = maxScale * Vector3.one * f / duration;
            yield return new WaitForEndOfFrame();
        }
        circleCollider.transform.localScale = maxScale * Vector3.one;
        circleCollider.SetActive(false);
    }

    public void OnHit()
    {
        killSource.pitch = UnityEngine.Random.Range(-.05f, .05f) + am.GetPitch();
        killSource.Play();
    }
}
