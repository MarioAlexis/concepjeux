using UnityEngine;
using System.Collections;

public class scorePointsOnJump : MonoBehaviour {


    public RaceManager raceManager;

    public Collider playerOneBodyCollider;

    public int numberOfPointsToAddOnJUmp = 10;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        // DoActivateTrigger();
        if (other == playerOneBodyCollider)
        {
            Debug.Log("Jump !");
            raceManager.addScore(numberOfPointsToAddOnJUmp);
        }
    }
}
