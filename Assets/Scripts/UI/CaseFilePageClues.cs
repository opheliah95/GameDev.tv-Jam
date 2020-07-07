using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;

public class CaseFilePageClues : CaseFilePage
{
    private static event Action<ClueData> clueSelected;
    
    [Tooltip("Grid containing widgets for clues to find")]
    public GridLayoutGroup cluesGrid;
    
    [Tooltip("Clue Button Prefab to instantiate in Clues Grid (taken from Clues Grid or Assets)")]
    public GameObject clueButtonPrefab;

    [Tooltip("Clue description widget common to all clues")]
    public TextMeshProUGUI clueDescriptionWidget;

    private void OnEnable()
    {
        clueSelected += OnClueSelected;
    }

    private void OnDisable()
    {
        clueSelected -= OnClueSelected;
    }

    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        
        Debug.AssertFormat(caseData.clueDataArray != null, caseData, "caseData.clueDataArray is null on CaseData {0}", caseData);
        Debug.AssertFormat(caseData.murderWeaponClueData != null, caseData, "murderWeaponClueData is null on CaseData {0}", caseData);
        Debug.AssertFormat(caseData.clueDataArray.Contains(caseData.murderWeaponClueData), caseData, "caseData.clueDataArray does not contain murderWeaponClueData in CaseData {0}", caseData);

        InitClueWidgets(caseData);
        
        // clear clue description until one is selected
        clueDescriptionWidget.text = "";
    }

    private void InitClueWidgets(CaseData caseData)
    {
        // First, create enough widgets for the current case
        // there may already be one or more widgets under cluesGrid to visualize better
        // in the editor, so only add what you need.
        // We assume all children of cluesGrid are actually clue widgets, so just count them.
        // If you open the Clues page for the second time in this mission, nothing should be added.
        int currentButtonsCount = cluesGrid.transform.childCount;

        for (int i = currentButtonsCount; i < caseData.clueDataArray.Length; i++)
        {
            Instantiate(clueButtonPrefab, cluesGrid.transform);
        }

        // Second, disable any extra widgets (e.g. created from a previous case that had more clues than the current one)
        for (int i = caseData.clueDataArray.Length; i < currentButtonsCount; i++)
        {
            cluesGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Third, setup the scene widgets for each available scene
        for (int i = 0; i < caseData.clueDataArray.Length; i++)
        {
            // get widget script
            Transform widgetTr = cluesGrid.transform.GetChild(i);
            Debug.AssertFormat(widgetTr != null, cluesGrid,
                "Scene widget grid {0} has no child of index {1}, yet we should have created up to {2} widgets " +
                "after the existing {3}.", cluesGrid, i, caseData.clueDataArray.Length, currentButtonsCount);
            var widget = widgetTr.GetComponentOrFail<CaseFileClueWidget>();
            
            // get clue data
            ClueData clueData = caseData.clueDataArray[i];
            
            // check if clue has been found
            bool isClueFound = CaseManager.Instance.CurrentCaseProgress.HasFoundClue(clueData.stringID);
            
            // init clue button widget
            widget.Init(clueData, isClueFound);
        }
    }

    public static void InvokeClueSelected(ClueData clueData)
    {
        clueSelected?.Invoke(clueData);
    }

    private void OnClueSelected(ClueData clueData)
    {
        clueDescriptionWidget.text = clueData.description;
    }
}
