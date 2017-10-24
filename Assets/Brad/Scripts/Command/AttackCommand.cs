using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AttackCommand
* child class of UnitCommand
* 
* executes a Unit's attack routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class AttackCommand : UnitCommand
{


    /*
    * AttackCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param int x - the x coordinate of the target tile
    * @param int y - the y coordinate of the target tile
    */
    public AttackCommand(Unit u, VoidFunc scb, VoidFunc fcb, int x, int y) : base(u, scb, fcb, x, y)
    {
    }


    /*
    * Update
    * overrides UnitCommand's Update()
    * 
    * called once per frame while the command is active
    * 
    * @returns void
    */
    public override void Update()
    {
        
    }
}
