using UnityEngine;

public class LeftPaddleController : PaddleController, ICollidable
{
    protected override float GetMovementInput() {
        return Input.GetAxis("LeftPaddle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnHit(collision);
        }
    }    
    
    //Implement OnHit Interface
    public void OnHit(Collision2D collision)
    {
        Debug.Log("Left paddle was hit.");
    }
}
