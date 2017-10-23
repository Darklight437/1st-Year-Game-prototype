﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    //list of references to players
    public List<Player> players = new List<Player>();

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
	}
	
	// Update is called once per frame
	void Update ()
    {
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
        turn++;
        
        //wrap around the turn id
        if (turn > players.Count - 1)
        {
            turn = 0;
        }

        //set the active player
        m_activePlayer = players[turn];
        m_activePlayer.CalculateKingPosition();

        cam.Goto(m_activePlayer.kingPosition, OnCameraFinished);

        timing = false;
    }


    public void OnCameraFinished()
    {
        timing = true;
    }
}
