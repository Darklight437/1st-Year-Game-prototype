using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTest : MonoBehaviour {

    public GameObject testModel;
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
        testModel.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }
}
