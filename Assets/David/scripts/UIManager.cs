using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    //enum of states for the UI
    public enum eUIState {BASE, ACTIVEUNIT, ENDGAME, PAUSEMENU}
    public eUIState currUIState;


    //all the UI elements in a play scene
    public GameObject PauseM = null;
    public GameObject UnitM = null;
    

	// Use this for initialization
	void Start ()
    {
		

	}
	
	// Update is called once per frame
	void Update ()
    {
        //check what UI state is
        //switch on enum
        //enable / disable elements of ui & manage UI anims

        if (currUIState == eUIState.BASE)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.PAUSEMENU;
            }
        }


        stateSwitch();
	}

    //will check which UI elements should be active
    void stateSwitch()
    {
        switch (currUIState)
        {
            case eUIState.BASE:

                break;

            case eUIState.ACTIVEUNIT:

                break;


            case eUIState.PAUSEMENU:
                PauseM.SetActive(true);
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
        if(UnitM)
        {
            UnitM.SetActive(false);
        }
        currUIState = eUIState.BASE;
    }
}
