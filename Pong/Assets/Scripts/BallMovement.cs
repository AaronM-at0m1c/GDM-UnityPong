using UnityEngine;

public class BallMovement : MonoBehaviour {

    Rigidbody2D rb;

void Start()
{
    rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(3f, 3f);
}



void update()
{
   void OnCollisionEnter2D(Collision2D collision)
{
  Rigidbody2D rb = GetComponent<Rigidbody2D>();
    
  // Reverse horizontal direction
  rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
}
}

}

