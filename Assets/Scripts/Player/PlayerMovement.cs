using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [SerializeField] BoxCollider2D foot;

    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Collider2D myCollider;

    void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        Move();
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
