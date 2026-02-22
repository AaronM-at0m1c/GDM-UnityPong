using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 5.0f;
    void Start()
    {
        
    }

    void Update()
    {
        float vertical  = Input.GetAxis("Vertical");
        Vector2 currentPosition = transform.position;
        currentPosition.y += vertical * speed * Time.deltaTime;
        currentPosition.y = Mathf.Clamp(currentPosition.y, -4f, 4f);
        transform.position = currentPosition;
    }
}