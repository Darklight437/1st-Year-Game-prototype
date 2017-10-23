using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    //function type for parsing a unit
    public delegate void UnitFunc(Unit unit);

    //list of references to players
    public List<Player> players = new List<Player>();

    //static reference to the statistics object
    public static Statistics stats = null;
    public Statistics statsReference = null;

    //reference to the active player
    public Player activePlayer = null;

    //reference to the camera movement script
    public CameraMovement cam = null;

    //ID of the active player
    public int turn = 0;

    //bool indicating if the game is in-between turns
    public bool transitioning = false;

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
        if (transitioning)
        {
            return;
        }

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
        Debug.Log(unit.gameObject.name);
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
        Debug.Log(x.ToString() + ", " + y.ToString());
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
