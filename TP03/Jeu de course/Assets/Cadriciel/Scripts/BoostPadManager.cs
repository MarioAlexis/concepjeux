using UnityEngine;
using System.Collections;

public class BoostPadManager : MonoBehaviour
{
    [SerializeField]
    float AddAcceleration = 500.0f;

    //PRIVATE GLOBAL VARIABLES
    private Rigidbody carRigi;
    private Transform carTrans;
    private float carVeloMagni;
    private float xVelo, zVelo;
    private float boostAngle, carAngle, diffAngle;
    private float xForce, zForce;
    private bool isInBoostPad;
    private float particleDuration = 0f;

	void Start ()
    {
        boostAngle = this.transform.rotation.eulerAngles.y;
    }

    void FixedUpdate()
    {
        if (isInBoostPad && particleDuration < 2f)
        {
            carTrans.FindChild("BoostFlame").gameObject.GetComponent<StartBoostFlame>().startBoostEffect();
            particleDuration += Time.fixedDeltaTime;
        }
        else
        {
            isInBoostPad = false;
            particleDuration = 0f;
        }
    }

    void OnTriggerEnter(Collider car)
    {
        if(car.name == "ColliderFront")
        {
            carRigi = car.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>();
            carTrans = car.transform.parent.transform.parent.gameObject.transform;

            // PRE-CALCUL
            carAngle = carTrans.rotation.eulerAngles.y;
            float anglemid = boostAngle - 180;
            anglemid = (anglemid < 0 ? 360 - Mathf.Abs(anglemid) : anglemid);
            float angmin = anglemid + 90;
            angmin = (angmin > 360 ? angmin - 360 : angmin);
            float angmax = anglemid - 90;
            angmax = (angmax < 0 ? 360 - Mathf.Abs(angmax) : angmax);
            diffAngle = boostAngle - carAngle;

            //VELOCITY
            carVeloMagni = carRigi.velocity.magnitude;
            xVelo = carVeloMagni * (Mathf.Sin(diffAngle * Mathf.Deg2Rad) /** Mathf.Rad2Deg*/);
            zVelo = carVeloMagni * (Mathf.Cos(diffAngle * Mathf.Deg2Rad) /** Mathf.Rad2Deg*/);

            //ACCELERATION
            xForce = AddAcceleration * (Mathf.Sin(diffAngle * Mathf.Deg2Rad)/* * Mathf.Rad2Deg*/);
            zForce = AddAcceleration * (Mathf.Cos(diffAngle * Mathf.Deg2Rad) /** Mathf.Rad2Deg*/);

            //ADD ACCELERATION & VELOCITY
            carRigi.velocity = Vector3.zero;
            carRigi.AddRelativeForce(new Vector3(xVelo, 0.0f, zVelo), ForceMode.VelocityChange);
            carRigi.AddRelativeForce(new Vector3(xForce, 0.0f, zForce), ForceMode.Acceleration);

            isInBoostPad = true;
        }

    }
}
