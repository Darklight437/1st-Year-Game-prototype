using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    //custom type for menu buttons
    public enum eActionType
    {
        NULL = 0,
        ATTACK = 1,
        MOVEMENT = 2,
        SPECIAL = 3,
    }

    //function type for parsing a unit
    public delegate void UnitFunc(Unit unit);

    //list of references to players
    public List<Player> players = new List<Player>();

    //static reference to the statistics object
    public static Statistics stats = null;
    public Statistics statsReference = null;

    //reference to the active player
    public Player activePlayer = null;

    //reference to the selected unit
    public Unit selectedUnit = null;

    //reference to the camera movement script
    public CameraMovement cam = null;

    //ID of the active player
    public int turn = 0;

    //bool indicating if the game is in-between turns
    public bool transitioning = false;

    //reference to the world space UI manager script
    public WorldspaceManager worldUI = null;

    //type of action from the world space manager
    public eActionType actionEvent = eActionType.NULL;

    //flag for the camera selection to respond to
    public bool uiPressed = false;

    //location of selected tile
    public int tileX = 0;
    public int tileY = 0;

	// Use this for initialization
	void Start ()
    {
        //set the reference
        GameManagment.stats = statsReference;
 
        activePlayer = players[0];

        //get the size of the players array once
        int playerCount = players.Count;

        //iterate through the players array, invoking the ID setter
        for (int i = 0; i < playerCount; i++)
        {
            players[i].LinkIDs(i);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        
	}


    /*
    * OnNextTurn 
    * 
    * called when the end turn button is pressed
    * 
    * @returns void
    */
    public void OnNextTurn()
    {

        if (transitioning || activePlayer.IsBusy())
        {
            return;
        }

        //turn off the action menu
        worldUI.gameObject.GetComponent<Canvas>().enabled = false;

        //deselect the unit
        selectedUnit = null;

        //increment the turn id
        turn++;
        
        //wrap around the turn id
        if (turn > players.Count - 1)
        {
            turn = 0;
        }

        //set the active player
        activePlayer = players[turn];
        activePlayer.CalculateKingPosition();

        cam.Goto(activePlayer.kingPosition, cam.transform.eulerAngles + new Vector3(0.0f, 180.0f, 0.0f), OnCameraFinished);

        transitioning = true;
    }


    /*
    * OnUnitSelected 
    * 
    * callback when a unit is clicked on
    * 
    * @param Unit unit - the unit that was clicked
    * @returns void
    */
    public void OnUnitSelected(Unit unit)
    {

        //there are no units selected
        if (unit.playerID == activePlayer.playerID)
        {
            //the player is selecting a different unit, hide the menu
            if (selectedUnit != unit)
            {
                worldUI.gameObject.GetComponent<Canvas>().enabled = false;
            }

            //David
            /*  going to set a state of the UI manager here in the future
             * 
             *  enum UI state = selected;
             */

            selectedUnit = unit;
        }
    }


    /*
    * OnTileSelected 
    * 
    * callback when an empty tile is clicked on
    * 
    * @param int x - the rounded x coordinate of the tile
    * @param int y - the rounded y coordinate of the tile
    * @returns void
    */
    public void OnTileSelected(int x, int y)
    {

        //if a unit was already selected and an empty tile was selected
        if (selectedUnit != null)
        {
            tileX = x;
            tileY = y;

            //set-up the world UI
            worldUI.transform.position = new Vector3(x + 0.5f, worldUI.transform.position.y, y + 0.5f);
            worldUI.gameObject.GetComponent<Canvas>().enabled = true;

            //selectedUnit.transform.position = new Vector3(x + 0.5f, selectedUnit.transform.position.y, y + 0.5f);
            //selectedUnit = null;
        }
    }


    /*
    * OnActionSelected 
    * 
    * callback when the world space manager's buttons are clicked
    * 
    * @param int actionType - the type of action that was triggered (int representation of the enum)
    */
    public void OnActionSelected(int actionType)
    {
        //the first entry in eActionType is null
        actionEvent = (eActionType)(actionType + 1);

        //set the flag for the camera selection system to process
        uiPressed = true;

        //turn off the action menu
        worldUI.gameObject.GetComponent<Canvas>().enabled = false;

        //execute the action
        selectedUnit.Execute(actionEvent, tileX, tileY);

        //deselect the unit
        selectedUnit = null;

       
    }

    /*
    * OnCameraFinished 
    * 
    * callback when the camera has finished it's automatic transition
    * 
    * @returns void
    */
    public void OnCameraFinished()
    {
        transitioning = false;
    }



    
}
