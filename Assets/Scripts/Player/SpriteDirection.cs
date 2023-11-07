using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirection : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private ProjectileConjurer _conjurer;
    
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();
    }

    private void Update()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bool flip = mousePos.x > transform.position.x;
        if (Time.timeScale > 0f)
        {
            spriteRenderer.flipX = flip;
            _conjurer.FlipFirePoint(flip);
        }
    }
}
