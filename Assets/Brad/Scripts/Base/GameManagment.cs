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
    public List<BasePlayer> players = new List<BasePlayer>();

    //static reference to the statistics object
    public static Statistics stats = null;
    public Statistics statsReference = null;

    //reference to the map
    public Map map = null;

    //reference to the active player
    public BasePlayer activePlayer = null;

    //reference to the selected unit
    public Unit selectedUnit = null;

    //list of all walkable tiles that the slected unit can walk to
    public List<Tiles> movableTiles = new List<Tiles>();

    //list of all attackable tiles
    public List<Tiles> attackableTiles = new List<Tiles>();

    //list of all tiles that an enemy could attack you in
    public List<Tiles> dangerTiles = new List<Tiles>();

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

    //David's
    //reference to Main UI manager script
    /*
  * WorldUi is Depreciated
  * use the state switch of UIManager to 
  * control which buttons are visible
  * UIManager.ButtonState(UIManager.eCommandState.OFF);
  * turns off all buttons
  * other button states are based off the board
  * MSC etcetera
  * 
  * 
  */
    public UIManager UIManager = null;

 
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

        UIManager = GetComponent<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        activePlayer.UpdateTurn();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("pressed unit toggle");
            ToggleBetweenActiveUnits();

        }
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

        //remove all dead units
        foreach (BasePlayer p in players)
        {
            //iterate through all units, removing null references
            for (int i = 0; i < p.units.Count; i++)
            {
                //get the unit
                Unit unit = p.units[i];

                //check that the unit isn't a missing reference
                if (unit == null)
                { 
                    p.units.RemoveAt(i);
                    i--;
                }
                else
                {
                    //reset the real-time turn tracking
                    unit.movementPoints = unit.movementRange;
                    unit.hasAttacked = false;
                }
            }
        }

        //turn off the action menu
        
        UIManager.ButtonState(UIManager.eCommandState.OFF);
        if (selectedUnit != null)
        {
            //turn off the unit selection glow
           // selectedUnit.GetComponent<Renderer>().material.shader = Shader.Find("Custom/DefaultShader");
        }
		 
        //deselect the unit
        selectedUnit = null;

        //stop showing walkable tiles if thy where showing
        ToggleTileModifiersFalse();

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

    /*
   * TurnUnitsOff 
   * 
   * is called when player end there turns and goes through and turns off
   * all the non active player units and turns on all active player units
   * for FOW reasons
   * 
   * @param non
   * @returns void
   * @author Callum Dunstone
   */
    public void TurnUnitsOff()
    {
        //go through all non active players and turn off there units
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != activePlayer)
            {
                for (int u = 0; u < players[i].units.Count; u++)
                {

                  //  players[i].units[u].GetComponent<Renderer>().enabled = false;
                    players[i].units[u].GetComponent<Unit>().sightHolder.SetActive(false);
                    foreach (Transform tran in players[i].units[u].transform)
                    {
                        tran.gameObject.SetActive(false);
                    }
                }
            }
        }

        //go through all active player units and make sure they are active
        foreach (Unit unit in activePlayer.units)
        {
           // unit.GetComponent<Renderer>().enabled = true;
            unit.sightHolder.gameObject.SetActive(true);

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
           // selectedUnit.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/DefaultShader");
        }
        
        //there are no units selected
        if (unit.playerID == activePlayer.playerID)
        {
            
            //the player is selecting a different unit, hide the menu
            if (selectedUnit != unit)
            {
                //not intended
                UIManager.ButtonState(UIManager.eCommandState.OFF);
                UIManager.MenuPosition.SetActive(false);
            }

            //stop showing walkable tiles if thy where showing
            ToggleTileModifiersFalse();

            selectedUnit = unit;
            // selectedUnit.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/WallThrough");

            ToggleTileModifiersActive();
        }
    }

    /*
    * ToggleBetweenActiveUnits
    * void function
    * 
    * this goes through the active player units trying to find the next active unit
    *  
    * @param non
    * @returns void
    * @author Callum Dunstone
    */
    public void ToggleBetweenActiveUnits()
    {
        bool startSearch = false;

        if (selectedUnit == null)
        {
            startSearch = true;
        }

        //look throught the list of units in the active player until we get to the currently selected unit
        //then begin our search for the next inactive unit
        foreach (Unit unit in activePlayer.units)
        {
            if (selectedUnit != null)
            {
                if (unit == selectedUnit)
                {
                    startSearch = true;
                    continue;
                }
            }

            if (startSearch == true)
            {
                if (CheckIfStillActive(unit))
                {
                    OnTileSelected(map.GetTileAtPos(unit.transform.position));
                    Vector3 holder = new Vector3(selectedUnit.transform.position.x, 0.0f, selectedUnit.transform.position.z);
                    cam.Goto(holder, cam.transform.eulerAngles, null);
                    return;
                }
            }
        }

        //if no unit was found check the first half of the list for an inactive unit but if we wrap back round to the currently selected unit
        //break out as there are no non inactive units left
        foreach (Unit unit in activePlayer.units)
        {
            if(unit == selectedUnit)
            {
                return;
            }

            if (CheckIfStillActive(unit))
            {
                OnTileSelected(map.GetTileAtPos(unit.transform.position));
                Vector3 holder = new Vector3(selectedUnit.transform.position.x, 0.0f, selectedUnit.transform.position.z);
                cam.Goto(holder, cam.transform.eulerAngles, OnCameraFinished);
                return;
            }
        }
    }
    
    /*
    * CheckIfStillActive
    * bool function
    * 
    * checks if the unit can still walk or attack and returns true meaning they still have actions left
    *  
    * @param Unit - refrence to the unit we are checking if it can still do something
    * @returns bool
    * @author Callum Dunstone
    */
    public bool CheckIfStillActive(Unit unit)
    {
        if (unit.movementPoints > 0 || unit.hasAttacked == false)
        {
            if (unit.commands.Count == 0)
            {
                return true;
            }
        }

        return false;
    }


    /*
    * ToggleWalkableTilesActive 
    * 
    * tells all tiles held in movableTiles to show that they are
    * movable
    * 
    * @param non
    * @returns void
    * @author Callum Dunstone
    */
    public void ToggleTileModifiersActive()
    {
        //gather and show new walkable tiles
        if (selectedUnit != null && selectedUnit.movementPoints > 0)
        {
            List<Tiles> holder = GetArea.GetAreaOfMoveable(map.GetTileAtPos(selectedUnit.transform.position), selectedUnit.movementPoints, map);

            foreach (Tiles tile in holder)
            {
                movableTiles.Add(tile);
            }

            foreach (Tiles tile in movableTiles)
            {
                if (tile.walkableHighLight.gameObject.activeSelf == false)
                {
                    tile.walkableHighLight.gameObject.SetActive(true);
                }
            }
        }

        //gather and show all attackabe tiles
        if (selectedUnit != null && selectedUnit.hasAttacked == false && selectedUnit != null)
        {
            List<Tiles> holder2 = GetArea.GetAreaOfAttack(map.GetTileAtPos(selectedUnit.transform.position), selectedUnit.attackRange, map);

            foreach (Tiles tile in holder2)
            {
                attackableTiles.Add(tile);
            }

            foreach (Tiles tile in attackableTiles)
            {
                if (tile.attackRangeHighLight.gameObject.activeSelf == false)
                {
                    tile.attackRangeHighLight.gameObject.SetActive(true);
                }
            }
        }

        if (dangerTiles.Count > 0)
        {
            Debug.Log(dangerTiles.Count);

            for (int i = 0; i < dangerTiles.Count; i++)
            {
                dangerTiles[i].dangerZoneRangeHighLight.gameObject.SetActive(true);
            }
        }
    }



    /*
    * ToggleTileModifiersFalse 
    * 
    * tells all tiles held in movableTiles to stop showing that they are
    * walable and attackable
    * 
    * @param non
    * @returns void
    * @author Callum Dunstone
    */
    public void ToggleTileModifiersFalse()
    {
        foreach (Tiles tile in movableTiles)
        {
            if (tile.walkableHighLight.gameObject.activeSelf == true)
            {
                tile.walkableHighLight.gameObject.SetActive(false);
            }
        }

        foreach (Tiles tile in attackableTiles)
        {
            if (tile.attackRangeHighLight.gameObject.activeSelf == true)
            {
                tile.attackRangeHighLight.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < dangerTiles.Count; i++)
        {
            dangerTiles[i].dangerZoneRangeHighLight.SetActive(false);
        }

        movableTiles.Clear();
        attackableTiles.Clear();
        dangerTiles.Clear();
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

        //don't do anything if the UI was pressed
        if (uiPressed)
        {
            uiPressed = false;
            return;
        }

        //if a unit was already selected and an empty tile was selected
        if (selectedUnit != null)
        {
            //the unit cannot select another unit on the same team
            if (tile.unit == null || selectedUnit.playerID != tile.unit.playerID)
            {

                endTile = tile;

                //David 
                //gonna change the Worldspace UI to screenspace set the position relative to the click
                //set-up the world UI


                //turn on UI
                UIManager.MenuPosition.SetActive(true);

                //set UI to mouse position
                UIManager.MenuPosition.GetComponent<RectTransform>().position = Input.mousePosition;
                

                
                //get the tile position of the unit
                Vector3 unitTilePos = selectedUnit.transform.position - Vector3.up * selectedUnit.transform.position.y;

                //get the position of the selected tile
                Vector3 tilePos = tile.pos;

                //relative vector to the tile
                Vector3 relative = tilePos - unitTilePos;

                float manhattanDistanceSqr = Mathf.Abs(relative.x) + Mathf.Abs(relative.z);
                manhattanDistanceSqr *= manhattanDistanceSqr;

                //reset the buttons
               
                // worldUI.AttButton.SetActive(false);
                // worldUI.MoveButton.SetActive(false);
                // worldUI.SpcButton.SetActive(false);

                //because A* will consider this not passable
                startTile.unit = null;

                //get the path using A*
                List<Tiles> path = AStar.GetAStarPath(startTile, endTile);

                //reassign the unit reference
                startTile.unit = selectedUnit;

                //get the squared steps in the path
                float pathDistanceSqr = path.Count;
                pathDistanceSqr *= pathDistanceSqr;

                //Buttonshow Block 

                bool move;
                bool attack;
                bool special;

                //can the unit attack the tile
                if (manhattanDistanceSqr <= selectedUnit.attackRange * selectedUnit.attackRange && !selectedUnit.hasAttacked)
                {
                    //worldUI.AttButton.SetActive(true);
                    attack = true;
                }
                else
                {
                    //worldUI.AttButton.SetActive(false);
                    attack = false;
                }

                //can the unit move to the tile, also a movement range of 0 means the path couldn't be found
                if (pathDistanceSqr <= selectedUnit.movementPoints * selectedUnit.movementPoints && pathDistanceSqr > 0.0f)
                {
                    //worldUI.MoveButton.SetActive(true);
                    move = true;
                }
                else
                {
                    //worldUI.MoveButton.SetActive(false);
                    move = false;
                }

                //can the unit apply a special move to the tile
                if (manhattanDistanceSqr <= selectedUnit.attackRange * selectedUnit.attackRange && !selectedUnit.hasAttacked)
                {
                    //worldUI.SpcButton.SetActive(true);
                    special = true;
                }
                else
                {
                    //worldUI.SpcButton.SetActive(false);
                    special = true;
                }
                //sets the button state in the UI manager to show the appropriate buttons
                UIManager.ButtonState(getvalidButtons(move, attack, special));
                

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

        if (tile.unit != null && tile.unit.playerID != activePlayer.playerID)
        {
            ToggleTileModifiersFalse();

            List<Tiles> holder = GetArea.GetAreaOfAttack(tile, tile.unit.movementRange + tile.unit.attackRange, map);

            for (int i = 0; i < holder.Count; i++)
            {
                dangerTiles.Add(holder[i]);
            }

            ToggleTileModifiersActive();

        }
    }
    

    /*
     * getValidButtons
     * 
     * @param bool move, bool attack, bool special: wether each button should be shown
     * @returns the correct enum forthe current state of button combinations
     * @author David Passlow
     */
   private UIManager.eCommandState getvalidButtons(bool mov, bool att, bool spec)
    {
        if (mov && spec)
        {
            return UIManager.eCommandState.MSC;
        }
        if (att && spec)
        {
            return UIManager.eCommandState.ASC;
        }
        if (mov)
        {
            return UIManager.eCommandState.MC;
        }
        if (att)
        {
            return UIManager.eCommandState.AC;
        }
        if (spec)
        {
            return UIManager.eCommandState.SC;
        }
        if (!mov && !spec && !att)
        {
            return UIManager.eCommandState.C;
        }
        if (mov && att)
        {
            return UIManager.eCommandState.AC;
        }


        return UIManager.eCommandState.C;
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
        UIManager.ButtonState(UIManager.eCommandState.OFF);

        //execute the action
        selectedUnit.Execute(actionEvent, startTile, endTile, OnActionFinished);

        //stop showing walkable and attackable tiles tiles
        ToggleTileModifiersFalse();

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

        //damage each unit at the start of the turn if it is standing on a trap tile
        foreach (Unit u in activePlayer.units)
        {
            Tiles currentTile = map.GetTileAtPos(u.transform.position);
            
            if (currentTile.tileType == eTileType.DAMAGE)
            {
                u.Defend(GameManagment.stats.trapTileDamage);
            }
        }
    }


    /*
    * OnActionFinished 
    * 
    * callback when a unit finishes performing an action
    * that was directly commanded by the player
    * 
    * @returns void
    */
    public void OnActionFinished()
    {
        activePlayer.CalculateKingPosition();

        //search for the king
        King king = null;

        for (int i = 0; i < activePlayer.units.Count; i++)
        {
            //store in temp variable
            Unit unit = activePlayer.units[i];

            //if the unit is a king and hasn't already been removed
            if (unit != null && unit is King)
            {
                king = unit as King;
                break;
            }
        }

        //there is no king because they have been killed
        if (king == null)
        {
            OnKingKilled(turn);
        }

        int unitsAdjacentToKing = 0;

        //apply king buffs
        for (int i = 0; i < activePlayer.units.Count; i++)
        {
            //store in temp variable
            Unit unit = activePlayer.units[i];

            //if the unit hasn't already been removed
            if (unit != null)
            {
                //reset the multiplier
                unit.attackMultiplier = 1.0f;

                //relative vector from the unit to the king
                Vector3 relative = king.transform.position - unit.transform.position;

                //get the manhattan distance
                float manhattan = Mathf.Abs(relative.x) + Mathf.Abs(relative.z);

                //the king gets buffed from this
                if (manhattan <= 1.0f)
                {
                    unitsAdjacentToKing++;
                }

                //close enough to the king to be buffed offensively
                if (manhattan <= king.adjacentUnitRange)
                {
                    unit.attackMultiplier = 1.0f + king.flatDamageRatio;
                }
            }
        }

        //reset the multiplier
        king.attackMultiplier = 1.0f;

        //buff the king depending on how many units are near it
        if (unitsAdjacentToKing > 0 && unitsAdjacentToKing <= king.kingDamageRatios.GetLength(0))
        {
            king.attackMultiplier = 1.0f + king.kingDamageRatios[unitsAdjacentToKing - 1];
        }
    }


    /*
    * KillAll 
    * 
    * forces all units on all teams to die
    * 
    * @returns void 
    */
    public void KillAll()
    {
        //iterate through all players
        foreach (BasePlayer p in players)
        {
            //iterate through all of the units, killing each
            for (int i = 0; i < p.units.Count; i++)
            {
                //store in temp value for readability
                Unit unit = p.units[i];

                //check for a null reference
                if (unit != null)
                {
                    //get the tile that the unit is standing on
                    Tiles currentTile = map.GetTileAtPos(unit.transform.position);

                    unit.Execute(eActionType.DEATH, currentTile, null, null);
                }
            }
        }
    }


    /*
    * OnKingKilled
    * 
    * called when a king is slain 
    * 
    * @param int playerID - the id of the player that had their king killed
    * @returns void
    */
    public void OnKingKilled(int playerID)
    {
        Debug.Log(playerID.ToString() + "'s King has been slain!");
        //sets the UI to display the victory splashscreen
        UIManager.currUIState = UIManager.eUIState.ENDGAME;
    }


}
