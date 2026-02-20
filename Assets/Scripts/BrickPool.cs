using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrickPool : MonoBehaviour
{
    [SerializeField]
    public BricksData bricksData;


    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();




    void Start()
    {
        // Initialiser un pool pour chaque type d'ennemi avec des quantités spécifiques
        for (int i = 0; i < bricksData.bricksTypes.Count; i++)
        {
            var bricksType = bricksData.bricksTypes[i];


            if (poolDictionary.ContainsKey(bricksType.prefab))
            {
                Debug.LogWarning($"Le prefab {bricksType.prefab.name} pour le type {bricksType.name} est déjà dans le pool");
                continue;
            }

            int poolSize = GetPoolSizeForBrickType(i);

            Queue<GameObject> brickQueue = new Queue<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject brick = Instantiate(bricksType.prefab);
                brick.transform.name = bricksType.prefab.name;
                brick.SetActive(false);
                brickQueue.Enqueue(brick);
            }

            poolDictionary.Add(bricksType.prefab, brickQueue);
        }
    }

    public GameObject GetBrick(GameObject prefab)
    {
        if (poolDictionary.TryGetValue(prefab, out Queue<GameObject> brickQueue) && brickQueue.Count > 0)
        {
            GameObject brick = brickQueue.Dequeue();
            brick.SetActive(true);

            return brick;
        }

        return null;
    }

    public void ReturnToPool(GameObject brick, GameObject prefab)
    {
        // TODO : A debugguer
        brick.SetActive(false);

        if (poolDictionary.TryGetValue(prefab, out var brickQueue))
        {
            brickQueue.Enqueue(brick);
        }
        else
        {
            Debug.Log("Tentative de retourner un ennemi à un pool inexistant !");
        }
    }

    public List<BricksData.BrickType> GetBrickType()
    {
        return bricksData.bricksTypes;
    }

    private int GetPoolSizeForBrickType(int index)
    {
        switch (index)
        {
            case 0: // Type A
                return 22;
            case 1: // Type B
                return UnityEngine.Random.Range(3,22);
            case 2: // Type C
                return UnityEngine.Random.Range(3, 5);
            default:
                return 0;
        }
    }

    void Update()
    {

    }
}
