using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float fallPower = 2f;
    [SerializeField] BoxCollider2D foot;

    [SerializeField] Animator playerAnimator;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Collider2D myCollider;
    GameObject currentOneWayPlatform;
    private bool usedFall = false;

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
        if (foot.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            usedFall = false;
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
        myRigidbody.linearVelocity += new Vector2(0, -fallPower); //add some force to the fall
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(myCollider, platformCollider, false);
    }

    void OnFall(InputValue value)
    {
        if (Time.timeScale > 0)
        {
            if (value.isPressed && currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
                usedFall = true;
            }
            else if (!usedFall)
            {
                myRigidbody.linearVelocity += new Vector2(0, -fallPower);
                usedFall = true;
            }
        }
    }

    public void UpdateMoveSpeed(float mult)
    {
        moveSpeed *= mult;
    }

    //Handles the player's movement. Also handles other restrictions such as whether or not the player
    //is paused (for future implementations).
    void Move() {
        myRigidbody.linearVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        if(Mathf.Abs(moveInput.x) < Mathf.Epsilon)
        {
            playerAnimator.SetBool("isWalking", false);
        }
        else
        {
            playerAnimator.SetBool("isWalking", true);
        }
        if (myRigidbody.linearVelocity.y <= 0.05f)
        {
            playerAnimator.SetBool("isFalling", false);
        }
        else
        {
            playerAnimator.SetBool("isFalling", true);
        }
        
    }

    //Receives the move inputs from the input system.
    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    //Receives the jump inputs from the input system and makes the player jump.
    void OnJump(InputValue value) {
        if (Time.timeScale > 0)
        {
            if (value.isPressed)
            {

                //super cool effect where the wizard has multiple jumps and can FLY
                //myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, jumpPower);

                //regular schmegular jump where the wizard is bound to gravity.
                if (foot.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, jumpPower);
                    playerAnimator.SetTrigger("Jump");
                }
            }
        }
    }
}
