using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CommonsPattern;
using UnityConstants;

public class SceneLoader : SingletonManager<SceneLoader>
{
    public void UnloadMainMenuAndLoadLevelAsync()
    {
        StartCoroutine(UnloadMainMenuAndLoadLevelCoroutine(Scenes.Level0_Forest));
    }
    
    private IEnumerator UnloadMainMenuAndLoadLevelCoroutine(int sceneIndex)
    {
        // active scene should main menu at this point
        yield return UnloadActiveSceneCoroutine();
        // once main menu has been unloaded, load level
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }
    
    private IEnumerator UnloadActiveSceneCoroutine()
    {
        AsyncOperation unloadSceneOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
#if UNITY_EDITOR
        string unloadedSceneName = SceneManager.GetActiveScene().name;
#endif

        while (!unloadSceneOperation.isDone)
        {
#if UNITY_EDITOR
            Debug.LogFormat("[Scene Loader] Unloading {0} progress: {1}%", unloadedSceneName, unloadSceneOperation.progress * 100);
#endif
            yield return null;
        }
#if UNITY_EDITOR
        Debug.LogFormat("[Scene Loader] Finished unloading {0}", unloadedSceneName);
#endif
        
        yield return unloadSceneOperation;
    }
}
