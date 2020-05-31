using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CommonsPattern;

public class InGameManager : SingletonManager<InGameManager>
{
    // there is no particular function required in-game that is not already handled by the more specific managers
    // however, we keep this class just to check that _InGameManagers scene was loaded:
    // if (InGameManager.Instance != null) { ... }
    // since getting Scene by index and checking IsValid() or isLoaded is not reliable
    // when adding sub-scenes in editor
}
