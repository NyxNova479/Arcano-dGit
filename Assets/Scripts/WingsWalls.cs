using System.Collections;
using UnityEngine;

public class WingsWalls : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private float duration = 8f;

    public void Activate()
    {
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