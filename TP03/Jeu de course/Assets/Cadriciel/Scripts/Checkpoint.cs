using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class Checkpoint : MonoBehaviour 
{
	[SerializeField]
	private CheckpointManager _manager;

	[SerializeField]
	private int _index;

    private int count = 0;

	void OnTriggerEnter(Collider coll)
	{
		if (coll as WheelCollider == null)
		{
			CarController car = coll.transform.GetComponentInParent<CarController>();
			if (car)
			{
				_manager.CheckpointTriggered(car,_index);

			}
		}
        if (coll.name == "ColliderFront")
        {
            Debug.Log("sending rubberBandingOff");
            coll.transform.parent.transform.parent.gameObject.SendMessage("rubberBandingOff");
        }
	}

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "ColliderFront")
        {
            count++;
            if (count > 4)
            {
                Debug.Log("sending rubberBandingOn");
                coll.transform.parent.transform.parent.gameObject.SendMessage("rubberBandingOn");
            }
            if (count >= 8) count = 0;
            Debug.Log(count);
        }   
    }
}
