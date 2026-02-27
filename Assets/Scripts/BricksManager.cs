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

    [SerializeField] AudioClip explosionSound;
    private AudioSource audioSource;

    public BrickPool brickPool;
    public int rows = 5; // Nb de rangées
    public int columns = 6; // Nb de colonnes
    public float spacing = 1f; 



    public Vector2 startPosition = new Vector2(-15f, -2f);


    private GameObject[,] bricks;
    private int remainingBricks = 0;

    private bool isPaused = false;
    public bool isExploding = false;



    private enum MoveState { MoveRight, MoveLeft }
    private MoveState currentState = MoveState.MoveRight;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerBoundaryX = player.GetComponent<PlayerBehaviour>().boundary;
        bricks = new GameObject[rows, columns];

        SpawnBricks();


    }
    private void FixedUpdate()
    {
        if (remainingBricks<= 0)
        {
            GameManager.Instance.CompletedLevel();
        }

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
            

            for (int col = 0; col < columns; col++)
            {
                var bricksType = GetRandomBrickType(bricksTypes);
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
                        bricksScript.Init(this, bricksType.prefab);
                    }
                    bricks[row, col] = brick;
                    remainingBricks++;
                }
            }

        }

        FindFirstObjectByType<SpecialPowerManager>().ChooseRandomPower();
    }

    public void ClearBricks()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (bricks[row, col] != null)
                {
                    brickPool.ReturnToPool(bricks[row, col],
                        bricks[row, col].GetComponent<BricksScript>().BrickType.prefab);

                    bricks[row, col] = null;
                }
            }
        }

        remainingBricks = 0;
    }



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
        audioSource.PlayOneShot(explosionSound, 0.2f);
        GameManager.Instance.AddScore(enemy.GetComponent<BricksScript>().ScoreData);

        brickPool.ReturnToPool(enemy, prefab);

        remainingBricks--;


        if (remainingBricks <= 0)
        {
            GameManager.Instance.CompletedLevel();
        }


    }

    

    private BricksData.BrickType GetRandomBrickType(List<BricksData.BrickType> brickTypes)
    {
        int rand = Random.Range(0, brickTypes.Count);
        return brickTypes[rand];
    }
    public List<GameObject> GetActiveBricks()
    {
        List<GameObject> activeBricks = new List<GameObject>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (bricks[row, col] != null && bricks[row, col].activeSelf)
                {
                    activeBricks.Add(bricks[row, col]);
                }
            }
        }

        return activeBricks;
    }
}
