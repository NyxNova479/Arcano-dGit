using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrickPool : MonoBehaviour
{
    [SerializeField]
    public BricksData bricksData;


    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        InitializePool();
    }


    

    public GameObject GetBrick(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError($"Prefab {prefab.name} non trouvé dans le pool");
            return null;
        }

        if (poolDictionary[prefab].Count == 0)
        {
            Debug.LogWarning($"Pool vide pour {prefab.name}");
            return null;
        }

        GameObject brick = poolDictionary[prefab].Dequeue();
        brick.SetActive(true);
        return brick;
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
                return 30;
            case 1: // Type B
                return 30;
            case 2: // Type C
                return 30;
            case 3: // Type D
                return 30;
            default:
                return 0;
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < bricksData.bricksTypes.Count; i++)
        {
            var bricksType = bricksData.bricksTypes[i];

            if (poolDictionary.ContainsKey(bricksType.prefab))
                continue;

            int poolSize = GetPoolSizeForBrickType(i);
            Queue<GameObject> brickQueue = new Queue<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject brick = Instantiate(bricksType.prefab, transform);
                brick.name = bricksType.prefab.name;
                brick.SetActive(false);

                // Position neutre hors écran (sécurité)
                brick.transform.position = Vector3.one * 9999f;

                brickQueue.Enqueue(brick);
            }

            poolDictionary.Add(bricksType.prefab, brickQueue);
        }
    }
}
