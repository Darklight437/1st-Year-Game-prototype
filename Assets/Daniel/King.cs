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
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    * @returns void
    */
    public override void Execute(GameManagment.eActionType actionType, Tiles st, Tiles et)
    {
        //movement command
        if (actionType == GameManagment.eActionType.MOVEMENT)
        {
            MoveCommand mc = new MoveCommand(this, OnCommandFinish, OnCommandFailed, st, et);

            commands.Add(mc);
        }
        //attack command
        else if (actionType == GameManagment.eActionType.ATTACK)
        {
            AttackCommand ac = new AttackCommand(this, OnCommandFinish, OnCommandFailed, st, et);

            ac.attackTimer = attackTime;

            commands.Add(ac);
        }
        //dying command
        else if (actionType == GameManagment.eActionType.DEATH)
        {
            DeathCommand dc = new DeathCommand(this, OnCommandFinish, null, st, null);

            dc.deathTimer = deathTime;

            commands.Add(dc);
        }
    }

}
