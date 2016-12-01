using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningIceSignalScript : MonoBehaviour {

    public Scrollbar warningSign;
    float timer = 0;
    bool showArrow;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        showArrow = false;

    }

    // Update is called once per frame
    void Update()
    {
        timer += (Time.deltaTime) ;
        if (showArrow)
        {
            if (timer % 1f < 0.5f)
            {
                warningSign.size = 1f;
            }
            else
                warningSign.size = 0f;
        }
            
        else
            warningSign.size = 0f;
    }

    public void activateArrow()
    {
        showArrow = true;
        timer = 0;
    }

    public void desactivateArrow()
    {
        showArrow = false;
    }
}
