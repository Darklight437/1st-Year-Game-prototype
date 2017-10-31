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
    public float maxHealth = 1000.0f;
    public float health = 1000.0f;
    public float AOV = 1.0f;

    //David's
    //random Comment

    //time it takes the unit to apply damage after "attacking"
    public float attackTime = 1.0f;

    //time it takes the unit to be completely deleted after "dying"
    public float deathTime = 1.0f;

    //container of commands to execute
    public List<UnitCommand> commands = new List<UnitCommand>();

    //holds all the GameObjects that makes up the units area of vision
    public List<GameObject> aovOBJ = new List<GameObject>();

    //the game prefab that is used to make up the units area of sight
    public GameObject sightPrefab;

    //David
    //the Health bar canvas reference
    public GameObject hpBar = null;

    // Use this for initialization
    void Start()
    {
        //get the tile that the unit is standing on
        Tiles currentTile = GameObject.FindObjectOfType<Map>().GetTileAtPos(transform.position);

        //set the unit space to this
        currentTile.unit = this;

        //create the gameobjs that make up the units area of vision
        CreateAOVOBJ();
    }

    // Update is called once per frame
    void Update()
    {
        if (commands.Count > 0)
        {
            commands[0].Update();
        }
    }

    private void CreateAOVOBJ()
    {
        for (int i = 0; i <= AOV; i++)
        {
            GameObject obj = Instantiate(sightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3((i * 2) + 1, 1, (AOV * 2) + 1 - (i * 2));
            aovOBJ.Add(obj);
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

        Debug.Log(gameObject.name + " was attacked for " + (damage * (1 - armourScalar)).ToString() + " damage.");

        //has the unit died from the hit
        if (health <= 0.0f)
        {
            health = 0.0f;

            //get the tile that the unit is standing on
            Tiles currentTile = GameObject.FindObjectOfType<Map>().GetTileAtPos(transform.position);

            Execute(GameManagment.eActionType.DEATH, currentTile, null);
        }
    }


    /*
    * Execute 
    * virtual function
    * 
    * base function for adding commands to the unit
    * 
    * @param GameManagement.eActionType actionType - the type of action to execute
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    * @returns void
    */
    public virtual void Execute(GameManagment.eActionType actionType, Tiles st, Tiles et)
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
