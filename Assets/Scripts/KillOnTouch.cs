using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    [SerializeField] private RevengeParent rp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Is this the best way to do this?
            Enemy script = collision.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(99999f);
            rp.OnHit();
        }
    }
}
