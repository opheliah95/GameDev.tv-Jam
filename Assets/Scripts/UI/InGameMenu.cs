using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityConstants;

public class InGameMenu : MonoBehaviour
{
    /* Child references */

    [Tooltip("Canvas root")]
    public Canvas canvas;
    
    [Tooltip("Case File root")]
    public CaseFile caseFile;
    
    /* State */

    private bool m_Open = false;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }
    
    public void Toggle()
    {
        if (m_Open)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    
    public void Open()
    {
        m_Open = true;
        canvas.gameObject.SetActive(true);

        // for now, always open on Case File
        if (!caseFile.IsOpen)
        {
            caseFile.Open();
        }
    }

    public void Close()
    {
        m_Open = false;
        canvas.gameObject.SetActive(false);
    }
}
