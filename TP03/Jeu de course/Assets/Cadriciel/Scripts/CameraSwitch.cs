using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

    public Camera camJeu;
    public Camera camFin;
    public Canvas canvasToDisable1;
    public Canvas canvasToDisable2;
    public Canvas canvasToDisable3;
    public Canvas canvasToDisable4;
    public Canvas canvasToDisable5;


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
        canvasToDisable1.enabled = false;
        canvasToDisable2.enabled = false;
        canvasToDisable3.enabled = false;
        canvasToDisable4.enabled = false;
        canvasToDisable5.enabled = false;
    }
}
