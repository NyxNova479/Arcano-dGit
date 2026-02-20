using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BricksData;

public class BricksManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private float playerBoundaryX;

    [SerializeField] BricksData brickData;

    //[SerializeField] AudioClip ufoSound;
    //[SerializeField] AudioClip explosionSound;
    //private AudioSource audioSource;

    public BrickPool brickPool;
    public int rows = 5; // Nb de rangées
    public int columns = 6; // Nb de colonnes
    public float spacing = 1f; 



    public Vector2 startPosition = new Vector2(-15f, -2f);


    private GameObject[,] bricks;
    private int remainingBricks = 0;

    private bool isPaused = false;
    public bool isExploding = false;

    //[SerializeField]
    //private MisssileManager missileManager;
    //public int shootLimit = 22;

    private enum MoveState { MoveRight, MoveLeft }
    private MoveState currentState = MoveState.MoveRight;

    //public GameObject[] missilePrefabs;
    //public Transform missilePoint;
    //public float missileInterval = 2.0f; // Intervalle minimum entre les tirs
    //public int missileType = 0;

    //private int explosionDuration = 17;
    //[SerializeField]
    //private GameObject explosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        playerBoundaryX = player.GetComponent<PlayerBehaviour>().boundary;
        bricks = new GameObject[rows, columns];

        SpawnBricks();


    }
    private void Update()
    {


    }



    public void SpawnBricks()
    {
        var bricksTypes = brickPool.GetBrickType();

        float totalWidth = (columns - 1) * spacing;
        float totalHeight = (rows - 1) * 0.8f;

        Vector2 centeredStartPosition = new Vector2(
            -totalWidth / 2f,
             totalHeight );

        for (int row = 0; row < rows; row++)
        {
            var bricksType = GetRandomBrickType(bricksTypes);

            for (int col = 0; col < columns; col++)
            {
                GameObject brick = brickPool.GetBrick(bricksType.prefab);

                if (brick != null)
                {
                    float xPos = centeredStartPosition.x + (col * spacing);
                    float yPos = centeredStartPosition.y - (row * 0.4f) ;

                  

                    brick.transform.position = new Vector3(xPos, yPos, 0);

                    BricksScript bricksScript = brick.GetComponent<BricksScript>();
                    if (bricksScript != null)
                    {
                        bricksScript.BrickType = bricksType;
                        bricksScript.ScoreData = bricksType.points;
                    }
                    bricks[row, col] = brick;
                    remainingBricks++;
                }
            }

        }


    }

    //IEnumerator HandheldEnemyMovement()
    //{
    //    while (remainingBricks > 0)
    //    {
    //
    //
    //        bool boundaryReached = false;
    //
    //        // Boucle pour déplacer chaque ennemie l'un après l'autre en commençant par la dernière ligne
    //        for (int row = rows - 1; row >= 0; row--)
    //        {
    //
    //
    //            for (int col = 0; col < columns; col++)
    //            {
    //                if (GameManager.Instance.IsPaused || isExploding)
    //                {
    //                    yield return new WaitUntil(() => !GameManager.Instance.IsPaused && !isExploding);
    //                }
    //
    //                if (bricks[row, col] != null && bricks[row, col].activeSelf)
    //                {
    //                    // Déplacer l'ennemi dans la currentDirection
    //
    //                    Vector3 direction = currentState == MoveState.MoveRight ? Vector3.right : Vector3.left; ;
    //
    //                    MoveEnemy(bricks[row, col], direction, _stepDistance);
    //
    //                    if (bricks[row, col] == null) continue;
    //
    //                    // Alterne le sprite de l'ennemi
    //                    EnemyScript enemyScript = enemies[row, col].GetComponent<EnemyScript>();
    //                    if (enemyScript != null) enemyScript.ChangeSprite();
    //
    //                    if (ReachedBoundary(enemies[row, col])) boundaryReached = true;
    //
    //                    yield return null;
    //
    //                }
    //
    //            }
    //
    //
    //        }
    //        if (missileManager.shootCount >= shootLimit)
    //        {
    //            Debug.Log("caca");
    //            missileManager.shootCount = 0;
    //            SpawnUFO();
    //        }
    //
    //
    //        if (boundaryReached)
    //        {
    //            yield return MoveAllEnemiesDown();
    //            currentState = currentState == MoveState.MoveRight ? MoveState.MoveLeft : MoveState.MoveRight;
    //        }
    //    }
    //}
    //
    //
    //IEnumerator EnemyShooting()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => !GameManager.Instance.IsPaused && !isExploding);
    //
    //        yield return new WaitForSeconds(Random.Range(missileInterval, missileInterval * 2));
    //
    //        List<GameObject> shooters = GetBottomEnemies();
    //
    //        if (shooters.Count > 0 && !GameManager.Instance.IsPaused && !isExploding && enemyData.enemyTypes[3].prefab)
    //        {
    //            GameObject shooter = shooters[Random.Range(0, shooters.Count)];
    //
    //            FireMissile(shooter, missileType);
    //            missileType = 1 + missileType % missilePrefabs.Length;
    //
    //        }
    //    }
    //}
    //
    private List<GameObject> GetBottomBricks()
    {
        List<GameObject> bottomBricks = new List<GameObject>();
    
        for (int col = 0; col < columns; col++)
        {
            for (int row = rows - 1; row >= 0; row--)
            {
                if (bricks[row, col] != null && bricks[row, col].activeSelf)
                {
                    bottomBricks.Add(bricks[row, col]);
                    break;
                }
            }
        }
    
    
    
        return bottomBricks;
    }
    //
    //private void FireMissile(GameObject shooter, int missileType)
    //{
    //    // Rechercher le FirePoint dans les enfants de l'ennemi
    //    Transform firePoint = shooter.transform.Find("FirePoint");
    //
    //    if (firePoint != null)
    //    {
    //        if (missileType == 0)
    //        {
    //            Instantiate(missilePrefabs[0], firePoint.position, Quaternion.identity);
    //        }
    //        if (missileType == 1)
    //        {
    //            Instantiate(missilePrefabs[1], firePoint.position, Quaternion.identity);
    //        }
    //        if (missileType == 2)
    //        {
    //            Instantiate(missilePrefabs[2], firePoint.position, Quaternion.identity);
    //            missileType = 0;
    //        }
    //
    //
    //    }
    //    else
    //    {
    //        Debug.Log($"FirePoint non trouvé pour l'ennemi : {shooter.name}");
    //    }
    //}


    public void ReturnEnemy(GameObject enemy, GameObject prefab)
    {
        for (int row = 0; row < rows; row++)
        {

            for (int col = 0; col < columns; col++)
            {

                if (bricks[row, col] == enemy)
                {
                    bricks[row, col] = null;
                }

            }


        }
        //audioSource.PlayOneShot(explosionSound, 0.7f);
        GameManager.Instance.AddScore(enemy.GetComponent<BricksScript>().ScoreData);

        brickPool.ReturnToPool(enemy, prefab);

        remainingBricks--;

        if (isExploding != true)
        {
            //StartCoroutine(ExplosionCoroutine(enemy));
        }

        if (remainingBricks <= 0)
        {
            GameManager.Instance.CompletedLevel();
        }


    }

    //IEnumerator ExplosionCoroutine(GameObject enemy)
    //{
    //    isExploding = true;
    //    int duration = explosionDuration;
    //
    //    GameObject explosion = Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
    //    while (duration > 0)
    //    {
    //
    //        duration--;
    //        yield return new WaitForEndOfFrame();
    //    }
    //
    //    explosion.SetActive(false);
    //
    //    isExploding = false;
    //}

    //private void MoveEnemy(GameObject enemy, Vector3 direction, float stepDistance)
    //{
    //
    //    if (enemy == null) return;
    //
    //    Vector3 newPosition = enemy.transform.position + direction * stepDistance;
    //
    //    newPosition.x = Mathf.Round(newPosition.x * 100f) / 100f;
    //    newPosition.y = Mathf.Round(newPosition.y * 100f) / 100f;
    //    newPosition.z = Mathf.Round(newPosition.z * 100f) / 100f;
    //
    //    enemy.transform.position = newPosition;
    //}
    //
    //private bool ReachedBoundary(GameObject enemy)
    //{
    //    float xPos = enemy.transform.position.x;
    //
    //    if (currentState == MoveState.MoveRight && xPos >= playerBoundaryX)
    //    {
    //
    //        return true;
    //    }
    //    if (currentState == MoveState.MoveLeft && xPos <= -playerBoundaryX)
    //    {
    //
    //        return true;
    //    }
    //
    //    return false;
    //
    //}
    

    private BricksData.BrickType GetRandomBrickType(List<BricksData.BrickType> brickTypes)
    {
        int rand = Random.Range(0, brickTypes.Count-1);
        return brickTypes[rand];
    }
}
