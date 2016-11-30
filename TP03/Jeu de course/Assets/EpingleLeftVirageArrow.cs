using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EpingleLeftVirageArrow : MonoBehaviour {

    public Scrollbar epingleGaucheArrow;
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
        timer += (Time.deltaTime) * 100f;
        if (showArrow)
            epingleGaucheArrow.size = (timer / 100f) % 1f;
        else
            epingleGaucheArrow.size = 0f;
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
