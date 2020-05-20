using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class CaseFile : MonoBehaviour
{
    /* Child references */

    [Tooltip("Parent of all left page objects. Must contain pages in same order as the tabs, starting with None.")]
    public Transform pagesLeftParent;
    
    /* State */

    private bool m_Open = false;
    public bool IsOpen => m_Open;
    
    private CaseFilePage m_CurrentPageLeft = CaseFilePage.Brief;

    private GameObject GetPage(CaseFilePage page)
    {
        var pageIndex = (int) page;
        var pageCount = pagesLeftParent.childCount;
        Debug.AssertFormat(pageIndex < pageCount,
            "Page index {0} is not less than pages left child count {1}.",
            pageIndex, pageCount);
        
        Transform pageTr = pagesLeftParent.GetChild(pageIndex);
        return pageTr.gameObject;
    }
    
    private void SetPageActive(CaseFilePage page, bool active)
    {
        GetPage(page).SetActive(active);
    }

    private void Start()
    {
        foreach (var caseFilePage in EnumUtil.GetValues<CaseFilePage>())
        {
            SetPageActive(caseFilePage, false);
        }

        // should be Brief at this point
        SetPageActive(m_CurrentPageLeft, true);
    }
    
    public void Open()
    {
        m_Open = true;
        
        // will call Start if activated for the first time
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        m_Open = false;

        gameObject.SetActive(false);
    }

    public void SwitchToPage(CaseFilePage page)
    {
        Debug.AssertFormat(m_Open, "CaseFile.OpenPage called on page {0} but CaseFile itself is not open yet.", page);

        if (page != m_CurrentPageLeft)
        {
            SetPageActive(m_CurrentPageLeft, false);
            m_CurrentPageLeft = page;
            SetPageActive(page, true);
        }
    }
    
    public void SwitchToPageByIndex(int pageIndex)
    {
        var page = (CaseFilePage) pageIndex;
        SwitchToPage(page);
    }
}
