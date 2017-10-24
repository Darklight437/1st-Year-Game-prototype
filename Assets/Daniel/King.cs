using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class King
* child class of Unit
* 
* the most important unit in the game, if the king dies the player loses
* 
* author: Daniel Witt, Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class King : Unit
{

    /*
    * Execute 
    * overrides function Unit's Execute(GameManagment.eActionType actionType, int tileX, int tileY)
    * 
    * adds a command of the specified type to the unit
    * 
    * @param GameManagement.eActionType actionType - the type of action to execute
    * @param int tileX - the x index of the 2D map array
    * @param int tileY - the y index of the 2D map array
    * @returns void
    */
    public override void Execute(GameManagment.eActionType actionType, int tileX, int tileY)
    {
        //movement command
        if (actionType == GameManagment.eActionType.MOVEMENT)
        {
            MoveCommand mc = new MoveCommand(this, OnCommandFinish, OnCommandFailed, tileX, tileY);

            commands.Add(mc);
        }
    }

}
