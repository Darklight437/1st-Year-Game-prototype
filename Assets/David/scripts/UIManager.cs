using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    //enum of states for the UI
    public enum eUIState {BASE, ENDGAME, PAUSEMENU}
    public eUIState currUIState;

    public enum eCommandState { MSC,ASC,MC,AC,SC,C}
    public eCommandState CurrentCommand;

    //all the UI elements in a play scene
    public GameObject PauseM = null;
    public GameObject EndM = null;
    //public GameObject UnitM = null;

    //the spare rectTransforms that the buttons will sit at
    public RectTransform[] Buttons = new RectTransform[5];
    //the core position that the buttons move to
    public GameObject MenuPosition;

    //the button Gameobjects
    public GameObject MoveButton;
    public GameObject AttackButton;
    public GameObject SpecialButton;
    public GameObject CancelButton;


	// Use this for initialization
	void Start ()
    {
        resetUI();
        //make function that clears rect transforms of buttons
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        
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

    public void ButtonState()
    {
        switch(CurrentCommand)
        {

            case eCommandState.MSC:
                MoveButton.SetActive(true);
                SpecialButton.SetActive(true);
                CancelButton.SetActive(true);
                //moveButton();

                break;

            case eCommandState.ASC:
                AttackButton.SetActive(true);
                SpecialButton.SetActive(true);
                CancelButton.SetActive(true);
                //moveButton();
                break;

            case eCommandState.MC:
                MoveButton.SetActive(true);
                CancelButton.SetActive(true);
                //moveButton();
                break;

            case eCommandState.AC:
                AttackButton.SetActive(true);
                CancelButton.SetActive(true);
                //moveButton();
                break;

            case eCommandState.SC:
                SpecialButton.SetActive(true);
                CancelButton.SetActive(true);
                //moveButton();
                break;

            case eCommandState.C:
                CancelButton.SetActive(true);
                //moveButton();
                break;
        }
    }

    private void turnOffButtons()
    {
        MoveButton.SetActive(false);
        AttackButton.SetActive(false);
        SpecialButton.SetActive(false);
        CancelButton.SetActive(false);

    }
   
}
