using UnityEngine;
using System.Collections;

public class RespawnPlayerZone : MonoBehaviour
{
    [SerializeField]
    int PenalityTimeRespawn = 3;
    [SerializeField]
    GUIText Annoucement;

    //PRIVATE GLOBAL VARIABLES
    private Rigidbody carRigi;
    private Transform carTrans;
    private bool isAI, isPlayer;

    void OnTriggerEnter(Collider car)
    {
        if(car.name == "ColliderFront")
        {
            carRigi = car.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>();
            carTrans = car.transform.parent.transform.parent.gameObject.transform;

            // SET NEW POSITION
            carTrans.position = this.transform.GetChild(0).gameObject.transform.position;

            // SET NULL VELOCITY
            carRigi.velocity = Vector3.zero;
            carRigi.angularVelocity = Vector3.zero;
            carTrans.eulerAngles = new Vector3(0.0f, 112.0f, 0.0f);
            if (carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>() != null)
            {
                carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = false;
                carTrans.FindChild("FireTrigger").GetComponent<shellSpawn>().enabled = false;
                isPlayer = true;
                isAI = false;
            }
            else if(carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarAIControl>() != null)
            {
                carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarAIControl>().enabled = false;
                isPlayer = false;
                isAI = true;
            }

            StartCoroutine(StartCountdown(carTrans, carTrans.position));

        }
    }

    IEnumerator StartCountdown(Transform toStand, Vector3 posToStay)
    {
        Annoucement.fontSize = 100;
        Annoucement.pixelOffset = new Vector2(0.0f, 300.0f);
        Annoucement.color = Color.red;

        int count = PenalityTimeRespawn;
        do
        {
            toStand.position = posToStay;
            Annoucement.text = "--> " + count.ToString() + " <--";
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        while (count > 0);
        Annoucement.text = "GO!";
        //CarActivation(true);
        yield return new WaitForSeconds(1.0f);
        activateCar();
        Annoucement.text = "";
        Annoucement.fontSize = 50;
        Annoucement.pixelOffset = new Vector2(0.0f, 0.0f);
        Annoucement.color = Color.white;
    }

    private void activateCar()
    {
        if(isPlayer)
        {
            carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = true;
            carTrans.FindChild("FireTrigger").GetComponent<shellSpawn>().enabled = true;
        }
        else if(isAI)
        {
            carTrans.gameObject.GetComponent<UnityStandardAssets.Vehicles.Car.CarAIControl>().enabled = true;
        }
    }
}
