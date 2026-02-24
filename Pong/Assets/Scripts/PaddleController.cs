using UnityEngine;
using Unity.Netcode;

public abstract class PaddleController : NetworkBehaviour
{
    //Protected variables for paddle speed and rigidbody
    protected float speed = 8.0f;
    protected Rigidbody2D paddle;
    private NetworkVariable<float> networkPositionY = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    //Initialize the rb component for the paddle
    void Start() {
        paddle = GetComponent<Rigidbody2D>();
        //paddle.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnNetworkSpawn()
    {
        if (OwnerClientId == 0)
        {
            transform.position = new Vector3(-6f, 0f, 0f);
            gameObject.name = "LPaddle";
            Debug.Log("Host paddle spawned - use W/S");
        } else
        {
            transform.position = new Vector3(15f, 0f, 0f);
            gameObject.name = "RPaddle";
            Debug.Log("Client paddle spawned - use up/down");
        }
        networkPositionY.Value = transform.position.y;
    }

    //Update function to handle paddle movement
    void FixedUpdate() {

        if (IsOwner)
        {
        //Set movement input
        float input = GetMovementInput();
        float newY = transform.position.y + (input * speed * Time.deltaTime);

        // Update local position
        paddle.MovePosition(new Vector2(transform.position.x, newY));

        //Update networkvariable so other clients can see
        networkPositionY.Value = newY;
        } else
        {
            paddle.MovePosition(new Vector2(transform.position.x, networkPositionY.Value));
        }
    }

    //Get input function
    protected abstract float GetMovementInput();
}
