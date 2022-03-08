using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class SwitchCameras : MonoBehaviour
{
    public PlayerInput input;

    public GameObject mainCamera;
    public GameObject houseBeachCamera;
    public GameObject boatCamera;

    public bool isBeachCameraOn;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        isBeachCameraOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(input.actions["houseBeach"].triggered)
        {
            if(isBeachCameraOn)
            {
                mainCamera.SetActive(true);
                houseBeachCamera.SetActive(false);
                isBeachCameraOn = false;
            }
            else
            {
                mainCamera.SetActive(false);
                houseBeachCamera.SetActive(true);
                isBeachCameraOn = true;
            }
        }
    }
}
