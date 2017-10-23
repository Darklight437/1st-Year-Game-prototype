using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NextTurnButton : MonoBehaviour
{
    //David
    public bool RedTurn = true;
    public Color Red;
    public Color Blue;
    private GameObject ColorChangers;
    private Image Panel;
    void Start()
    {
        ColorChangers = GameObject.FindGameObjectWithTag("RedVSBlue");
        //Put an if on whose turn to determine default UI color
        Panel = ColorChangers.GetComponent<Image>();
        Panel.color = Red;
    }

   public void Click()
    {
        //toggle bool turn
        
        if(Panel.color == Red)
        {
            Panel.color = Blue;
        }
        else
        {
            Panel.color = Red;
        }
        
    }
	
}
