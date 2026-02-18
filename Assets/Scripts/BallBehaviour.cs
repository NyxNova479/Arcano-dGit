using UnityEngine;
using UnityEngine.UIElements;

public class BallBehaviour : MonoBehaviour
{
    public int bounces = 5;


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

    private Vector2 Reflect(Vector2 d, Vector2 n)
    {
        return d - 2 * Vector2.Dot(n, d) * n;
    }
}
