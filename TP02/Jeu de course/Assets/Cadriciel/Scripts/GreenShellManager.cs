using UnityEngine;
using System.Collections;

public class GreenShellManager : MonoBehaviour
{
    // PRIVATE GLOBAL VARIABLES
    private Rigidbody shellRigi;
    private float constantSpeed = 50f;
    private Quaternion constRot;
    private float constHigh = 1f;
    [SerializeField]
    public float nbBounce = 1;
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

    void checkState()
    {
        if (nbBounce == 0) Destroy(gameObject);
        else nbBounce--;
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Shell destroyed");
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "wall") checkState();
    }

}
