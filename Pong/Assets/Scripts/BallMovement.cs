using UnityEngine;

public class BallMovement : MonoBehaviour, ICollidable
{

//Private Fields
private float speed = 3f;
private Vector2 direction;
private Rigidbody2D ball;

//Public Get-Set Properties
public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }


public Vector2 Direction
    {
        get { return direction; }
        set { direction =value.normalized; }
    }

//Start function
void Start()
{
    //Create ball object and set starting velocity
    ball = GetComponent<Rigidbody2D>();
    direction = new Vector2(1, 1);
    ball.velocity = direction * speed;
}

//Collision Logic
void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(collision);
        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnHit(collision);
        }
    }

public void OnHit(Collision2D collision)
{ 
  // Reverse horizontal direction for top and bottom walls
  if (collision.gameObject.name == "Top Wall" || collision.gameObject.name == "Bottom Wall")
        {
            direction.y = -direction.y;
        }
    //Reverse vertical direction for left and right walls
    else if (collision.gameObject.name == "Left Wall" || collision.gameObject.name == "Right Wall")
        {
            direction.x = -direction.x;
        }
    //Reverse vertical direction for paddle collisions
    else if (collision.gameObject.name == "Left Paddle" || collision.gameObject.name == "Right Paddle")
        {
            direction.x = -direction.x;
            speed += 1; //Add little speed boost to the ball when it its a paddle
        }
  //Debug.Log("Hit: " + collision.gameObject.name); //Sanity check for debugging bounce issue
}

//Update function to continuously update ball velocity
void FixedUpdate()
    {
        ball.velocity = direction * speed;
    }
}