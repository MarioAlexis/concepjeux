using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class NitroRefill : MonoBehaviour {

    public CarController carController;

    public float RespawnDelaySec = 5.0f;

    private Vector3 boostInitPos;

    float timer = 0f;

    bool needToReset = false;

    void OnTriggerEnter(Collider car)
    {
        if (car.gameObject.tag == "Joueur 1")
        {
            carController.addNitro(25);
            this.transform.position = new Vector3(this.transform.position.x, -3.0f, this.transform.position.z);
            timer = 0f;
            needToReset = true;
        }
    }

    // Use this for initialization
    void Start () {
        boostInitPos = this.transform.position;
    }

   

    // Update is called once per frame
    void Update () {
	    timer += (Time.deltaTime);
        if (needToReset && timer > RespawnDelaySec)
        {
            this.transform.position = boostInitPos;
            needToReset = false;
        }
    }
}
