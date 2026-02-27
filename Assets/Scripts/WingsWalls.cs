using System.Collections;
using UnityEngine;

public class WingsWalls : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private float duration = 8f;

    [SerializeField] AudioClip activationSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        audioSource.PlayOneShot(activationSound, 0.4f);
        StartCoroutine(WallsCoroutine());
    }

    private IEnumerator WallsCoroutine()
    {
        leftWall.SetActive(true);
        rightWall.SetActive(true);

        yield return new WaitForSeconds(duration);

        leftWall.SetActive(false);
        rightWall.SetActive(false);
    }
}