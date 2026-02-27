using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controls;
    [SerializeField] GameObject powers;


    private int inputCount = 0;


    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            inputCount++;

        }
        if (inputCount == 1)
        {
            controls.SetActive(true);
        }
        if (inputCount == 2)
        {
            controls.SetActive(false);
            powers.SetActive(true);
        }
        if (inputCount > 2)
        {
            LoadGame(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Application.Quit();
        }
    }

    public void LoadGame(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }





}
