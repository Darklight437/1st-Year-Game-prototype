using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AiPlayer
* child class of BasePlayer
* 
* object for a computer controller player, executes automated actions
* following a stratergy implemented by the behaviour tree 
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class AiPlayer : BasePlayer
{
    //automated reference to the game management component
    private GameManagment manager = null;

    //automated reference to the map
    private Map map = null;

    //fuzzy logic machine for making decisions and moves
    public FuzzyLogic logicMachine;

    //AI stats
    public float advanceImportance = 0.5f;
    public float fleeImportance = 0.5f;

    // Use this for initialization
    new void Start()
    {
        isHuman = false;
        manager = Object.FindObjectOfType<GameManagment>();
        map = Object.FindObjectOfType<Map>();
    }

    // Update is called once per frame
    new void Update()
    {

    }


    /*
    * UpdateTurn 
    * overrides BasePlayers' UpdateTurn()
    * 
    * called once per frame while the player is active
    * 
    * @returns void
    */
    public override void UpdateTurn()
    {
        Unit u = units[2];

        if (u.movementPoints == u.movementRange)
        {
            MovePiece(u, u.transform.position + new Vector3(0, 0, -3));
        }

        manager.OnNextTurn();
      
    }

    public void MovePiece(Unit target, Vector3 targetPosition)
    {


        manager.selectedUnit = target;
        manager.startTile = map.GetTileAtPos(target.transform.position);
        manager.endTile = map.GetTileAtPos(targetPosition);

        //execute a movement
        manager.OnActionSelected(1);
    }

}
