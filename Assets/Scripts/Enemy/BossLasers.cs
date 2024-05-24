using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLasers : MonoBehaviour
{
    [SerializeField] private List<Laser> lasers;
    private int routine = 0;

    private void Start()
    {
        lasers[0].Activate(2f);
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
