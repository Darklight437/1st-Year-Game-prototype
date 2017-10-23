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

    private float tempTimer = 0.0f;
    private bool timing = true;

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
        //TEMPORARY
        if (!timing)
        {
            return;
        }

        tempTimer += Time.deltaTime;

        if (tempTimer > 10.0f)
        {
            OnNextTurn();
            tempTimer = 0.0f;
        }
	}


    public void OnNextTurn()
    {
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

        timing = false;
    }


    public void OnCameraFinished()
    {
        timing = true;
    }
}
