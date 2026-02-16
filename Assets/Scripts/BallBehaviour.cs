using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private Vector2 Reflect(Vector2 d, Vector2 n)
    {
        return d - 2 * Vector2.Dot(n, d) * n;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Reflect(gameObject.transform.position, collision.gameObject.transform.position);
    }
}
