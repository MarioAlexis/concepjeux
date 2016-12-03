using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System.Collections;

public class StartScreenManager : MonoBehaviour 
{
    public Image instructions;
    private bool canEnterGame = false;

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
	}

	// Update is called once per frame
	void Update () 
	{
        if (instructions.GetComponent<Image>().enabled == true) canEnterGame = true;
		if (CrossPlatformInputManager.GetButton("Submit") && canEnterGame == true)
		{
            SceneManager.LoadScene("test_terrain");
        }
	}
}
