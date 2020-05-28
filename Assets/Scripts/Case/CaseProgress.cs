using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

using UnityConstants;

public class CaseProgress
{
    /// Array of set of unlocked scene enums, index by SceneType enum as integer
    private readonly HashSet<ScenesEnum>[] m_UnlockedSceneEnumsSetBySceneType = {new HashSet<ScenesEnum>(), new HashSet<ScenesEnum>()};
    
    public bool HasUnlockedScene(CaseSceneType sceneType, ScenesEnum sceneEnum)
    {
        return m_UnlockedSceneEnumsSetBySceneType[(int) sceneType].Contains(sceneEnum);
    }
    
    public bool UnlockScene(CaseSceneType sceneType, ScenesEnum sceneEnum)
    {
        return m_UnlockedSceneEnumsSetBySceneType[(int) sceneType].Add(sceneEnum);
    }
}
