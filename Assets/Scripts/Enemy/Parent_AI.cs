using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parent_AI : MonoBehaviour
{
    protected GameObject player;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float speed = 1.0f;
    protected bool stunned;
    private Coroutine stunRoutine;

    public virtual void Awake()
    {
        stunned = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void Stun()
    {
        //boss should be unstunnable
        if (this is BossAI)
        {
            return;
        }
        stunned = true;
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
        }
        stunRoutine = StartCoroutine(unStun());
    }

    private IEnumerator unStun()
    {
        yield return new WaitForSeconds(.2f);
        stunned = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!stunned)
        {
            OnUpdate();
        }  
    }

    protected abstract void OnUpdate();
}
