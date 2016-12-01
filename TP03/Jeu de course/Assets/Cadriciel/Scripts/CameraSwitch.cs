using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

    public Camera camJeu;
    public Camera camFin;

    // Use this for initialization
    void Start () {
        camJeu.enabled = true;
        camJeu.GetComponent<AudioListener>().enabled = true;
        camFin.enabled = false;
        camFin.GetComponent<AudioListener>().enabled = false;
    }
	
    void SwitchCam()
    {
        camJeu.enabled = false;
        camJeu.GetComponent<AudioListener>().enabled = false;
        camFin.enabled = true;
        camFin.GetComponent<AudioListener>().enabled = true;
        //camFin.GetComponentInParent<AutoCam>().enabled = false;
    }
}
