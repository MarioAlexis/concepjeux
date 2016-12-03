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
        if(car.tag == "player" || car.tag == "ai")
        {
            Transform wheels = car.gameObject.transform.parent.transform.parent.Find("WheelsHubs");
            int nbChild = wheels.childCount;
            for (int i=0; i < nbChild; i++)
            {
                Transform child = wheels.GetChild(i);
                WheelCollider singleWheel = child.GetComponent<WheelCollider>();
                WheelFrictionCurve side_Friction = singleWheel.sidewaysFriction;
                WheelFrictionCurve forward_Friction = singleWheel.forwardFriction;
                side_Friction.stiffness = 0.1f;
                forward_Friction.stiffness = 0.75f;
                singleWheel.sidewaysFriction = side_Friction;
                singleWheel.forwardFriction = forward_Friction;
            }
        }
    }

    void OnTriggerExit(Collider car)
    {
        if (car.tag == "player" || car.tag == "ai")
        {
            Transform wheels = car.gameObject.transform.parent.transform.parent.Find("WheelsHubs");
            int nbChild = wheels.childCount;
            for (int i = 0; i < nbChild; i++)
            {
                Transform child = wheels.GetChild(i);
                WheelCollider singleWheel = child.GetComponent<WheelCollider>();
                WheelFrictionCurve side_Friction = singleWheel.sidewaysFriction;
                WheelFrictionCurve forward_Friction = singleWheel.forwardFriction;
                side_Friction.stiffness = 1.0f;
                forward_Friction.stiffness = 1.0f;
                singleWheel.sidewaysFriction = side_Friction;
                singleWheel.forwardFriction = forward_Friction;
            }
        }
    }
}
