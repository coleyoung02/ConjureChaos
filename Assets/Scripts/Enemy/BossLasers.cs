using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLasers : MonoBehaviour
{
    [SerializeField] private List<Laser> lasers;
    [SerializeField] private GameObject laserPrefab;
    private float horizontalDuration = 1.5f;
    private int routine = 0;
    private int order = 1;

    private void Start()
    {
        //lasers[0].Activate(2f);
    }

    public void VertLasers()
    {
        StartCoroutine(Verticals());
    }

    private IEnumerator Verticals()
    {
        float xOff = 0;
        xOff += UnityEngine.Random.Range(-3f, 3f);
        float negxOff = xOff;
        Instantiate(laserPrefab, new Vector3(xOff, 3, -2), Quaternion.Euler(0, 0, 90f));
        for (int i = 0; i < 2; ++i)
        {
            yield return new WaitForSeconds(.3f);
            xOff += UnityEngine.Random.Range(6f, 9f);
            Instantiate(laserPrefab, new Vector3(xOff, 3, -2), Quaternion.Euler(0, 0, 90f));
            yield return new WaitForSeconds(.3f);
            negxOff -= UnityEngine.Random.Range(4f, 8f);
            Instantiate(laserPrefab, new Vector3(negxOff, 3, -2), Quaternion.Euler(0, 0, 90f));
        }
    }

    private void StartLasers()
    {
        if (routine == 0)
        {

            if (order > 0)
            {
                lasers[0].Activate(horizontalDuration);
            }
            else
            {
                lasers[lasers.Count - 1].Activate(horizontalDuration);
            }
        }
        else if (routine == 1)
        {
            lasers[0 + order].Activate(horizontalDuration);
            lasers[2 + order].Activate(horizontalDuration);
            StartCoroutine(WaitAndLaser(1  + order, horizontalDuration + 2f, horizontalDuration));
            StartCoroutine(WaitAndLaser((3 + order) % lasers.Count, horizontalDuration + 2f, horizontalDuration));
        }
    }

    private IEnumerator WaitAndLaser(int laserIndex, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        lasers[laserIndex].Activate(horizontalDuration);
    }


    public void DoRoutine(int i)
    {
        routine = i;
        order = UnityEngine.Random.Range(0, 2);
        if (routine != 1)
        {
            order = order * 2 - 1;
        }
        StartLasers();
    }


    public void NotifyIn(int index)
    {
        //unused
    }

    public void NotifyOut(int index)
    {
        if (routine == 0)
        {
            if (index + order < lasers.Count && index + order >= 0)
            {
                StartCoroutine(WaitAndLaser(index + order, .1f, horizontalDuration * .4f));
            }
        }
    }
}
