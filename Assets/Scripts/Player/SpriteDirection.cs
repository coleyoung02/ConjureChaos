using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirection : MonoBehaviour
{
    //[SerializeField]
    //private SpriteRenderer spriteRenderer;

    //private ProjectileConjurer _conjurer;
    
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        
        // Saves the conjurer so we only have to get it once
        //_conjurer = FindObjectOfType<ProjectileConjurer>();
    }

    private void Update()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bool flip = mousePos.x > transform.position.x;
        if (Time.timeScale > 0f)
        {
            if (flip)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        //spriteRenderer.flipX = flip;
        //_conjurer.FlipFirePoint(flip);
    }
}
