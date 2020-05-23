using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseFilePageScenes : CaseFilePage
{
    [Tooltip("Grid containing widgets for scenes to go to")]
    public GridLayoutGroup sceneGrid;

    [Tooltip("Scene Button Prefab to instantiate in Scene Grid (taken from Scene Grid or Assets)")]
    public GameObject sceneButtonPrefab;
    
    public override void OnShow()
    {
        Debug.Log("OnShow Scenes");
        
        // First, create enough scene widgets for the current mission
        // there may already be one or more scene widgets under sceneGrid to visualize better
        // in the editor, so only add what you need.
        // We assume all children of sceneGrid are actually scene widgets, so just count them.
        // If you open the Scenes page for the second time in this mission, nothing should be added.
        int currentSceneButtonsCount = sceneGrid.transform.childCount;
        // TODO: get actual mission info
        int missionInfoSceneCount = 4;
        if (currentSceneButtonsCount < 4)
        {
            for (int i = currentSceneButtonsCount; i < 4; i++)
            {
                Instantiate(sceneButtonPrefab, sceneGrid.transform);
            }
        }
    }
}
