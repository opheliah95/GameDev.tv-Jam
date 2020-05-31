using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using CommonsHelper;
using UnityConstants;

public class CaseFileSceneWidget : MonoBehaviour
{
    /* Sibling components */

    private Button button;
    
    /* Child references */
    
    [Tooltip("Readable Scene Text")]
    public TextMeshProUGUI sceneNameText;
    
    /* Init parameters */
    
    private ScenesEnum m_SceneEnum;

    private void Awake()
    {
        button = this.GetComponentOrFail<Button>();
    }

    public void Init(SceneData sceneData, bool isSceneUnlocked)
    {
        m_SceneEnum = sceneData.sceneEnum;
        sceneNameText.text = sceneData.sceneName;
        button.interactable = isSceneUnlocked;
    }

    /// Button event callback: load target scene
    public void LoadScene()
    {
        SceneManager.LoadScene((int) m_SceneEnum);
    }
}
