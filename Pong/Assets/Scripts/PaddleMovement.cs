using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        float vertical  = Input.GetAxis("Vertical");

        transform.position += new Vector3(0, vertical * speed * Time.deltaTime, 0);
    }
}