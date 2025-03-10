using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeartPortal : MonoBehaviour
{
    [SerializeField] private GameObject spawnerSprite;
    [SerializeField] private Light2D spawnerLight;
    [SerializeField] private float appearTime = .9f;
    [SerializeField] private float shrinkTime = .25f;
    [SerializeField] private GameObject heartPickup;

    // Start is called before the first frame update
    void Start()
    {
        spawnerSprite.transform.localScale = Vector3.zero;
        StartCoroutine(HeartRoutine());
    }

    private IEnumerator HeartRoutine()
    {
        for (float f = 0f; f < appearTime; f += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            spawnerSprite.transform.localScale =
                new Vector3(f / appearTime, f / appearTime, f / appearTime);
            spawnerLight.intensity = f / appearTime;
            spawnerSprite.transform.rotation = Quaternion.Euler(0, 0, 360f * f / appearTime);
        }
        spawnerSprite.transform.localScale = Vector3.one;
        spawnerLight.intensity = 1f;
        spawnerSprite.transform.rotation = Quaternion.Euler(0, 0, 360f);
        SpawnHeart();

        for (float f = shrinkTime; f >= 0f; f -= Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            spawnerSprite.transform.localScale =
                new Vector3(f / shrinkTime, f / shrinkTime, f / shrinkTime);
            spawnerLight.intensity = f / shrinkTime;
            spawnerSprite.transform.rotation = Quaternion.Euler(0, 0, 360f * f / appearTime);
        }
        spawnerSprite.transform.localScale =
            new Vector3(0, 0, 0);
        spawnerLight.intensity = 0;
        Destroy(gameObject, 1f);
    }

    void SpawnHeart()
    {
        GameObject enemy_instantiated = Instantiate(heartPickup, transform.position + 
            new Vector3(0, 0, -4.6f + UnityEngine.Random.Range(-.02f, .02f)), Quaternion.identity);
    }
}
