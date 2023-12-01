using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    public void EnableUI(GameObject canvas)
    {
        canvas.SetActive(true);
    }

    public void DisableUI(GameObject canvas)
    {
        canvas.SetActive(false);
    }

}
