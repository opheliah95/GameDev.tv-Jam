
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
        FindObjectOfType<ReconstructionManager>().ShowReconstruction();
        inReconstruction = true;
    }

    private void ExitReconstruct()
    {
        FindObjectOfType<ReconstructionManager>().ExitReconstruction();
        inReconstruction = false;
    }
}
