using UnityEngine;
using UnityEngine.UIElements;

public class BallBehaviour : MonoBehaviour
{

    [SerializeField] Transform playerTransform;

    public bool isLaunched = false;

    public int bounces = 5;
    private float ballSpeed = 0.05f;
    private float delta = 0;

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 dir = transform.right;
        Ray ray = new Ray(origin, dir);

        Gizmos.DrawLine(origin, origin + dir);

        for (int i = 0; i < bounces; i++)
        {


            if ((Physics.Raycast(ray, out RaycastHit hit)))
            {
                Gizmos.DrawSphere(hit.point, 0.1f);
                Vector2 reflected = Reflect(ray.direction, hit.normal);
                Gizmos.color = Color.Lerp(Color.white, Color.red, i / bounces);
                Gizmos.DrawLine(hit.point, (Vector2)hit.point + reflected);
                ray.direction = reflected;
                ray.origin = hit.point;


            }
            else break;
        }

    }



    private void Update()
    {
        if (!isLaunched)
        {
            transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y + 0.05f);
        }
        else
        {
            delta += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, transform.position.y + ballSpeed * delta); 
        }
        

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 reflected= Reflect(transform.position, collision.gameObject.transform.position);
        transform.position = reflected;
    }

    private Vector2 Reflect(Vector2 d, Vector2 n)
    {
        return d - 2 * Vector2.Dot(n, d) * n;
    }
}
