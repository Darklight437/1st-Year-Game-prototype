﻿using System.Collections;
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
        DEATH = 4,
    }

    //function type for parsing a unit
    public delegate void UnitFunc(Unit unit);

    //list of references to players
    public List<Player> players = new List<Player>();

    //static reference to the statistics object
    public static Statistics stats = null;
    public Statistics statsReference = null;

    //reference to the map
    public Map map = null;

    //reference to the active player
    public Player activePlayer = null;

    //reference to the selected unit
    public Unit selectedUnit = null;

    //reference to the starting tile (first selection)
    public Tiles startTile = null;

    //reference to the ending tile (second selection)
    public Tiles endTile = null;

    //reference to the camera movement script
    public CameraMovement cam = null;

    //ID of the active player
    public int turn = 0;

    //bool indicating if the game is in-between turns
    public bool transitioning = false;

    //reference to the world space UI manager script
    public WorldspaceManager worldUI = null;

    //David's
    //reference to Main UI manager script
    //will probably need this later, not active right now
    //public UIManager UIManager = null;

    //type of action from the world space manager
    public eActionType actionEvent = eActionType.NULL;

    //flag for the camera selection to respond to
    public bool uiPressed = false;

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

        TurnUnitsOff();
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

        if (selectedUnit != null)
        {
            //turn off the unit selection glow
            selectedUnit.GetComponent<Renderer>().material.shader = Shader.Find("Custom/DefaultShader");
        }

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

        TurnUnitsOff();
    }

    public void TurnUnitsOff()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != activePlayer)
            {
                for (int u = 0; u < players[i].units.Count; u++)
                {
                    players[i].units[u].GetComponent<Renderer>().enabled = false;
                    foreach (Transform tran in players[i].units[u].transform)
                    {
                        tran.gameObject.SetActive(false);
                    }
                }
            }
        }

        foreach (Unit unit in activePlayer.units)
        {
            unit.GetComponent<Renderer>().enabled = true;

            foreach (Transform tran in unit.transform)
            {
                tran.gameObject.SetActive(true);
            }
        }
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

        if (selectedUnit != null)
        {
            selectedUnit.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/DefaultShader");
        }
        
        //there are no units selected
        if (unit.playerID == activePlayer.playerID)
        {
            //the player is selecting a different unit, hide the menu
            if (selectedUnit != unit)
            {
                worldUI.gameObject.GetComponent<Canvas>().enabled = false;
            }            


            selectedUnit = unit;
            selectedUnit.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/WallThrough");
        }
    }


    /*
    * OnTileSelected 
    * 
    * callback when a tile is clicked on
    * 
    * @param Tiles tile - the tile that was selected
    * @returns void
    */
    public void OnTileSelected(Tiles tile)
    {

       
        //if a unit was already selected and an empty tile was selected
        if (selectedUnit != null)
        {
            //the unit cannot select another unit on the same team
            if (tile.unit == null || selectedUnit.playerID != tile.unit.playerID)
            {

                endTile = tile;

                //set-up the world UI
                worldUI.transform.position = new Vector3(endTile.pos.x, worldUI.transform.position.y, endTile.pos.z);
                worldUI.gameObject.GetComponent<Canvas>().enabled = true;

                //get the tile position of the unit
                Vector3 unitTilePos = selectedUnit.transform.position - Vector3.up * selectedUnit.transform.position.y;

                //get the position of the selected tile
                Vector3 tilePos = tile.pos;

                //relative vector to the tile
                Vector3 relative = tilePos - unitTilePos;

                float manhattanDistanceSqr = Mathf.Abs(relative.x) + Mathf.Abs(relative.z);
                manhattanDistanceSqr *= manhattanDistanceSqr;

                //reset the buttons
                worldUI.AttButton.SetActive(false);
                worldUI.MoveButton.SetActive(false);
                worldUI.SpcButton.SetActive(false);

                //because A* will consider this not passable
                startTile.unit = null;

                //get the path using A*
                List<Tiles> path = AStar.GetAStarPath(startTile, endTile);

                //reassign the unit reference
                startTile.unit = selectedUnit;

                //get the squared steps in the path
                float pathDistanceSqr = path.Count;
                pathDistanceSqr *= pathDistanceSqr;

                //can the unit attack the tile
                if (manhattanDistanceSqr <= selectedUnit.attackRange * selectedUnit.attackRange)
                {
                    worldUI.AttButton.SetActive(true);
                }

                //can the unit move to the tile, also a movement range of 0 means the path couldn't be found
                if (pathDistanceSqr <= selectedUnit.movementRange * selectedUnit.movementRange && pathDistanceSqr > 0.0f)
                {
                    worldUI.MoveButton.SetActive(true);
                }

                //can the unit apply a special move to the tile
                if (manhattanDistanceSqr <= selectedUnit.attackRange * selectedUnit.attackRange)
                {
                    worldUI.SpcButton.SetActive(true);
                }


            }

            //call the unit handling function if a unit was found on the tile
            else if (tile.unit != null)
            {
                startTile = tile;
                endTile = null;
                OnUnitSelected(tile.unit);
            }
        }
        else
        {
            //call the unit handling function if a unit was found on the tile
            if (tile.unit != null)
            {
                startTile = tile;
                OnUnitSelected(tile.unit);
            }
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
        selectedUnit.Execute(actionEvent, startTile, endTile);

        selectedUnit.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/DefaultShader");

        //deselect the unit
        selectedUnit = null;

        //deselect the tiles
        startTile = null;
        endTile = null;

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
