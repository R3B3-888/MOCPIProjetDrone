using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SideMenu : MonoBehaviour
{
    public GameObject sideMenu;
    public bool isShown;
    private bool isPaused;

    void Start()
    {
        sideMenu.SetActive(false);
        isShown = false;
        isPaused = PauseMenu.paused;
    }

    void Update()
    {
        isPaused = PauseMenu.paused;

        if(Input.GetKeyDown(KeyCode.Tab) && !isPaused)
        {
            showSideMneu();
        }
    }

    public void showSideMneu()
    {
        isShown = !isShown;
        sideMenu.SetActive(isShown);
        if(isShown)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}