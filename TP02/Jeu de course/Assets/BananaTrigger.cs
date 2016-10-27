using UnityEngine;
using System.Collections;

public class BananaTrigger : MonoBehaviour
{
    float spintime = 3.0f;
    float currentSpinTime = 0.0f;
    float distance = 20.0f;
    bool isSpin = false;
    float currentangle;
    Transform carTrans;
    Rigidbody carRigi;
    Vector3 velo;
    void OnTriggerEnter(Collider car)
    {
        Debug.Log("collider : " +car);
        Debug.Log(car.transform.parent.transform.parent.name);
        carTrans = car.transform.parent.transform.parent.gameObject.GetComponent<Transform>();
        carRigi = car.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>();
        isSpin = true;
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2, this.transform.position.z);
        velo = carRigi.velocity;
        Debug.Log(velo);
        Update();
    }

    void Update()
    {
        if(isSpin && currentSpinTime <= spintime)
        {
            //Debug.Log("spin state");
            currentSpinTime += Time.deltaTime;
            //currentangle = carTrans.rotation.eulerAngles.y;
            //Debug.Log("angle : " + currentangle);
            Debug.Log("VELO ==> " + carRigi.velocity);
            Debug.Log("ANGULAR ==> " + carRigi.angularVelocity);
           // carTrans.Rotate(Vector3.up, Time.deltaTime * 250);
            carTrans.Translate(Time.deltaTime * 15, 0.0f, 0.0f,Space.World);
           // carRigi.angularVelocity = carRigi.angularVelocity * 0.05f;
            //carTrans.Rotate(0, currentangle+Time.deltaTime, 0, Space.World);
           // carTrans.Rotate(Vector3.up, Time.deltaTime*250);

        }
        else
        {
            isSpin = false;
        }
    }
}
