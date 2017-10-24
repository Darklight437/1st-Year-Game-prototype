using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceManager : MonoBehaviour {
    //David

    public GameObject MoveButton;
    public GameObject AttButton;
    public GameObject SpcButton;

    RaycastHit hit;
    
    // Use this for initialization
    void Start ()
    {
        //MoveButton.SetActive(false);
        //AttButton.SetActive(false);
        //SpcButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);

        if (Input.GetMouseButtonDown(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //JumpToTile();
                Debug.Log("hit!");
            }

        }
    }

    //jumps the worldspace canvas to a location
    void JumpToTile()
    {
        RectTransform Canvas = gameObject.GetComponent<RectTransform>();
        Canvas.anchoredPosition3D = hit.transform.position;
    }


}
