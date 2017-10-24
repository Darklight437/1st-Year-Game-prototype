using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelection : MonoBehaviour
{
    //automated reference to the manager and input
    private GameManagment manager = null;
    private CustomInput input = null;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindObjectOfType<GameManagment>();
        input = GameObject.FindObjectOfType<CustomInput>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //detect mouse clicks
		if (Input.GetMouseButtonUp(0))
        {
            if (manager.uiPressed)
            {
                manager.uiPressed = false;
            }
            else
            {


                //get a ray originating from the mouse pointing forwards
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                //data from the raycast
                RaycastHit hitInfo;

                //check if the raycast hit anything
                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    //get the object that the raycast hit
                    GameObject hitObject = hitInfo.collider.gameObject;

                    //get the unit component (null if there isn't one)
                    Unit unit = hitObject.GetComponent<Unit>();

                    //get the grid component (null if there isn't one)
                    Grid grid = hitObject.GetComponent<Grid>();

                    if (unit != null)
                    {
                        manager.OnUnitSelected(unit);
                    }

                    if (grid != null)
                    {
                        //declare the position variables
                        int x, y;

                        grid.CalculatePosition(hitInfo.point, out x, out y);

                        manager.OnTileSelected(x, y);
                    }
                }
            }
        }
	}
}
