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
    public TextMeshPro briefTextBasic;
    
    [Tooltip("Text: extra info on brief")]
    public TextMeshPro briefTextExtra;
    
    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        InitToggles(caseData);
        InitButton(caseData);
        InitText(caseData);
    }

    public void InitToggles(CaseData caseData)
    {
        
    }
    
    public void InitButton(CaseData caseData)
    {
        
    }
    
    public void InitText(CaseData caseData)
    {
        
    }
}
