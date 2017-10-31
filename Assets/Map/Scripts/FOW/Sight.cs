using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    //refrence to the unit this sight belongs to
    public Unit myUnit;
    
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Renderer>().enabled == false)
        {
            other.GetComponent<Renderer>().enabled = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Unit>().playerID != myUnit.playerID)
        {
            other.GetComponent<Renderer>().enabled = false;
        }
    }
}
