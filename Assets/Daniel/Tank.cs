﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* class Tank
* child class of Unit
* 
* slow moving unit with lots of health
* 
* author: Daniel Witt, Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class Tank : Unit
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
        //ability command (special attack)
        else if (actionType == GameManagment.eActionType.SPECIAL)
        {
            TileModifierCommand tmc = new TileModifierCommand(this, OnCommandFinish, null, st, et);

            tmc.modifyType = TileModifierCommand.eModifyType.DEFENSE;

            commands.Add(tmc);
        }
    }
}
