using UnityEngine;
using Unity.Netcode;

public abstract class PaddleController : NetworkBehaviour
{
    //Protected variables for paddle speed and rigidbody
    protected float speed = 8.0f;
    protected Rigidbody2D paddle;

    //Initialize the rb component for the paddle
    void Start() {
        paddle = GetComponent<Rigidbody2D>();
    }

    //Update function to handle paddle movement
    void FixedUpdate() {

        //if (IsOwner)
        {
        //Set movement inputs and speed
        float input = GetMovementInput();
        float currentSpeed = speed;

        //Set paddle velocity based on speed * user input
        paddle.velocity = new Vector2(0, input * currentSpeed);
        }
    }

    //Get input function
    protected abstract float GetMovementInput();
}
