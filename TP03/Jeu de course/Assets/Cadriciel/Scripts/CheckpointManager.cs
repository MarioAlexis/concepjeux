using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;

public class CheckpointManager : MonoBehaviour 
{

	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private int _checkPointCount;
	[SerializeField]
	private int _totalLaps;

    private int nbCarsFinished = 0;

	private bool _finished = false;
	
	private Dictionary<CarController,PositionData> _carPositions = new Dictionary<CarController, PositionData>();

	private class PositionData
	{
		public int lap;
		public int checkPoint;
		public int position;
	}

	// Use this for initialization
	void Awake () 
	{
		foreach (CarController car in _carContainer.GetComponentsInChildren<CarController>(true))
		{
			_carPositions[car] = new PositionData();
		}
	}
	
	public void CheckpointTriggered(CarController car, int checkPointIndex)
	{

		PositionData carData = _carPositions[car];

		//if (!_finished)
		//{
			if (checkPointIndex == 0)
			{
				if (carData.checkPoint == _checkPointCount-1)
				{
					carData.checkPoint = checkPointIndex;
					carData.lap += 1;
					Debug.Log(car.name + " lap " + carData.lap);
					if (IsPlayer(car))
					{
						GetComponent<RaceManager>().Announce("Tour " + (carData.lap+1).ToString());
                        car.addNitro(50f);
					}

					if (carData.lap == _totalLaps)
					{
                        //_finished = true;
                        nbCarsFinished++;
                        Debug.Log("TOUR FINI");
                        Debug.Log(nbCarsFinished);
                        GetComponent<RaceManager>().EndCarRace(car, nbCarsFinished);
					}
                    //if (nbCarsFinished >= 8) GetComponent<RaceManager>().RaceEnded();

                }
			}
			else if (carData.checkPoint == checkPointIndex-1) //Checkpoints must be hit in order
			{
				carData.checkPoint = checkPointIndex;
			}
		//}


	}

	bool IsPlayer(CarController car)
	{
		return car.GetComponent<CarUserControl>() != null;
	}
}
