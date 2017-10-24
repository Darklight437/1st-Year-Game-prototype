using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class MoveCommand
* child class of UnitCommand
* 
* executes a Unit's movement routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class MoveCommand : UnitCommand
{


    /*
    * MoveCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param int x - the x coordinate of the target tile
    * @param int y - the y coordinate of the target tile
    */
    public MoveCommand(Unit u, VoidFunc scb, VoidFunc fcb, int x, int y) : base(u, scb, fcb, x, y)
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
        //the 3D target of the movement
        Vector3 target = new Vector3(tileX + 0.5f, 0.5f, tileY + 0.5f);

        Vector3 relative = target - unit.transform.position;

        if (relative.magnitude < 0.5f * Time.deltaTime)
        {
            unit.transform.position = target;
            successCallback();
        }
        else
        {
            unit.transform.position += relative.normalized * 0.5f * Time.deltaTime;
        }
    }
}
