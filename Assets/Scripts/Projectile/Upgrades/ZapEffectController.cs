using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class ZapEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private float velocity;
    [SerializeField] private Rigidbody2D rb;
    private Enemy target;
    private float damage;
    private bool used;

    private void Start()
    {
        Destroy(gameObject, .25f);
        Destroy(ps.gameObject, .25f);
        used = false;
    }

    private void Update()
    {
        if (!used && target != null && !target.gameObject.IsDestroyed())
        {
            Vector2 moveDir = target.transform.position - transform.position;
            rb.linearVelocity = moveDir.normalized * velocity;
        }
    }

    public void SetTarget(Enemy target)
    {
        this.target = target;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage * 15f;
    }

    public void SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        HurtEnemy(myCollider);
    }

    private void HurtEnemy(Collider2D myCollider)
    {
        if (!used)
        {
            if (myCollider.CompareTag("Enemy"))
            {
                Enemy script = myCollider.gameObject.GetComponent<Enemy>();
                if (script != target)
                {
                    return;
                }
                Vector3 hitLoc = transform.position + new Vector3(rb.linearVelocity.normalized.x, rb.linearVelocity.normalized.y) * .25f;
                hitLoc.z = -9.5f;
                hitLoc.x += UnityEngine.Random.Range(-.3f, .3f);
                hitLoc.y += UnityEngine.Random.Range(-.3f, .3f);
                script.DamageEnemy(damage, hitLoc);
                used = true;
                StartCoroutine(HandleParticleDeath());
            }
        }
    }

    private IEnumerator HandleParticleDeath()
    {
        yield return new WaitForSeconds(.035f);
        ps.transform.parent = null;
        Destroy(gameObject);
    }
}
