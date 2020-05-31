using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;
using UnityConstants;

public class CaseFilePageScenes : CaseFilePage
{
    [Tooltip("Grid containing widgets for real scenes to go to")]
    public GridLayoutGroup realSceneGrid;

    [Tooltip("Grid containing widgets for reconstruction scenes to go to")]
    public GridLayoutGroup reconstructionSceneGrid;

    [Tooltip("Real Scene Button Prefab to instantiate in Scene Grid (taken from Scene Grid or Assets)")]
    public GameObject realSceneButtonPrefab;
    
    [Tooltip("Reconstruction Scene Button Prefab to instantiate in Scene Grid (taken from Scene Grid or Assets)")]
    public GameObject reconstructionSceneButtonPrefab;
    
    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        
        Debug.AssertFormat(caseData.realSceneDataArray != null, caseData, "realSceneDataArray is null on CaseData {0}", caseData);
        Debug.AssertFormat(caseData.reconstructionSceneDataArray != null, caseData, "reconstructionSceneDataArray is null on CaseData {0}", caseData);

        InitSceneWidgets(CaseSceneType.Real, realSceneGrid, caseData.realSceneDataArray, realSceneButtonPrefab);
        InitSceneWidgets(CaseSceneType.Reconstruction, reconstructionSceneGrid, caseData.reconstructionSceneDataArray, reconstructionSceneButtonPrefab);
    }

    private static void InitSceneWidgets(CaseSceneType sceneType, GridLayoutGroup sceneGrid, SceneData[] sceneDataArray, GameObject sceneButtonPrefab)
    {
        // First, create enough scene widgets for the current case
        // there may already be one or more scene widgets under sceneGrid to visualize better
        // in the editor, so only add what you need.
        // We assume all children of sceneGrid are actually scene widgets, so just count them.
        // If you open the Scenes page for the second time in this mission, nothing should be added.
        int currentSceneButtonsCount = sceneGrid.transform.childCount;


        for (int i = currentSceneButtonsCount; i < sceneDataArray.Length; i++)
        {
            Instantiate(sceneButtonPrefab, sceneGrid.transform);
        }

        // Second, disable any extra widgets (e.g. created from a previous case that had more scenes than the current one)
        for (int i = sceneDataArray.Length; i < currentSceneButtonsCount; i++)
        {
            sceneGrid.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Third, setup the scene widgets for each available scene
        for (int i = 0; i < sceneDataArray.Length; i++)
        {
            // get widget script
            Transform sceneWidgetTr = sceneGrid.transform.GetChild(i);
            Debug.AssertFormat(sceneWidgetTr != null, sceneGrid,
                "Scene widget grid {0} has no child of index {1}, yet we should have created up to {2} widgets " +
                "after the existing {3}.", sceneGrid, i, sceneDataArray.Length, currentSceneButtonsCount);
            var widget = sceneWidgetTr.GetComponentOrFail<CaseFileSceneWidget>();
            
            // get scene data
            SceneData sceneData = sceneDataArray[i];
            
            // check if scene is unlocked
            bool isSceneUnlocked = sceneData.unlockedOnStart || CaseManager.Instance.HasUnlockedScene(sceneType, sceneData.sceneEnum);
            
            // init scene button widget
            widget.Init(sceneData, isSceneUnlocked);
        }
    }
}
