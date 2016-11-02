using UnityEngine;
using System.Collections;

public class GreenShellManager : MonoBehaviour
{
    // PRIVATE GLOBAL VARIABLES
    private Rigidbody shellRigi;
    private float constantSpeed = 50f;
    private Quaternion constRot;
    private float constHigh = 1f;
    // Use this for initialization
    void Start ()
    {
        shellRigi = this.GetComponent<Rigidbody>();
        constRot = this.transform.rotation;
        constHigh = this.transform.position.y;
        shellRigi.AddRelativeForce(new Vector3(0.0f, 0.0f, 300.0f), ForceMode.Acceleration);
    }
	
    void FixedUpdate()
    {
        shellRigi.velocity = constantSpeed * (shellRigi.velocity.normalized);
    }
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPos = this.transform.position;
        newPos.y = constHigh;
        RaycastHit ray;
        //if(Physics.Raycast(this.transform.position, -this.transform.up, out ray))
        //{
        //    //if(ray.distance > constHigh)
        //    //{
        //       // Debug.Log("HIT ! : " + ray.distance);
        //        this.transform.Translate(newPos, Space.World);
        //    //}
        //}

        this.transform.position = newPos;
        this.transform.rotation = constRot;

    }
}
