using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class UIBoot : MonoBehaviour {

    public Image title;
    public Image controleManette;
    public Image controleKeyboard;
    public Image instructions;
    private Color c;
    private float trspInc = 0f;
    private float timeStamp;
    private bool displayText = false;
    private bool displayKeyControl = false;
    private bool displayManControl = false;

    void Start()
    {
        c = title.GetComponent<Image>().color;
        c.a = trspInc;
        title.GetComponent<Image>().enabled = true;
        title.color = c;
        instructions.GetComponent<Image>().enabled = false;
        controleManette.GetComponent<Image>().enabled = false;
        controleKeyboard.GetComponent<Image>().enabled = false;
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
            if (CrossPlatformInputManager.GetButton("DispManette") && displayKeyControl == false && displayManControl == false)
            {
                title.GetComponent<Image>().enabled = false;
                instructions.GetComponent<Image>().enabled = false;
                controleManette.GetComponent<Image>().enabled = true;
                displayManControl = true;
            }
            if (CrossPlatformInputManager.GetButton("DispKey") && displayKeyControl == false && displayManControl == false)
            {
                title.GetComponent<Image>().enabled = false;
                instructions.GetComponent<Image>().enabled = false;
                controleKeyboard.GetComponent<Image>().enabled = true;
                displayKeyControl = true;
            }
            if (CrossPlatformInputManager.GetButton("RetourMenu"))
            {
                if (displayKeyControl)
                {
                    controleKeyboard.GetComponent<Image>().enabled = false;
                    displayKeyControl = false;
                    title.GetComponent<Image>().enabled = true;
                    instructions.GetComponent<Image>().enabled = true;
                }
                if (displayManControl)
                {
                    controleManette.GetComponent<Image>().enabled = false;
                    displayManControl = false;
                    title.GetComponent<Image>().enabled = true;
                    instructions.GetComponent<Image>().enabled = true;
                }
            }

            if (!displayKeyControl && !displayManControl)
            {
                if (timeStamp > 1f && displayText)
                {
                    displayText = !displayText;
                    instructions.GetComponent<Image>().enabled = displayText;
                    timeStamp = 0f;
                }
                if (timeStamp > .4f && !displayText)
                {
                    displayText = !displayText;
                    instructions.GetComponent<Image>().enabled = displayText;
                    timeStamp = 0f;
                }
                else timeStamp += Time.fixedDeltaTime;
            }
        }
	}
}
