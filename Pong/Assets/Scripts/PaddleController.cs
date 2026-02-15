using UnityEngine;
using Unity.Netcode;

public abstract class PaddleController : NetworkBehaviour
{
    //Protected variables for paddle speed and rigidbody
    protected float speed = 8.0f;
    private NetworkVariable<float> syncedYPosition = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    protected Rigidbody2D paddle;

    //Initialize the rb component for the paddle
    void Start() {
        paddle = GetComponent<Rigidbody2D>();
    }

    //Update function to handle paddle movement
    void Update() {

        if (IsOwner)
        {
        //Set movement input
        float input = GetMovementInput();
        float newY = transform.position.y + (input * speed * Time.deltaTime);

        // Update local position
        paddle.MovePosition(new Vector2(transform.position.x, newY));

        //Update networkvariable so other clients can see
        syncedYPosition.Value = newY;
        } else
        {
            paddle.MovePosition(new Vector2(transform.position.x, syncedYPosition.Value));
        }
    }

    //Get input function
    protected abstract float GetMovementInput();
}
