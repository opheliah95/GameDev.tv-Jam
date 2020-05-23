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

    public Transform[] GetAllPages()
    {
        int childCount = pagesParent.childCount;
        
        var pageTransforms = new Transform[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            pageTransforms[i] = pagesParent.GetChild(i);
        }

        return pageTransforms;
    }

    public void HideAllPages()
    {
        foreach (Transform child in pagesParent)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    private void HideCurrentPage(Transform[] pageTransforms)
    {
        Debug.AssertFormat(m_CurrentPageIndex < pageTransforms.Length, this,
            "We registered {0} pages, side {1} cannot hide current page {2}",
            this, pageTransforms.Length, m_CurrentPageIndex);

        pageTransforms[m_CurrentPageIndex].gameObject.SetActive(false);
    }
    
    /// Show page of given index and transform on this side,
    /// without hiding anything else (should only be called after HideAllPages)
    public void ShowPage(int pageIndex, Transform[] pageTransforms)
    {
        Transform pageTransform = pageTransforms[pageIndex];
        
        // update model
        m_CurrentPageIndex = pageIndex;
        // move page to this side, preserving local position
        pageTransform.SetParent(pagesParent, false);
        // show page if not already active
        pageTransform.gameObject.SetActive(true);
    }
    
    /// Show page of given index and transform on this side,
    /// hiding any current page
    public void ShowOnlyPage(int pageIndex, Transform[] pageTransforms)
    {
        HideCurrentPage(pageTransforms);
     
        Debug.AssertFormat(pageIndex < pageTransforms.Length, this,
            "We registered {0} pages, side {1} cannot show page {2}",
            this, pageTransforms.Length, pageIndex);
        
        Transform pageTransform = pageTransforms[pageIndex];
        
        // update model
        m_CurrentPageIndex = pageIndex;
        // if needed, move page to this side, preserving local position
        if (pageTransform.parent != pagesParent)
        {
            pageTransform.SetParent(pagesParent, false);
        }
        // show page if not already active
        pageTransform.gameObject.SetActive(true);
    }
    
    /// Switch to page by index, moving page from another side if needed.
    /// Main entry function for case file page navigation, used by tab buttons.
    public void SwitchToPage(int pageIndex)
    {
        caseFile.SwitchSideToPage(this, pageIndex);
    }
}
