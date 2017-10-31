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
    //reference to the map
    public Map map = null;

    //list of tiles to follow
    private List<Tiles> m_tilePath = null;


    public void Start()
    {
       
    }


    /*
    * MoveCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public MoveCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {

        //find the map component
        map = GameObject.FindObjectOfType<Map>();
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
        if (m_tilePath == null)
        {
            //get the start and end of the path
            Tiles startingTile = map.GetTileAtPos(unit.transform.position);

            startingTile.unit = null;

            //get the tile path to follow
            m_tilePath = AStar.GetAStarPath(startingTile, endTile);

            //the path is clear
            if (m_tilePath.Count > 0)
            {
                startingTile.unit = null;
                endTile.unit = unit;
            }
            else
            {
                //the path failed
                startingTile.unit = unit;
                failedCallback();
                return;
            }
        }

        //check if there is still a path to follow
        if (m_tilePath.Count > 0)
        {
            //get the next position to go to
            Tiles nextTile = m_tilePath[0];

            //the 3D target of the movement
            Vector3 target = new Vector3(nextTile.pos.x, 0.5f, nextTile.pos.z);

            Vector3 relative = target - unit.transform.position;

            if (relative.magnitude < 3.0f * Time.deltaTime)
            {
                //this is a healing tile, heal the unit
                if (nextTile.IsHealing)
                {
                    unit.Heal(GameManagment.stats.tileHealthGained);
                }
                //this is a trap tile, it could kill the unit
                if (nextTile.tileType == eTileType.DAMAGE)
                {
                    unit.Defend(GameManagment.stats.trapTileDamage);
                }

                unit.transform.position = target;
                m_tilePath.RemoveAt(0);
            }
            else
            {
                unit.transform.position += relative.normalized * 3.0f * Time.deltaTime;
            }
        }
        else
        {
            successCallback();
            return;
        }
    }
}
