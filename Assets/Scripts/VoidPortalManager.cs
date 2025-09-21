using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VoidPortalManager : MonoBehaviour
{
    [SerializeField] private List<Transform> locations;
    [SerializeField] private GameObject portal;
    private List<float> timers;
    private ProjectileConjurer conjurer;

    private void Start()
    {
        conjurer = FindAnyObjectByType<ProjectileConjurer>();
        timers = new List<float>();
        for (int i = 0; i < locations.Count; ++i)
        {
            timers.Add(0);
        }
        OpenPortal();
        StartCoroutine(StartNewPortal(GetDelay()));
    }

    private void Update()
    {
        for (int i = 0; i < timers.Count; ++i)
        {
            if (timers[i] >= 0)
            {
                timers[i] -= Time.deltaTime;
            }
        }
    }

    public void EndOfWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartNewPortal(float delay)
    {
        yield return new WaitForSeconds(delay);
        OpenPortal();
        StartCoroutine(StartNewPortal(GetDelay()));
    }

    private void OpenPortal()
    {
        int index = PickRandomIndexWithZero();
        Instantiate(portal, locations[index]);
        timers[index] += VoidPortal.Duration + 2.5f;
    }

    private float GetDelay()
    {
        return VoidPortal.Duration / (1f + conjurer.GetStats()[Stats.ShotCount]) + UnityEngine.Random.Range(-2f, 0f);
    }

    private int PickRandomIndexWithZero()
    {
        List<int> indicies = new List<int>();
        for (int i = 0; i < timers.Count; ++i)
        {
            if (timers[i] <= 0)
            {
                indicies.Add(i);
            }
        }
        if (indicies.Count > 0)
        {
            return indicies[UnityEngine.Random.Range(0, indicies.Count)];
        }
        else
        {
            return -1;
        }
    }
}
