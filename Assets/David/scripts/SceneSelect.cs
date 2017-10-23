using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {
    //David

    public Scene ChangeTo;
    public enum eChangeSceneTo { MENU, PLAY };
    public eChangeSceneTo NextScene;
    // Use this for initialization
    void Start ()
    {
        ChangeTo = SceneManager.GetSceneByBuildIndex((int)NextScene);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void SceneTest()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)eChangeSceneTo.MENU)
        {
            SceneManager.SetActiveScene(ChangeTo);
        }
    }
}
