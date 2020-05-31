using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManagersLoader : MonoBehaviour
{
    void Start()
    {
        // lazily load Global Managers scene, so they are only loaded once in every scene setup
        if (!SceneManager.GetSceneByBuildIndex(Scenes._GlobalManagers).IsValid())
        {
            SceneManager.LoadScene(Scenes._GlobalManagers, LoadSceneMode.Additive);
        }
    }
}
