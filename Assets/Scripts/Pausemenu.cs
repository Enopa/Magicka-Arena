using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausemenu : MonoBehaviour
{
    //this game object is now public so that it does not clash with being set inactive in the player script
    public GameObject pause;
    public GameObject options;
    public GameObject controls;
    public GameObject spellSelect;
    private bool spIsActive;
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //this IF statement checks the input and will also check to see if the spell menu is active
        if (Input.GetButtonDown("Cancel") && !spellSelect.activeSelf)
        {
            //this ensures that you can still close the pause menu when on the options screen
             if (options.activeSelf)
            {
                options.SetActive(false);
                pause.SetActive(false);
            }
            //same as the options closing, but with the controls screen within the pause menu
            else if (controls.activeSelf)
            {
                controls.SetActive(false);
                pause.SetActive(false);
            }
             //otherwise, only the pause menu will close
            else
            {
                pause.SetActive(!pause.activeSelf);
            }
        }
        //fire3 is the E button on the keyboard. this IF statement checks if it was pressed and will activate the spell menu
        if (Input.GetButtonDown("Fire3") && !pause.activeSelf)
        { 
            //this will only happen if the pause menu is not active
            spellSelect.SetActive(!spellSelect.activeSelf);
        }
        //if either the pause menu or spell menu is open, the cursor will be visible and you are able to move it around
        if (pause.activeSelf || spellSelect.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //by activating timescale, the movement of objects is halted, thus pausing the game
        if (pause.activeSelf)
        {
            Time.timeScale = 0;

        } else
        {
            Time.timeScale = 1;
        }
    }
}
