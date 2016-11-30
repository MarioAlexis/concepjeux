using UnityEngine;
using System.Collections;

public class showVirageArrowGauche : MonoBehaviour {

    public LeftArrowSignalScript arrow;
    public Collider playerOneBodyCollider;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerOneBodyCollider)
        {
            arrow.activateArrow();
        }
        //Debug.Log("Entering Zone");
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerOneBodyCollider)
        {
            arrow.desactivateArrow();
        }
        //Debug.Log("Leaving Zone");
    }
}
