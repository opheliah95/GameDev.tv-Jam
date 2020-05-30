using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManagersLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(Scenes._GlobalManagers, LoadSceneMode.Additive);
    }
}
