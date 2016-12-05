using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MudEffect : MonoBehaviour {

    public Image mudImage;
    private Color color;
    private float trspInc = 0f;
    private bool mudDisplayed;
    private bool mudBeingDisplayed;
    private float timeMud = 0f;

    // Use this for initialization
    void Start () {
        color = mudImage.GetComponent<Image>().color;
        color.a = trspInc;
        mudImage.GetComponent<Image>().enabled = true;
        mudImage.color = color;
    }

    void FixedUpdate()
    {
        if (mudDisplayed)
        {
            if (color.a > 0 && timeMud >= 4f)
            {
                trspInc -= .002f;
                color.a = trspInc;
                mudImage.color = color;
            }
            else timeMud += Time.fixedDeltaTime;
            if (color.a <= 0) mudDisplayed = false;
        }
        if (mudBeingDisplayed)
        {
            if (color.a <= 1)
            {
                trspInc += .2f;
                color.a = trspInc;
                mudImage.color = color;
            }
            else
            {
                mudBeingDisplayed = false;
                mudDisplayed = true;
            }
        }
    }
	
    void OnTriggerEnter(Collider car)
    {
        if (car.transform.parent.transform.parent.gameObject.name == "Joueur 1")
        {
            mudBeingDisplayed = true;
        }
    }
}
