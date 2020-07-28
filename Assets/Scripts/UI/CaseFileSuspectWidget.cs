using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;

public class CaseFileSuspectWidget : MonoBehaviour
{
    /* Sibling components */

    private Button button;
    
    /* Child references */
    
    [Tooltip("Cross picture for eliminated suspects")]
    public Image cross;
    
    [Tooltip("Readable Suspect Name Widget")]
    public TextMeshProUGUI suspectNameWidget;
    
    /* Init parameters */
    
    private SuspectData m_SuspectData;
    
    /* State */

    private bool m_Crossed;

    private void Awake()
    {
        button = this.GetComponentOrFail<Button>();
    }

    public void Init(SuspectData suspectData, bool isCrossed)
    {
        m_SuspectData = suspectData;
        suspectNameWidget.text = suspectData.suspectName;
        m_Crossed = isCrossed;
        cross.enabled = isCrossed;
    }
    
    private void ToggleCrossSuspect()
    {
        if (!m_Crossed)
        {
            m_Crossed = true;
            cross.enabled = true;
            CaseManager.Instance.CurrentCaseProgress.CrossSuspect(m_SuspectData.stringID);
        }
        else
        {
            m_Crossed = false;
            cross.enabled = false;
            CaseManager.Instance.CurrentCaseProgress.UncrossSuspect(m_SuspectData.stringID);
        }
    }

    // Event callback
    public void OnSuspectHoveredStart()
    {
        // this event call is a bit convoluted, but allows to turn static access to instance access
        // without using a singleton, nor passing CaseFilePageSuspects instance in CaseFileSuspectWidget.Init
        CaseFilePageSuspects.InvokeSuspectHovered(m_SuspectData);
    }
    
    // Event callback (named Submit because it is bound to button On Click
    // and supports pointer click and keyboard/gamepad Submit; do not use
    // EventTrigger component which would require both Pointer Click and Submit)
    public void OnSuspectSubmitted()
    {
        ToggleCrossSuspect();
        CaseFilePageSuspects.InvokeSuspectCrossingChanged(m_SuspectData);
    }
}
