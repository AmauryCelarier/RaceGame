using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayInfini()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PlayChrono()
    {
        SceneManager.LoadScene("GameSceneChrono");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}