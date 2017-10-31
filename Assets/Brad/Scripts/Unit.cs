using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    //id of the player that owns this unit
    public int playerID = 0;

    //attributes
    public int movementRange = 0;
    public int attackRange = 0;
    public int armour = 0;

    public float damage = 0.0f;
    public float maxHealth = 1000.0f;
    public float health = 1000.0f;
    public float AOV = 1.0f;


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

    //good bye BB

    //David
    //the Health bar image reference
    public RectTransform hpBar = null;

    //the health bar number reference
    public Text hpNum = null;

    //reference to the UI armour bar
    public SpriteRenderer armourBar = null;


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
        HealthUpdate();
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
            obj.GetComponent<Sight>().myUnit = this;
        }
    }

    /*
    * Attack 
    * virtual function
    * 
    * calculates the damage done to a target
    * 
    * @param Unit target - the unit to apply the damage to
    * @param float multiplier - multiplys the damage
    * @returns void
    */
    public virtual void Attack(Unit target, float multiplier = 1.0f)
    {
        target.Defend(damage * multiplier);
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
        //armour to add (only added if this unit is inside a defensive tile
        int additionalArmour = 0;

        //get the tile that the unit is standing on
        Tiles currentTile = GameObject.FindObjectOfType<Map>().GetTileAtPos(transform.position);

        //defensive tiles reduce damage further
        if (currentTile.tileType == eTileType.DEFENSE)
        {
            additionalArmour++;
        }

        //calculate the damage reduction
        float armourScalar = 1 - GameManagment.stats.armourCurve.Evaluate(armour + additionalArmour) * 0.01f;

        //the armour scalar affects the damage output
        health -= damage * armourScalar;

        Debug.Log(gameObject.name + " was attacked for " + (damage * armourScalar).ToString() + " damage.");

        //has the unit died from the hit
        if (health <= 0.0f)
        {
            health = 0.0f;
            Execute(GameManagment.eActionType.DEATH, currentTile, null);
        }
    }



    /*
    * Heal 
    * virtual function
    * 
    * recieves healing
    * 
    * @param float points - the amount of health healed
    * @return void
    */
    public virtual void Heal(float points)
    {
        //add the points
        health += points;
        
        //clamp the max health
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log(gameObject.name + " was healed for " + points.ToString() + " health.");
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
        return commands.Count > 0 && health > 0.0f;
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

    /*  
    *   HealthUpdate
    *   
    *   runs each update to show the health of the unit in a bar & text
    *   
    *   @returns void
    */  
    public virtual void HealthUpdate()
    {


        Vector2 shield1 = new Vector2(2.44f, 2.5f);
        Vector2 shield2 = new Vector2(4.89f, 2.5f);
        Vector2 shield3 = new Vector2(7.3f, 2.5f);
        Vector2 shield4 = new Vector2(9.78f, 2.5f);

        switch(armour)
        {
            case 0:
                armourBar.size = new Vector2(0f, 2.5f);
                break;

            case 1:
                armourBar.size = shield1;
                break;

            case 2:
                armourBar.size = shield2;
                break;

            case 3:
                armourBar.size = shield3;
                break;

            case 4:
                armourBar.size = shield4;
                break;
        }

        Vector3 tempVect = hpBar.localScale;

        tempVect.x = health / maxHealth;

        hpBar.localScale = tempVect;

        hpNum.text = (int)health + " / " + (int)maxHealth;

    }
}
