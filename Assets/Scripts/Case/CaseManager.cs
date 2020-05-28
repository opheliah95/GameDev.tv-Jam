﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsPattern;
using UnityConstants;

public class CaseManager : SingletonManager<CaseManager>
{
    private CaseData m_CurrentCaseData;
    public CaseData CurrentCaseData => m_CurrentCaseData;

    private CaseProgress m_CurrentCaseProgress;
    
    protected override void Init()
    {
        // only one case for the demo, so just hardcode load it
        m_CurrentCaseData = Resources.Load<CaseData>("Data/Case/CaseData1");
        Debug.AssertFormat(m_CurrentCaseData != null, this, "CaseManager could not find CaseData at Resources/Data/Case/CaseData1.");
        
        m_CurrentCaseProgress = new CaseProgress();
    }

    public bool HasUnlockedScene(CaseSceneType sceneType, ScenesEnum sceneEnum)
    {
        return m_CurrentCaseProgress.HasUnlockedScene(sceneType, sceneEnum);
    }
    
    public void UnlockScene(CaseSceneType sceneType, ScenesEnum sceneEnum)
    {
        m_CurrentCaseProgress.UnlockScene(sceneType, sceneEnum);
    }
}
