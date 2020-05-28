
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerReConstruct : MonoBehaviour
{
    public bool inReconstruction;

   
    private void OnReconstruction()
    {
        if (inReconstruction)
            ExitReconstruct();
        else
            StartReconstruct();

    }

    private void StartReconstruct()
    {
        ReconstructionManager.Instance.ShowReconstruction();
        inReconstruction = true;
    }

    private void ExitReconstruct()
    {
        ReconstructionManager.Instance.ExitReconstruction();
        inReconstruction = false;
    }
}
