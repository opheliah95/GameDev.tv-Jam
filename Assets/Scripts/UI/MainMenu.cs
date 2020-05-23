using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(Scenes.TestScene_Long);
    }

    public void ShowOptions()
    {
        Debug.Log("Show options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
