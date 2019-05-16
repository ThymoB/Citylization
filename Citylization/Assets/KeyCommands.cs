using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCommands : MonoBehaviour
{
    
    [Header("Time")]
    public KeyCode slowSpeed = KeyCode.Alpha1;
    public KeyCode medSpeed = KeyCode.Alpha2;
    public KeyCode fastSpeed = KeyCode.Alpha3;
    public KeyCode pause = KeyCode.Space;

    public bool useTimeCommands;

    [Header("Windows")]
    public KeyCode techTree = KeyCode.T;

    // Update is called once per frame
    void Update()
    {
        if (useTimeCommands)
            TimeCommands();

        if (Input.GetKeyDown(techTree))
            Player.instance.techTree.SetActive(!Player.instance.techTree.activeSelf);
    }


    void TimeCommands()
    {
        if (Input.GetKeyDown(slowSpeed))
        {
            TimeSystem.instance.timeScale = TimeSystem.instance.slowSpeed;
            TimeSystem.instance.gameIsPaused = false;
        }
        if (Input.GetKeyDown(medSpeed))
        {
            TimeSystem.instance.timeScale = TimeSystem.instance.medSpeed;
            TimeSystem.instance.gameIsPaused = false;
        }
        if (Input.GetKeyDown(fastSpeed))
        {
            TimeSystem.instance.timeScale = TimeSystem.instance.fastSpeed;
            TimeSystem.instance.gameIsPaused = false;
        }
        if (Input.GetKeyDown(pause))
        {
            if (TimeSystem.instance.gameIsPaused)
            {
                TimeSystem.instance.timeScale = TimeSystem.instance.medSpeed;
                TimeSystem.instance.gameIsPaused = false;
            }
            else
            {
                TimeSystem.instance.timeScale = 0f;
                TimeSystem.instance.gameIsPaused = true;
            }
        }

    }
}
