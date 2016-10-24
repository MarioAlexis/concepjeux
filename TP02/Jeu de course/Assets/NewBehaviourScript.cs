using UnityEngine;
using System.Collections;

public class ScorePointOnCollision : MonoBehaviour {

    public RaceManager raceManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onColliderEnter(Collision collision)
    {
        raceManager.addScore(10);
        Debug.Log("test");
    }
}
