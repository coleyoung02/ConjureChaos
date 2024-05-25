using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLasers : MonoBehaviour
{
    [SerializeField] private List<Laser> lasers;
    [SerializeField] private GameObject laserPrefab;
    private int routine = 0;

    private void Start()
    {
        //lasers[0].Activate(2f);
        VertLasers();
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

    }


    public void DoRoutine(int i)
    {
        routine = i;
    }


    public void NotifyIn(int index)
    {
        //unused
    }

    public void NotifyOut(int index)
    {
        if (routine == 0)
        {
            if (index + 1 < lasers.Count)
            {
                lasers[index + 1].Activate(2f);
            }
        }
    }
}
