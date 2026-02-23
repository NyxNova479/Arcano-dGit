using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField] Transform playerTransform;

    private float playerBoundaryX;
    private float ballSpeed = 5f;
    private float delta = 0;

    public bool isLaunched = false;
    bool boundaryReached = false;

    public Vector3 direction = new Vector3(0,1,0);
    public Vector2 normal;



    private void Update()
    {
        if (!isLaunched)
        {
            transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + 0.5f);
        }
        else
        {
            delta = Time.deltaTime;
            transform.position += direction * ballSpeed * delta; 
        }

        if (ReachedBoundary(gameObject)) boundaryReached = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (boundaryReached)
        {
            normal = (transform.position - collision.transform.position).normalized;
        }
        normal = (transform.position - collision.transform.position).normalized;
        direction = Reflect(direction.normalized, normal);
        Debug.Log(direction);
    }




    private Vector2 Reflect(Vector2 d, Vector2 n)
    {
        return d - 2 * Vector2.Dot(n, d) * n;
    }

    private bool ReachedBoundary(GameObject enemy)
    {
        float xPos = enemy.transform.position.x;
    
        if (xPos >= playerBoundaryX)
        {
    
            return true;
        }
        if (xPos <= -playerBoundaryX)
        {
    
            return true;
        }
    
        return false;
    
    }
}
