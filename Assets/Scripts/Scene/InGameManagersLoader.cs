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
        // !SceneManager.GetSceneByBuildIndex(Scenes._InGameManagers).IsValid() is not reliable
        // when adding sub-scenes in editor, so check for presence of representative singleton instead
        if (InGameManager.Instance == null)
        {
            SceneManager.LoadScene(Scenes._InGameManagers, LoadSceneMode.Additive);
        }
    }
}
