using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManagersLoader : MonoBehaviour
{
    void Start()
    {
        // lazily load Global Managers scene, so they are only loaded once in every scene setup
        if (!SceneManager.GetSceneByBuildIndex(Scenes._InGameManagers).isLoaded)
        {
            SceneManager.LoadScene(Scenes._InGameManagers, LoadSceneMode.Additive);
        }
    }
}
