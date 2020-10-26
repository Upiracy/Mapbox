using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPressPlay()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void OnPressExit()
    {
        Application.Quit();
    }
}
