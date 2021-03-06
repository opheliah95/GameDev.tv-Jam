﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityConstants;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.Instance.UnloadActiveSceneAndLoadLevelAsNewActiveSceneAsync(Scenes.Level0_Forest);
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
