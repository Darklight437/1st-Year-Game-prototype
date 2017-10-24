using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public enum eUIState {BASE, ACTIVEUNIT, ENDGAME, PAUSEMENU}
    public eUIState currUIState;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		//check what UI state is
        //switch on enum
        //enable / disable elements of ui & manage UI anims
	}
    void BaseState()
    {

    }
}
