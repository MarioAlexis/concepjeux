using UnityEngine;
using System.Collections;

public class brushScoring : MonoBehaviour {

    public Collider playerOneBodyCollider;
    public RaceManager raceManager;

    float timer = 0;
    float lastTimeScored = 0;

    public float intervalBetween2PointScored = 100;            //ms
    public float timeNeededBeforeStartingToScorePoints = 200;  //ms      
    public int numberOfPointsToAdd = 1;                       

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other == playerOneBodyCollider) {
            timer = 0;
        }
        //Debug.Log("Entering Zone");
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerOneBodyCollider)
        {
            timer = 0;
            lastTimeScored = 0;
        }
        //Debug.Log("Leaving Zone");
    }

    void OnTriggerStay(Collider other)
    {
        if (other == playerOneBodyCollider)
        {
            //Debug.Log(timer);
            timer += (Time.deltaTime)*1000;
            if(timer > timeNeededBeforeStartingToScorePoints && timer-lastTimeScored> intervalBetween2PointScored)
            {
                lastTimeScored = timer;
                raceManager.addScore(numberOfPointsToAdd);
                //Debug.Log(timer);
            }
        }
    }
}
