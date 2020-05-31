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
        // !SceneManager.GetSceneByBuildIndex(Scenes._GlobalManagers).IsValid() is not reliable
        // when adding sub-scenes in editor, so check for presence of representative singleton instead
        if (GlobalManager.Instance == null)
        {
            SceneManager.LoadScene(Scenes._GlobalManagers, LoadSceneMode.Additive);
        }
    }
}
