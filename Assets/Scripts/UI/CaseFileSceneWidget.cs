using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityConstants;

public class CaseFileSceneWidget : MonoBehaviour
{
    /* Child references */
    
    [Tooltip("Readable Scene Text")]
    public TextMeshProUGUI sceneNameText;
    
    /* Init parameters */
    
    private ScenesEnum m_SceneEnum;
    
    public void Init(CaseData.SceneData sceneData)
    {
        m_SceneEnum = sceneData.sceneEnum;
        sceneNameText.text = sceneData.name;
    }

    /// Button event callback: load target scene
    public void LoadScene()
    {
        SceneManager.LoadScene((int) m_SceneEnum);
    }
}
