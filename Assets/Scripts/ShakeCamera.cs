using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{ 
    public Camera mainCamera;
    

    public bool startShakin = false;

    // Start is called before the first frame update
    void Start()
    {           
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
       ShakeTheCamera();
    }   

    public void ShakeTheCamera()
    {
        while(startShakin)
        {
        mainCamera.transform.Translate(Vector3.up*3);
        }
    }

    public void ChangeShakinState()
    {
        startShakin = true;
        Debug.Log("START SHAKIN");
    }
}
