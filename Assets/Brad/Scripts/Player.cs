﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //list of all of the units the player owns
    public List<Unit> units = new List<Unit>();

    //position of the king, this is where the camera will go when the turn switches
    public Vector3 kingPosition = Vector3.zero;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    /*
    * LinkIDs 
    * 
    * sets the local IDs of every unit owned
    * by this player
    * 
    * @param int ID - the ID of the player
    * @returns void
    */
    public void LinkIDs(int ID)
    {
        //get the size of the units array once
        int unitsCount = units.Count;

        //iterate through all of the units, setting their IDs
        for (int i = 0; i < unitsCount; i++)
        {
            units[i].playerID = ID;
        }
    }


    /*
    * CalculateKingPosition
    * 
    * searches for the king and then sets a local variable
    * containing it's position
    * 
    * @returns voide
    */
    public void CalculateKingPosition()
    {
        //get the size of the units array once
        int unitsCount = units.Count;

        //iterate through all of the units, looking for the king
        for (int i = 0; i < unitsCount; i++)
        {
            //get the reference once (performance and readability)
            Unit unit = units[i];

            //check if the unit is the king
            if (unit is King)
            {
                //set the king position variable of the player
                kingPosition = new Vector3(unit.transform.position.x, 0.0f, unit.transform.position.z);
                return;
            }
        }
    }
}
