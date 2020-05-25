using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CommonsHelper;
using UnityConstants;

public class CaseFilePageScenes : CaseFilePage
{
    [Tooltip("Grid containing widgets for scenes to go to")]
    public GridLayoutGroup sceneGrid;

    [Tooltip("Scene Button Prefab to instantiate in Scene Grid (taken from Scene Grid or Assets)")]
    public GameObject sceneButtonPrefab;
    
    public override void OnShow()
    {
        Debug.Log("OnShow Scenes");
        
        // First, create enough scene widgets for the current case
        // there may already be one or more scene widgets under sceneGrid to visualize better
        // in the editor, so only add what you need.
        // We assume all children of sceneGrid are actually scene widgets, so just count them.
        // If you open the Scenes page for the second time in this mission, nothing should be added.
        int currentSceneButtonsCount = sceneGrid.transform.childCount;
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        CaseData.SceneData[] sceneDataArray = caseData.sceneDataArray;
        
        Debug.AssertFormat(sceneDataArray != null, caseData, "SceneDataArray is null on CaseData {0}", caseData);
        
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
            Transform sceneWidgetTr = sceneGrid.transform.GetChild(i);
            Debug.AssertFormat(sceneWidgetTr != null, sceneGrid,
                "Scene widget grid {0} has no child of index {1}, yet we should have created up to {2} widgets " +
                "after the existing {3}.", sceneGrid, i, sceneDataArray.Length, currentSceneButtonsCount);
            var widget = sceneWidgetTr.GetComponentOrFail<CaseFileSceneWidget>();
            CaseData.SceneData sceneData = sceneDataArray[i];
            widget.Init(sceneData);
        }
    }
}
