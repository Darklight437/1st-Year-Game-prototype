using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Inventory/Statistics", order = 1)]
public class Statistics : ScriptableObject
{
    //reference to the armour function
    public AnimationCurve armourCurve = AnimationCurve.Linear(0,0,4,50);

}