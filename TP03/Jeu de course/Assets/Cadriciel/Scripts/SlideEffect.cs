using UnityEngine;
using System.Collections;

public class SlideEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   void OnTriggerEnter(Collider car)
    {
        if(car.tag == "player")
        {
            Debug.Log("Player glisse");
            Transform wheels = car.gameObject.transform.parent.transform.parent.Find("WheelsHubs");
            Debug.Log(wheels.name);
            int nbChild = wheels.childCount;
            Debug.Log(nbChild);
            for (int i=0; i < nbChild; i++)
            {
                Transform child = wheels.GetChild(i);
                Debug.Log(child.name);
                WheelCollider singleWheel = child.GetComponent<WheelCollider>();
                WheelFrictionCurve frictionCurve = singleWheel.sidewaysFriction;
                frictionCurve.stiffness = 0.1f;
                singleWheel.sidewaysFriction = frictionCurve;
            }
        }
    }

    void OnTriggerExit(Collider car)
    {
        if (car.tag == "player")
        {
            Debug.Log("Player glisse");
            Transform wheels = car.gameObject.transform.parent.transform.parent.Find("WheelsHubs");
            Debug.Log(wheels.name);
            int nbChild = wheels.childCount;
            Debug.Log(nbChild);
            for (int i = 0; i < nbChild; i++)
            {
                Transform child = wheels.GetChild(i);
                Debug.Log(child.name);
                WheelCollider singleWheel = child.GetComponent<WheelCollider>();
                WheelFrictionCurve frictionCurve = singleWheel.sidewaysFriction;
                frictionCurve.stiffness = 1.0f;
                singleWheel.sidewaysFriction = frictionCurve;
            }
        }
    }
}
