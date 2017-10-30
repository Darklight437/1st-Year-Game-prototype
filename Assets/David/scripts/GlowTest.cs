using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTest : MonoBehaviour {

    public GameObject testModel = null;
    //private Color clear = Color.
     public bool UnitActive = false;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void GlowLikeABastard()
    {
        if(UnitActive)
        {
            testModel.GetComponent<Renderer>().material.SetFloat("_Outline", 0);
        }
        else
        {
            testModel.GetComponent<Renderer>().material.SetFloat("_Outline", 0.25f);
        }
        UnitActive = !UnitActive;



    }
}
