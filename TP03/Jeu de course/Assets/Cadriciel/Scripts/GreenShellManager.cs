using UnityEngine;
using System.Collections;

public class GreenShellManager : MonoBehaviour
{
    // PRIVATE GLOBAL VARIABLES
    private Rigidbody shellRigi;
    [SerializeField]
    private float constantSpeed = 50f;
    private Quaternion constRot;
    private float deltaTimeHeight = 0.0f;

    //RAYCAST
    RaycastHit hit;
    float distanceToGround;

//SPIN VARIABLES
    private Transform carTrans;
    private Rigidbody carRigi;
    private bool isSpin = false;
    private bool isAfterSpin = false;
    private float spinTour = 0;
    private float accDeg = 0;
    private float angleRot = 7f;
    private float currentangle;
    private float accDelay = 0.0f;
    private Vector3 velo;
    private float SlowDownRatio = 0.5f;


    [SerializeField]
    public float nbBounce = 1;
    // Use this for initialization
    void Start()
    {
        shellRigi = this.GetComponent<Rigidbody>();
        constRot = this.transform.rotation;
        constRot.x = 0;
        constRot.z = 0;

        shellRigi.AddRelativeForce(new Vector3(0.0f, 0.0f, 300.0f), ForceMode.Acceleration);
    }

    void FixedUpdate()
    {
        shellRigi.velocity = constantSpeed * (shellRigi.velocity.normalized);
        if (deltaTimeHeight > .05f)
        {
            hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                distanceToGround = hit.distance;
            }
            if (distanceToGround > .7f)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - distanceToGround + .6f, this.transform.position.z);
            }
            if (distanceToGround < .5f)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + distanceToGround, this.transform.position.z);
            }
            deltaTimeHeight = 0;
        }
        else deltaTimeHeight += Time.fixedDeltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position;
        this.transform.rotation = constRot;

        // SPIN UPDATE
        if (isSpin && (accDeg < (360 * spinTour)))
        {
            Vector3 currentPos = carTrans.position;
            Vector3 newVelo = (velo * SlowDownRatio) * Time.deltaTime;
            Vector3 newPosSpin = currentPos + newVelo; ;

            currentangle = carTrans.rotation.eulerAngles.y;
            carTrans.eulerAngles = new Vector3(0.0f, currentangle + angleRot, 0.0f);
            accDeg = accDeg + angleRot;
            carTrans.position = newPosSpin;
        }
        else if (isSpin && (accDeg >= (360 * spinTour)))
        {
            // a repenser pour donner une petite acceleration
            carRigi.velocity = velo * SlowDownRatio * 0.5f;
            isSpin = false;
            accDeg = 0;
            Destroy(this.gameObject);
        }
    }

    void checkState()
    {
        if (nbBounce == 0) Destroy(this.gameObject);
        else nbBounce--;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            shellRigi.velocity = Vector3.zero;
            this.transform.localScale = Vector3.zero;
            this.transform.position = new Vector3(this.transform.position.x, 3.0f, this.transform.position.z);
            //constHigh = -3.0f;

            // MAKE PLAYER SPIN WHEN HIT WITH THE GREEN SHELL
            makePlayerSpin(col.gameObject);
        }
        if (col.gameObject.tag == "wall") checkState();
    }

    void makePlayerSpin(GameObject car)
    {
        if (!isSpin)
        {
            carTrans = car.GetComponent<Transform>();
            carRigi = car.GetComponent<Rigidbody>();
            velo = carRigi.velocity;
            if (velo.magnitude >= 0 && velo.magnitude <= 15)
                spinTour = 1;
            else if (velo.magnitude >= 16 && velo.magnitude <= 30)
                spinTour = 2;
            else if (velo.magnitude >= 31 && velo.magnitude <= 45)
                spinTour = 3;
            carRigi.velocity = Vector3.zero;
            isSpin = true;
        }
    }
}
