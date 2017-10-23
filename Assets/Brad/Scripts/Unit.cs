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

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
}
