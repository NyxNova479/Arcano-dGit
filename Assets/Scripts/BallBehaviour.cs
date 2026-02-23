using UnityEngine;
using UnityEngine.UIElements;

public class BallBehaviour : MonoBehaviour
{

    [SerializeField] Transform playerTransform;

    public bool isLaunched = false;

    public int bounces = 5;
    private float ballSpeed = 5f;
    private float delta = 0;

    public Vector2 direction = Vector2.one;
 



    private void Update()
    {
        if (!isLaunched)
        {
            transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + 0.05f);
        }
        else
        {
            delta = Time.deltaTime;
            transform.position += (Vector3)direction * ballSpeed * delta; 
        }
        

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Reflect(direction.normalized, collision.contacts[0].normal);
        Debug.Log(direction);
    }

    private Vector2 Reflect(Vector2 d, Vector2 n)
    {
        return d - 2 * Vector2.Dot(n, d) * n;
    }
}
