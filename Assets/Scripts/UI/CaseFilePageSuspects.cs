using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;

public class CaseFilePageSuspects : CaseFilePage
{
    private static event Action<SuspectData> suspectHovered;
    private static event Action<SuspectData> suspectCrossingChanged;
    
    [Tooltip("Grid containing widgets for suspects to find")]
    public GridLayoutGroup suspectsGrid;
    
    [Tooltip("Suspect Button Prefab to instantiate in Suspects Grid (taken from Suspects Grid or Assets)")]
    public GameObject suspectButtonPrefab;

    [Tooltip("Suspect description widget common to all suspects")]
    public TextMeshProUGUI suspectDescriptionWidget;

    private void OnEnable()
    {
        suspectHovered += OnSuspectHovered;
        suspectCrossingChanged += OnSuspectCrossingChanged;
    }

    private void OnDisable()
    {
        suspectHovered -= OnSuspectHovered;
        suspectCrossingChanged -= OnSuspectCrossingChanged;
    }

    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        
        Debug.AssertFormat(caseData.suspectDataArray != null, caseData, "caseData.suspectDataArray is null on CaseData {0}", caseData);
        Debug.AssertFormat(caseData.culpritData != null, caseData, "culpritData is null on CaseData {0}", caseData);
        Debug.AssertFormat(caseData.suspectDataArray.Contains(caseData.culpritData), caseData, "caseData.suspectDataArray does not contain culpritData in CaseData {0}", caseData);

        InitSuspectWidgets(caseData);
        
        // clear suspect bio until one is selected
        suspectDescriptionWidget.text = "";
    }

    private void InitSuspectWidgets(CaseData caseData)
    {
        // First, create enough widgets for the current case
        // there may already be one or more widgets under suspectsGrid to visualize better
        // in the editor, so only add what you need.
        // We assume all children of suspectsGrid are actually suspect widgets, so just count them.
        // If you open the Suspects page for the second time in this mission, nothing should be added.
        int currentButtonsCount = suspectsGrid.transform.childCount;

        for (int i = currentButtonsCount; i < caseData.suspectDataArray.Length; i++)
        {
            Instantiate(suspectButtonPrefab, suspectsGrid.transform);
        }

        // Second, disable any extra widgets (e.g. created from a previous case that had more suspects than the current one)
        for (int i = caseData.suspectDataArray.Length; i < currentButtonsCount; i++)
        {
            suspectsGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Third, setup the scene widgets for each available scene
        for (int i = 0; i < caseData.suspectDataArray.Length; i++)
        {
            // get widget script
            Transform widgetTr = suspectsGrid.transform.GetChild(i);
            Debug.AssertFormat(widgetTr != null, suspectsGrid,
                "Scene widget grid {0} has no child of index {1}, yet we should have created up to {2} widgets " +
                "after the existing {3}.", suspectsGrid, i, caseData.suspectDataArray.Length, currentButtonsCount);
            var widget = widgetTr.GetComponentOrFail<CaseFileSuspectWidget>();
            
            // get suspect data
            SuspectData suspectData = caseData.suspectDataArray[i];
            
            // check if suspect has been crossed
            bool isCrossed = CaseManager.Instance.CurrentCaseProgress.HasCrossedSuspect(suspectData.stringID);
            
            // init suspect button widget
            widget.Init(suspectData, isCrossed);
        }
    }

    public static void InvokeSuspectHovered(SuspectData suspectData)
    {
        suspectHovered?.Invoke(suspectData);
    }

    public static void InvokeSuspectCrossingChanged(SuspectData suspectData)
    {
        suspectCrossingChanged?.Invoke(suspectData);
    }
    
    private void OnSuspectHovered(SuspectData suspectData)
    {
        // show suspect description text
        suspectDescriptionWidget.text = suspectData.description;
    }
    
    private void OnSuspectCrossingChanged(SuspectData suspectData)
    {
        // check if only one suspect remains (go through CaseProgress
        // ie model rather than CaseFileSuspectWidget to benefit from helper
        // method)
    }
}
