using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    public Transform cameraPosition;

    [SerializeField]
    public Transform cameraLastPosition;

    public float effectMultiplier;
    public float difference;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        cameraPosition = Camera.main.transform;
        cameraLastPosition = cameraPosition.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 deltaMovement = cameraPosition.position - cameraLastPosition.position;
        float dir = cameraPosition.position.y - transform.position.y;
        transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0) * effectMultiplier * dir;
        cameraLastPosition.position = cameraPosition.position;

        if (Mathf.Abs(cameraPosition.position.y - transform.position.y) >= difference)
        {
            Vector3 newPos = new Vector3(transform.position.x, cameraPosition.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
        }
    }
}
