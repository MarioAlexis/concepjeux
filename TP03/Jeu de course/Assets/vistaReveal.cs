using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class vistaReveal : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private CarController carplayer;
    [SerializeField]
    private float timeOfRotation = 1f;
    [SerializeField]
    private int whichPassage = 1; // 1 beforeHill ; 2 onHill ; 3 midHill ; 4 afterHill


    private bool doRotate = false;
    private float timeStartRotate = .0f;

    void FixedUpdate()
    {
        if (doRotate)
        {
            Vector3 playerPosition = carplayer.transform.position;
            if(whichPassage == 1) {
                cam.transform.RotateAround(playerPosition, carplayer.transform.right, (-10.0f / timeOfRotation) * Time.fixedDeltaTime);
                cam.fieldOfView += (15.0f / timeOfRotation) * Time.fixedDeltaTime;
            }
                
            else if (whichPassage == 2)
            {
                cam.transform.RotateAround(playerPosition, carplayer.transform.right, (15.0f / timeOfRotation) * Time.fixedDeltaTime);
                cam.fieldOfView += (15.0f / timeOfRotation) * Time.fixedDeltaTime;
            }
            else if (whichPassage == 3)
                cam.transform.RotateAround(playerPosition, carplayer.transform.right, (-10.0f / timeOfRotation )* Time.fixedDeltaTime);
                
            else if (whichPassage == 4) {
                cam.transform.RotateAround(playerPosition, carplayer.transform.right, (5.0f / timeOfRotation) * Time.fixedDeltaTime);
                cam.fieldOfView -= (30.0f / timeOfRotation) * Time.fixedDeltaTime;
            }
                

        }
        if(Time.time > timeStartRotate + timeOfRotation)
        {
            doRotate = false;
        }

    }

	void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "player")
        {
            doRotate = true;
            timeStartRotate = Time.time;
        }
    }
}
