using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField] private float ballSpeed = 6f;
    [SerializeField] private float ballRadius = 0.25f;
    [SerializeField] private LayerMask collisionMask;

    public bool isLaunched = false;

    private Vector2 direction = Vector2.up;

    void FixedUpdate()
    {
        if (!isLaunched)
        {
            transform.position = new Vector2(
                playerTransform.position.x,
                playerTransform.position.y + 0.5f
            );
            return;
        }

        MoveBall();
    }

    private void MoveBall()
    {
        float remainingDistance = ballSpeed * Time.fixedDeltaTime;

        int safetyCounter = 0; // évite boucle infinie

        while (remainingDistance > 0.0001f && safetyCounter < 3)
        {
            safetyCounter++;

            RaycastHit2D hit = Physics2D.CircleCast(
                transform.position,
                ballRadius,
                direction,
                remainingDistance,
                collisionMask
            );

            if (hit.collider != null)
            {
                // On avance jusqu’au point d’impact exact
                transform.position += (Vector3)(direction * hit.distance);

                remainingDistance -= hit.distance;

                if (hit.collider.CompareTag("Player"))
                {
                    HandlePaddleBounce(hit);
                }
                else
                {
                    direction = Vector2.Reflect(direction, hit.normal);
                }
                if (Mathf.Abs(direction.y) < 0.2f)
                {
                    direction.y = Mathf.Sign(direction.y) * 0.2f;
                    direction.Normalize();
                }

                

                // Petit offset pour sortir proprement de la surface
                transform.position += (Vector3)(direction * 0.001f);
            }
            else
            {
                transform.position += (Vector3)(direction * remainingDistance);
                remainingDistance = 0;
            }
        }
    }

    private void HandlePaddleBounce(RaycastHit2D hit)
    {
        float paddleWidth = hit.collider.bounds.size.x;
        float hitOffset = hit.point.x - hit.collider.bounds.center.x;

        float normalizedOffset = hitOffset / (paddleWidth / 2f);

        direction = new Vector2(normalizedOffset, 1f).normalized;
    }

    public void Launch()
    {
        if (!isLaunched)
        {
            isLaunched = true;
            direction = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        }
    }
}