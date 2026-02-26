using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;



    [Header("Settings")]
    [SerializeField] private float ballSpeed = 6f;
    [SerializeField] private float ballRadius = 0.25f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] GameObject sparkPrefab;

    public bool isLaunched = false;

    private Vector2 direction = Vector2.up;

    private const float SKIN = 0.01f;      // marge anti-blocage
    private const float MIN_Y = 0.2f;      // empêche angles plats

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0.9f, 1f);
    }

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
        if (gameObject.transform.position.y <= -7)
        {
            GameManager.Instance.LoseLife();
        }
        MoveBall();
    }

    private void MoveBall()
    {
        float remainingDistance = ballSpeed * Time.fixedDeltaTime;
        int safetyCounter = 0;

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
                Instantiate(sparkPrefab, hit.point, Quaternion.identity);
                // Avance jusqu’au point de contact (avec skin)
                float moveDist = Mathf.Max(hit.distance - SKIN, 0f);
                transform.position += (Vector3)(direction * moveDist);
                remainingDistance -= moveDist;

                // Paddle → angle contrôlé
                if (hit.collider.CompareTag("Player"))
                {
                    HandlePaddleBounce(hit);
                }
                else if (hit.collider.CompareTag("Brick"))
                {
                    BricksScript brick = hit.collider.GetComponent<BricksScript>();
                    if (brick != null)
                        brick.OnHit();
                    direction = Vector2.Reflect(direction, hit.normal).normalized;
                }
                else
                {
                    direction = Vector2.Reflect(direction, hit.normal).normalized;
                }

                // Évite les trajectoires trop horizontales
                if (Mathf.Abs(direction.y) < MIN_Y)
                {
                    direction.y = Mathf.Sign(direction.y == 0 ? 1 : direction.y) * MIN_Y;
                    direction.Normalize();
                }

                // Sort proprement de la surface touchée
                transform.position += (Vector3)(direction * SKIN);
            }
            else
            {
                transform.position += (Vector3)(direction * remainingDistance);
                remainingDistance = 0f;
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
            direction = new Vector2(
                Random.Range(-0.5f, 0.5f),
                1f
            ).normalized;
        }
    }
}