using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] AudioClip reviveSound;
    private AudioSource audioSource;
    private bool reviveAvailable = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        reviveAvailable = true;
        audioSource.PlayOneShot(reviveSound, 0.6f);
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