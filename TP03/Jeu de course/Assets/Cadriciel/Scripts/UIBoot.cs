using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBoot : MonoBehaviour {

    public Image title;
    public GUIText startText;
    private Color c;
    private float trspInc = 0f;
    private float timeStamp;
    private bool displayText = false;

    void Start()
    {
        c = title.GetComponent<Image>().color;
        startText.GetComponent<GUIText>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (c.a < 1)
        {
            trspInc += .005f;
            c.a = trspInc;
            title.color = c;
        }
        else
        {
            Debug.Log(timeStamp);
            if (timeStamp > 1f)
            {
                displayText = !displayText;
                startText.GetComponent<GUIText>().enabled = displayText;
                timeStamp = 0f;
            }
            else timeStamp += Time.fixedDeltaTime;
        }

	}
}
