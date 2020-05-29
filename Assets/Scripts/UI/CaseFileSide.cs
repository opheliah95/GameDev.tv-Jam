using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class CaseFileSide : MonoBehaviour
{
    /* Parent references */
    
    [Tooltip("Parent case file")]
    public CaseFile caseFile;
    
    /* Child references */

    [Tooltip("Parent of all page objects on this side.")]
    public Transform pagesParent;
    
    /// Current page on this side
    private int m_CurrentPageIndex = 0;
    public int CurrentPageIndex => m_CurrentPageIndex;

    public CaseFilePage[] GetAllPages()
    {
        int childCount = pagesParent.childCount;
        
        var pages = new CaseFilePage[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            pages[i] = pagesParent.GetChild(i).GetComponentOrFail<CaseFilePage>();
        }

        return pages;
    }

    public void HideAllPages()
    {
        foreach (Transform child in pagesParent)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    private void HideCurrentPage(CaseFilePage[] pages)
    {
        Debug.AssertFormat(m_CurrentPageIndex < pages.Length, this,
            "We registered {0} pages, side {1} cannot hide current page {2}",
            this, pages.Length, m_CurrentPageIndex);

        pages[m_CurrentPageIndex].gameObject.SetActive(false);
    }
    
    /// Show page of given index and transform on this side,
    /// without hiding anything else (should only be called after HideAllPages)
    public void ShowPage(int pageIndex, CaseFilePage[] pages)
    {
        CaseFilePage page = pages[pageIndex];
        Transform pageTransform = page.transform;
        
        // update model
        m_CurrentPageIndex = pageIndex;
        // if needed, move page to this side, preserving local position
        if (pageTransform.parent != pagesParent)
        {
            pageTransform.SetParent(pagesParent, false);
        }
        // show page if not already active
        page.gameObject.SetActive(true);
        
        page.OnShow();
    }
    
    /// Show page of given index and transform on this side,
    /// hiding any current page
    public void ShowOnlyPage(int pageIndex, CaseFilePage[] pages)
    {
        HideCurrentPage(pages);
     
        Debug.AssertFormat(pageIndex < pages.Length, this,
            "We registered {0} pages, side {1} cannot show page {2}",
            this, pages.Length, pageIndex);

        ShowPage(pageIndex, pages);
    }
    
    /// Switch to page by index, moving page from another side if needed.
    /// Callback for case file page navigation, used by tab buttons.
    public void SwitchToPage(int pageIndex)
    {
        caseFile.SwitchSideToPage(this, pageIndex);
    }
    
    /// Refresh page content. Call when case file is opened, in case content changed on page since last time.
    public void RefreshCurrentPage(CaseFilePage[] pages)
    {
        Debug.AssertFormat(m_CurrentPageIndex < pages.Length, this,
            "Side {0} has m_CurrentPageIndex {1}, but only {2} pages have been passed from CaseFile.",
            this, m_CurrentPageIndex, pages.Length);
        
        CaseFilePage page = pages[m_CurrentPageIndex];
        page.OnShow();
    }
}
