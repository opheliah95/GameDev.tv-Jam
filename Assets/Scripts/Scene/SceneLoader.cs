using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CommonsPattern;
using UnityConstants;

public class SceneLoader : SingletonManager<SceneLoader>
{
    public void UnloadActiveSceneAndLoadLevelAsNewActiveSceneAsync(int sceneIndex)
    {
        StartCoroutine(UnloadActiveSceneAndLoadLevelAsNewActiveSceneCoroutine(sceneIndex));
    }
    
    private IEnumerator UnloadActiveSceneAndLoadLevelAsNewActiveSceneCoroutine(int sceneIndex)
    {
        // active scene should main menu at this point
        yield return UnloadActiveSceneCoroutine();
        
        // if already in a level, notify PlayerManager we are changing level, and player will need respawn
        // (would be cleaner with an event to avoid direct call and null check, but works)
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnChangeLevel();
        }
        
        // close in-game menu if it was open to show Scenes page (it's not even there if still in main menu)
        // again, cleaner with event, but OK
        if (InGameMenu.Instance && InGameMenu.Instance.IsOpen())
        {
            InGameMenu.Instance.Close();
        }
        
        // once main menu has been unloaded, load level
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        
        // Unity needs a complete frame to actually load the scene above so we can activate it
        // so we wait for the end of this frame, then the next frame
        // Thanks for the thread below, although it didn't mention I needed a second wait:
        // https://forum.unity.com/threads/scenemanager-loadscene-additive-and-set-active.380826/
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        
        // spawn or respawn character (needs an extra frame on first level because PlayerManager has not
        // registered singleton instance yet, and since it's not a GlobalManager it didn't exist in the Main Menu)
        // in practice, PlayerManager.Start will be faster than this call on first load, but this call is needed
        // for respawn
        yield return new WaitForEndOfFrame();
        PlayerManager.Instance.SpawnPlayerCharacterIfNotAlready();
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
