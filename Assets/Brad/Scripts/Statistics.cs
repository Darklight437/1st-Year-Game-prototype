using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Inventory/Statistics", order = 1)]
public class Statistics : ScriptableObject
{
    //reference to the armour function
    public AnimationCurve armourCurve = AnimationCurve.Linear(0,0,4,50);

    //amount of health gained when a healing tile is stepped on
    public float tileHealthGained = 500.0f;

    //amount of damage a trap tile does when stepped on
    public float trapTileDamage = 100.0f;

}