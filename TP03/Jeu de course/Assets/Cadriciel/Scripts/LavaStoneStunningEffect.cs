using UnityEngine;
using System.Collections;

public class LavaStoneStunningEffect : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider car)
    {
        if(car.tag == "player" || car.tag == "ai")
        {
            GameObject thiscar = car.transform.parent.transform.parent.gameObject;
            Debug.Log("lava stone " + thiscar.name);
            thiscar.SendMessage("LavaStoneImpact");
        }
    }
}
