using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    //enum of states for the UI
    public enum eUIState {BASE, ENDGAME, PAUSEMENU}
    public eUIState currUIState;


    //all the UI elements in a play scene
    public GameObject PauseM = null;
    public GameObject EndM = null;
    //public GameObject UnitM = null;
    

	// Use this for initialization
	void Start ()
    {
        resetUI();

	}
	
	// Update is called once per frame
	void Update ()
    {
        //bool changed = false;
        //check what UI state is
        //switch on enum
        //enable / disable elements of ui & manage UI anims

        //clear ui to base state
        if (currUIState == eUIState.BASE)
        {
            

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.PAUSEMENU;
                stateSwitch();
                return;
            }
        }
        if(currUIState == eUIState.PAUSEMENU)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.BASE;
                stateSwitch();
                return;
            }
            
        }
        if(currUIState == eUIState.ENDGAME)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.BASE;
                stateSwitch();
                return;
            }
        }
        
    }

    //will check which UI elements should be active
    void stateSwitch()
    {
        switch (currUIState)
        {
            case eUIState.BASE:
                resetUI();
                break;

            case eUIState.PAUSEMENU:
                resetUI();
                PauseM.SetActive(true);
                break;

            case eUIState.ENDGAME:
                resetUI();
                EndM.SetActive(true);
                break;

        }
    }
    //turns off all of the ui elements currently active
   public void resetUI()
    {
        if (PauseM)
        {
            PauseM.SetActive(false);
        }
        if (EndM)
        {
            EndM.SetActive(false);
        }
        
        //currUIState = eUIState.BASE;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

   
}
