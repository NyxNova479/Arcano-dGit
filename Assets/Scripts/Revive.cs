using UnityEngine;

public class Revive : MonoBehaviour
{
    private bool reviveAvailable = false;

    public void Activate()
    {
        reviveAvailable = true;
        Debug.Log("Revive prêt !");
    }

    public bool TryRevive(BallBehaviour ball)
    {
        if (!reviveAvailable)
            return false;

        reviveAvailable = false;

        ball.ResetBall();

        return true;
    }
}