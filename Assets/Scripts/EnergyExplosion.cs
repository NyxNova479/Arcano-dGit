using UnityEngine;
using System.Collections.Generic;

public class EnergyExplosion : MonoBehaviour
{
    [SerializeField] private float smallThreshold = 5f;
    [SerializeField] private float mediumThreshold = 10f;

    [SerializeField] private BallBehaviour ball;
    [SerializeField] private BricksManager bricksManager;

    public void Activate()
    {
        float energy = ball.energy;

        if (energy < smallThreshold)
            DestroyRandomBricks(3);
        else if (energy < mediumThreshold)
            DestroyRandomBricks(6);
        else
            DestroyRandomBricks(12);

        ball.energy = 0f;
    }

    private void DestroyRandomBricks(int amount)
    {
        List<GameObject> bricks = bricksManager.GetActiveBricks();

        for (int i = 0; i < amount && bricks.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, bricks.Count);
            GameObject brick = bricks[randomIndex];

            brick.GetComponent<BricksScript>().OnHit();

            bricks.RemoveAt(randomIndex);
        }
    }
}