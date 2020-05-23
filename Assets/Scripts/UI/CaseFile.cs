using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class CaseFile : MonoBehaviour
{
    /* Child references */
    
    [Tooltip("Array of size 2 with Pages_Left and Pages_Right, in this order.")]
    public CaseFileSide[] sides;

    /// Array of all page transforms, whatever side they are on.
    /// Start with None, then all "real" pages in same order as the tabs
    private Transform[] pageTransforms;
    
    /* State */

    private bool m_Open = false;
    public bool IsOpen => m_Open;

    private void Start()
    {
        // all pages should be on Pages_Left on start
        Debug.AssertFormat(sides.Length == 2, this, "sides has length {0}, expected 2", sides.Length);
        Debug.AssertFormat(sides[1].GetAllPages().Length == 0, sides[1], "Right Side has {0} child(ren), expected 0", sides[1].transform.childCount);

        // store references to all pages on left side now, we'll move them later between sides
        pageTransforms = sides[0].GetAllPages();
        
        // on left side, start by showing page 1 (Brief)
        sides[0].HideAllPages();
        sides[0].ShowPage(1, pageTransforms);
        
        // on right side, start by showing page 0 (None)
        // there are no pages to start with, according to assertions above, so no need to hide first
        sides[1].ShowPage(0, pageTransforms);
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

    /// Switch to target page on target side, moving it from another side if needed.
    public void SwitchSideToPage(CaseFileSide side, int pageIndex)
    {
        // check that target page is not None
        if (pageIndex == 0)
        {
            Debug.LogWarningFormat(this,
                "Cannot show page index 0 (None) on side {0}, as there is only one None page " +
                "and it couldn't be used as fallback page for the side already displaying it.", side);
            return;
        }
        
        // check that target page is not already shown on target side
        if (pageIndex == side.CurrentPageIndex)
        {
            Debug.LogWarningFormat(this, "Side {0} already shows page {1}", side, pageIndex);
            return;
        }

        // check if page is not already shown on the target side
        Transform page = pageTransforms[pageIndex];
        CaseFileSide oldSide = null;
        if (page.gameObject.activeSelf && page.parent != side.pagesParent)
        {
            // Page is on another side at the moment, so we will move it to the target side
            // in side.ShowOnlyPage more below.
            // On the old side, we will fallback to page 0 (None).
            // However, we wait for side.ShowOnlyPage below first so it doesn't re-hide
            // the None page afterward.
            oldSide = page.parent.parent.GetComponentOrFail<CaseFileSide>();
        }

        // show new page on target side, moving it if needed
        side.ShowOnlyPage(pageIndex, pageTransforms);
        
        // check if page has moved
        if (oldSide != null)
        {
            // We can now fallback to page 0 (None) on the old side.
            // We must not hide what the old side thinks is the current page
            // (it's now the target page on the new side), so we must use ShowPage
            // instead of ShowOnlyPage.
            oldSide.ShowPage(0, pageTransforms);
        }
    }
}
