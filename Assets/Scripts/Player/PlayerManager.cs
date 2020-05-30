using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Player Character prefab")]
    public GameObject playerCharacterPrefab;

    /// Spawn transform (only position and rotation are used). Depends on the level. Retrieved by tag.
    private Transform m_SpawnTransform;
    
    private void Start()
    {
        GameObject spawnTransformObj = GameObject.FindWithTag(Tags.SpawnTransform);
        Debug.Assert(spawnTransformObj != null, "No game object with Tag \"SpawnTransform\" found in the scenes.", this);
        
        m_SpawnTransform = GameObject.FindWithTag(Tags.SpawnTransform).transform;
        SpawnPlayerCharacter();
    }

    private void SpawnPlayerCharacter()
    {
        Instantiate(playerCharacterPrefab, m_SpawnTransform.position, m_SpawnTransform.rotation);
    }
}
