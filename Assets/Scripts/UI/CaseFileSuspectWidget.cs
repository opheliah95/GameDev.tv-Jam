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
    
    [Tooltip("Readable Suspect Name Widget")]
    public TextMeshProUGUI suspectNameWidget;
    
    /* Init parameters */
    
    private SuspectData m_SuspectData;

    private void Awake()
    {
        button = this.GetComponentOrFail<Button>();
    }

    public void Init(SuspectData suspectData)
    {
        m_SuspectData = suspectData;
        suspectNameWidget.text = suspectData.suspectName;
    }

    // Event callback
    public void OnSuspectSelected()
    {
        // this event call is a bit convoluted, but allows to turn static access to instance access
        // without using a singleton, nor passing CaseFilePageSuspects instance in CaseFileSuspectWidget.Init
        CaseFilePageSuspects.InvokeSuspectSelected(m_SuspectData);
    }
}
