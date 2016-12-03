using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenManager : MonoBehaviour 
{
    public GUIText text;
    private bool canEnterGame = false;

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
	}

	// Update is called once per frame
	void Update () 
	{
        if (text.GetComponent<GUIText>().enabled == true) canEnterGame = true;
		if (Input.GetKeyDown(KeyCode.Return) && canEnterGame == true)
		{
            SceneManager.LoadScene("test_terrain");
        }
	}
}
