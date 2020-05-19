using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    /* State */

    private bool m_Open;

    private void Start()
    {
        gameObject.SetActive(false);
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
        
        gameObject.SetActive(true);
    }

    public void Close()
    {
        m_Open = false;

        gameObject.SetActive(false);
    }
}
