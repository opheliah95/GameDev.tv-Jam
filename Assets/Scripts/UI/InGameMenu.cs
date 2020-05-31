using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;
using CommonsPattern;

public class InGameMenu : SingletonManager<InGameMenu>
{
    /* Events */
    
    public static event Action menuOpened;
    public static event Action menuClosed;

    
    /* Sibling components */
    private Canvas canvas;
    
    
    /* Child references */
    
    [Tooltip("Case File root")]
    public CaseFile caseFile;
    
    /* State */

    /// Is the in-game menu open?
    private bool m_Open = false;
    public bool IsOpen() => m_Open;

    protected override void Init()
    {
        canvas = this.GetComponentOrFail<Canvas>();
    }

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
        
        // refresh both sides in case content changed since last time it was opened (or since Start showing Brief page),
        // whether opened or not
        caseFile.RefreshBothSides();

        OnMenuOpened();
    }

    public void Close()
    {
        m_Open = false;
        canvas.gameObject.SetActive(false);

        OnMenuClosed();
    }

    private static void OnMenuOpened()
    {
        menuOpened?.Invoke();
    }

    private static void OnMenuClosed()
    {
        menuClosed?.Invoke();
    }
}
