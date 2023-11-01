using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float fallPower = 2f;
    [SerializeField] BoxCollider2D foot;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Collider2D myCollider;
    GameObject currentOneWayPlatform;

    void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        Move();
    }


    //One way platforms developed from https://www.youtube.com/watch?v=7rCUt6mqqE8
    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(myCollider, platformCollider);
        myRigidbody.velocity += new Vector2(myRigidbody.velocity.x, -fallPower); //add some force to the fall
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(myCollider, platformCollider, false);
    }

    void OnFall(InputValue value)
    {
        if(value.isPressed && currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    //Handles the player's movement. Also handles other restrictions such as whether or not the player
    //is paused (for future implementations).
    void Move() {
        myRigidbody.velocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
    }

    //Receives the move inputs from the input system.
    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    //Receives the jump inputs from the input system and makes the player jump.
    void OnJump(InputValue value) {
        if(value.isPressed) {

            //super cool effect where the wizard has multiple jumps and can FLY
            //myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, jumpPower);

            //regular schmegular jump where the wizard is bound to gravity.
            if(foot.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
                myRigidbody.velocity += new Vector2 (0, jumpPower);
            }
        }
    }
}
