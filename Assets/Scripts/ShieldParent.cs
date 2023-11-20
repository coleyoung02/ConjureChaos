using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParent : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject child;
    [SerializeField] float downDuration;

    void Update()
    {
        transform.Rotate(0f, 0f, Time.deltaTime * rotateSpeed);
    }

    public void Activate()
    {
        child.SetActive(true);
    }

    public void OnHit()
    {
        StartCoroutine(disableShiled(downDuration));
    }

    private IEnumerator disableShiled(float duration)
    {
        child.SetActive(false);
        yield return new WaitForSeconds(duration);
        child.SetActive(true);
    }
}
