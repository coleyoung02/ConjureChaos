using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPortalManager : MonoBehaviour
{
    [SerializeField] private ProjectileConjurer conjurer;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private GameObject portal;
    [SerializeField] private List<Transform> spawnLocations;
    private GameObject player;
    private bool running;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conjurer = FindAnyObjectByType<ProjectileConjurer>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        running = false;
    }

    public void Activate()
    {
        running = true;
        StartCoroutine(DoHearts());
    }

    private IEnumerator DoHearts()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minDelay, maxDelay));
        Vector3 pos = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count)].position;
        int i = 0;
        int maxIters = 50;
        while (i < maxIters && ((Vector2)pos - (Vector2)player.transform.position).magnitude < 7.5f)
        {
            pos = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count)].position;
        }
        Instantiate(portal, pos, Quaternion.identity);
        StartCoroutine(DoHearts());
    }

    public void CancelFuture()
    {
        StopAllCoroutines();
    }

    public void ClearAll()
    {
        HeartPickup[] hearts = FindObjectsByType<HeartPickup>(FindObjectsSortMode.None);
        HeartPortal[] portals = FindObjectsByType<HeartPortal>(FindObjectsSortMode.None);
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            Destroy(hearts[i].gameObject);
        }
        for (int i = portals.Length - 1; i >= 0; i--)
        {
            Destroy(portals[i].gameObject);
        }
        StopAllCoroutines();
    }

    public void NewWave()
    {
        ClearAll();
        if (running)
        {
            StartCoroutine(DoHearts());
        }
    }
}
