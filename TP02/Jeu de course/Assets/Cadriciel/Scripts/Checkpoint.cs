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

	void OnTriggerEnter(Collider other)
	{
		if (other as WheelCollider == null)
		{
			CarController car = other.transform.GetComponentInParent<CarController>();
			if (car)
			{
				_manager.CheckpointTriggered(car,_index);

			}
		}
        if (other.name == "ColliderFront")
        {
            //Debug.Log("sending rubberBandingOff");
            other.transform.parent.transform.parent.gameObject.SendMessage("rubberBandingOff");
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.name == "ColliderFront")
        {
            count++;
            if (count > 4)
            {
                //Debug.Log("sending rubberBandingOn");
                other.transform.parent.transform.parent.gameObject.SendMessage("rubberBandingOn");
            }
            if (count >= 8) count = 0;
            Debug.Log(count);
        }   
    }
}
