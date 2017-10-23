using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    //list of references to players
    public List<Player> players = new List<Player>();

    //static reference to the statistics object
    public static Statistics stats;

    //reference to the active player
    private Player m_activePlayer = null;

    //reference to the camera movement script
    public CameraMovement cam = null;

    //ID of the active player
    public int turn = 0;

    //bool indicating if the game is in-between turns
    public bool transitioning = false;

	// Use this for initialization
	void Start ()
    {
        m_activePlayer = players[0];

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
        m_activePlayer = players[turn];
        m_activePlayer.CalculateKingPosition();

        cam.Goto(m_activePlayer.kingPosition, cam.transform.eulerAngles + new Vector3(0.0f, 180.0f, 0.0f), OnCameraFinished);

        transitioning = true;
    }


    public void OnCameraFinished()
    {
        transitioning = false;
    }
}
