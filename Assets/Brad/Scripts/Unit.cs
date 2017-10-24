using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    //id of the player that owns this unit
    public int playerID = 0;

    //attributes
    public int attackRange = 0;
    public int movementRange = 0;
    public int armour = 0;

    public float damage = 0.0f;
    public float health = 1000.0f;
    public float AOV = 1.0f;

    //container of commands to execute
    public List<UnitCommand> commands = new List<UnitCommand>();

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (commands.Count > 0)
        {
            commands[0].Update();
        }
    }

    /*
    * Attack 
    * virtual function
    * 
    * calculates the damage done to a target
    * 
    * @param Unit target - the unit to apply the damage to
    * @returns void
    */
    public virtual void Attack(Unit target)
    {
        target.Defend(damage);
    }



    /*
    * Defend 
    * virtual function
    * 
    * recieves damage and modifies the base value
    * 
    * @param float damage - the amount of damage recieved
    * @returns void
    */
    public virtual void Defend(float damage)
    {
        //calculate the damage reduction
        float armourScalar = GameManagment.stats.armourCurve.Evaluate(damage) * 0.01f;

        //the armour scalar affects the damage output
        health -= damage * (1 - armourScalar);
    }


    /*
    * Execute 
    * virtual function
    * 
    * base function for adding commands to the unit
    * 
    * @param GameManagement.eActionType actionType - the type of action to execute
    * @param int tileX - the x index of the 2D map array
    * @param int tileY - the y index of the 2D map array
    * @returns void
    */
    public virtual void Execute(GameManagment.eActionType actionType, int tileX, int tileY)
    {

    }


    /*
    * isDead 
    *
    * checks if the unit has positive health
    * 
    * @returns bool - the result of the check (positive if dead)
    */
    public bool isDead()
    {
        return health > 0.0f;
    }


    /*
    * IsBusy 
    * 
    * checks if the unit is still executing UnitCommands
    * 
    * @returns bool - the result of the check (positive if still running)
    */
    public bool IsBusy()
    {
        return commands.Count > 0;
    }


    /*
    * OnCommandFinish
    * 
    * called when the unit's latest command finishes
    * 
    * @returns void
    */
    public void OnCommandFinish()
    {
        //remove the latest command
        commands.RemoveAt(0);
    }


    /*
   * OnCommandFailed
   * 
   * called when the unit's latest command fails
   * 
   * @returns void
   */
    public void OnCommandFailed()
    {
        //remove all commands as they may depend on the one that just failed being successful
        commands.Clear();
    }
}
