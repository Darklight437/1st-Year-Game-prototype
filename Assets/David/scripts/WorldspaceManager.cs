using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceManager : MonoBehaviour {
    //David

    public GameObject MoveButton;
    public GameObject AttButton;
    public GameObject SpcButton;

	// Use this for initialization
	void Start ()
    {
        MoveButton.SetActive(false);
        AttButton.SetActive(false);
        SpcButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
