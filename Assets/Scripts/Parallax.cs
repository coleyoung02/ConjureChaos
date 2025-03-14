using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = getNewPos();
    }

    private Vector3 getNewPos()
    {
        Vector3 playerPos = FindAnyObjectByType<CameraManager>().gameObject.transform.position;
        return new Vector3(playerPos.x / 6, playerPos.y / 10, transform.position.z);
    }
}
