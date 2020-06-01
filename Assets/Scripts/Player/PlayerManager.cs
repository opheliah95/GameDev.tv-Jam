using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsPattern;
using UnityConstants;

public class PlayerManager : SingletonManager<PlayerManager>
{
    [Tooltip("Player Character prefab")]
    public GameObject playerCharacterPrefab;

    private bool m_HasSpawnedPlayerCharacterForCurrentLevel = false;

    private void Start()
    {
        // convenient when testing level directly in the editor
        SpawnPlayerCharacterIfNotAlready();
    }

    public void SpawnPlayerCharacterIfNotAlready()
    {
        if (m_HasSpawnedPlayerCharacterForCurrentLevel)
        {
            return;
        }
        
        // retrieve spawn transform in this level
        GameObject spawnTransformObj = GameObject.FindWithTag(Tags.SpawnTransform);
        Debug.Assert(spawnTransformObj != null, "No game object with Tag \"SpawnTransform\" found in the scenes.", this);
        
        Transform spawnTransform = GameObject.FindWithTag(Tags.SpawnTransform).transform;
        Instantiate(playerCharacterPrefab, spawnTransform.position, spawnTransform.rotation);
        
        m_HasSpawnedPlayerCharacterForCurrentLevel = true;
    }

    public void OnChangeLevel()
    {
        m_HasSpawnedPlayerCharacterForCurrentLevel = false;
    }
}
