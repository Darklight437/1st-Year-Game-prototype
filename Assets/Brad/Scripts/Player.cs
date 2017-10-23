using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //list of all of the units the player owns
    public List<Unit> units = new List<Unit>();

    //position of the king, this is where the camera will go when the turn switches
    public Vector3 kingPosition = Vector3.zero;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
