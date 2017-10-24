using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {
    //David

    
    public enum eChangeSceneTo { MENU, PLAY };
    public eChangeSceneTo NextScene;

    public void SceneTest()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)NextScene)
        {
            SceneManager.LoadScene(((int)NextScene));
        }
    }
}
