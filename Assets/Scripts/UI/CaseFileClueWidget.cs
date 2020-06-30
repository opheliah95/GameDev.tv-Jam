using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;

public class CaseFileClueWidget : MonoBehaviour
{
    /* Sibling components */

    private Button button;
    
    /* Child references */
    
    [Tooltip("Readable Clue Name Widget")]
    public TextMeshProUGUI clueNameWidget;
    
    /* Init parameters */
    
    private ClueData m_ClueData;

    private void Awake()
    {
        button = this.GetComponentOrFail<Button>();
    }

    public void Init(ClueData clueData, bool isClueFound)
    {
        m_ClueData = clueData;
        clueNameWidget.text = clueData.clueName;
        button.interactable = isClueFound;
    }

    public void PrintItemDescription()
    {
        Debug.Log(m_ClueData.description);
    }

    // Event callback
    public void OnClueSelected()
    {
        // this event call is a bit convoluted, but allows to turn static access to instance access
        // without using a singleton, nor passing CaseFilePageClues instance in CaseFileClueWidget.Init
        CaseFilePageClues.InvokeClueSelected(m_ClueData);
    }
}
