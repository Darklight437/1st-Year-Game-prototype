using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* class UnitCommand
* 
* base class for Unit action routine
* 
* eg. moving from one location to another, attacking, animations
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class UnitCommand
{

    //reference to the unit that owns this command
    public Unit unit = null;

    //delegate type
    public delegate void VoidFunc();

    //function reference to call when complete
    public VoidFunc successCallback = null;

    //function reference to call when failed
    public VoidFunc failedCallback = null;

    //target
    public int tileX = 0;
    public int tileY = 0;

    /*
    * UnitCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * * @param VoidFunc fcb - the callback to use when failed
    * @param int x - the x coordinate of the target tile
    * @param int y - the y coordinate of the target tile
    */
    public UnitCommand(Unit u, VoidFunc scb, VoidFunc fcb, int x, int y)
    {
        unit = u;
        successCallback = scb;
        failedCallback = scb;
        tileX = x;
        tileY = y;
    }


    /*
    * Update
    * virtual function
    * 
    * called once per frame when active
    * 
    * @returns void
    */
    public virtual void Update()
    {
        
    }
	
}
