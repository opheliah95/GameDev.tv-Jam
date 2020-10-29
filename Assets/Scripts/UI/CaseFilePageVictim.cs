using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaseFilePageVictim : CaseFilePage
{
    [Tooltip("Text: death location value")]
    public TextMeshProUGUI deathLocationValue;
    
    [Tooltip("Text: murder weapon value")]
    public TextMeshProUGUI murderWeaponValue;
    
    public override void OnShow()
    {
        CaseData caseData = CaseManager.Instance.CurrentCaseData;
        CaseProgress caseProgress = CaseManager.Instance.CurrentCaseProgress;
        InitDeathLocation(caseData, caseProgress);
        InitMurderWeapon(caseData, caseProgress);
    }

    void InitDeathLocation(CaseData caseData, CaseProgress caseProgress)
    {
        // For now, assume player deductions are perfect and as soon as the death location scene is found,
        // associate it to death location. Later, player will have to set this value manually (and may be wrong).
        if (caseData.deathLocationSceneData != null)
        {
            if (caseProgress.HasUnlockedScene(CaseSceneType.Real, caseData.deathLocationSceneData.sceneEnum) ||
                caseProgress.HasUnlockedScene(CaseSceneType.Reconstruction, caseData.deathLocationSceneData.sceneEnum))
            {
                // scene has been unlocked, show readable name
                deathLocationValue.text = caseData.deathLocationSceneData.sceneName;
            }
        }
        else
        {
            deathLocationValue.text = "N/A";
        }
    }
    
    void InitMurderWeapon(CaseData caseData, CaseProgress caseProgress)
    {
        // For now, assume player deductions are perfect and as soon as the murder weapon clue is found,
        // associate it to murder weapon. Later, player will have to set this value manually (and may be wrong).
        if (caseData.murderWeaponClueData != null)
        {
            if (caseProgress.HasFoundClue(caseData.murderWeaponClueData.stringID))
            {
                // clue has been found, show readable name
                murderWeaponValue.text = caseData.murderWeaponClueData.clueName;
            }
        }
        else
        {
            murderWeaponValue.text = "N/A";
        }

    }
}
