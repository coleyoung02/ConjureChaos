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

    // Start is called before the first frame update
    public virtual void Start()
    {
        stunned = false;
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
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
    void Update()
    {
        if (!stunned)
        {
            OnUpdate();
        }   
    }

    protected abstract void OnUpdate();
}
