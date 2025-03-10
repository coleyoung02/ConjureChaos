using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject pickupParticles;
    private int beats = 0;
    private static int maxBeats = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            sr.sprite = emptySprite;
            Instantiate(pickupParticles, transform.position, Quaternion.identity);
            FindAnyObjectByType<AudioManager>().PlayUIClip(pickupSound);
            FindAnyObjectByType<PlayerHealth>().PlayerAddHealth(1);
            GetComponent<Animator>().SetTrigger("FastShrink");
            GetComponent<Animator>().speed = 2f;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void Beat()
    {
        beats++;
        if (beats >= maxBeats)
        {
            GetComponent<Animator>().SetTrigger("Shrink");
        }
    }

    public void Done()
    {
        Destroy(gameObject);
    }
}
