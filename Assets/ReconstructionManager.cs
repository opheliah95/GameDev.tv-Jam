using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconstructionManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> itemsToReconstruct = new List<GameObject>() ;

    // Start is called before the first frame update
    void Start()
    {
        int children = transform.childCount;

        for (int i = 0; i < children; i++)
        {
            itemsToReconstruct.Add(transform.GetChild(i).gameObject);
        }
        
    }

    
    public void ShowReconstruction()
    {
        foreach(GameObject i in itemsToReconstruct)
        {
            i.SetActive(true);
        }
    }


    public void ExitReconstruction()
    {
        foreach (GameObject i in itemsToReconstruct)
        {
            i.SetActive(false);
        }
    }
}
