using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaseFilePageBrief : CaseFilePage
{
    [Tooltip("Overview image")]
    public Image overviewImage;
    
    [Tooltip("Toggle: cleared victim info")]
    public Toggle victimInfoToggle;
    
    [Tooltip("Toggle: found culprit")]
    public Toggle culpritToggle;
    
    [Tooltip("Toggle: found all clues")]
    public Toggle allCluesToggle;
    
    [Tooltip("Button: close case")]
    public Button closeCaseButton;
    
    [Tooltip("Text: initial brief")]
    public TextMeshProUGUI briefTextBasic;
    
    [Tooltip("Text: extra info on brief")]
    public TextMeshProUGUI briefTextExtra;
    
    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        InitToggles(caseData);
        InitButton(caseData);
        InitText(caseData);
    }

    void InitToggles(CaseData caseData)
    {
        
    }
    
    void InitButton(CaseData caseData)
    {
        
    }
    
    void InitText(CaseData caseData)
    {
        
    }
}
