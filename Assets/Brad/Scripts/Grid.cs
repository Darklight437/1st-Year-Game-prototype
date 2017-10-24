using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void CalculatePosition(Vector3 hitPoint, out int x, out int y)
    {
        x = Mathf.FloorToInt(hitPoint.x);
        y = Mathf.FloorToInt(hitPoint.z);
    }
}
