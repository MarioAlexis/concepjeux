using UnityEngine;
using System.Collections;

public class changeSignalToRight : MonoBehaviour {

    public RaceManager raceManager;

    public Collider playerOneBodyCollider;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other == playerOneBodyCollider)
        {
            raceManager.changeTurnSignal("Right");
        }


    }
}
