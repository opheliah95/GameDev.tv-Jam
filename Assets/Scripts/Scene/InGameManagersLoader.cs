using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManagersLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(Scenes._InGameManagers, LoadSceneMode.Additive);
    }
}
