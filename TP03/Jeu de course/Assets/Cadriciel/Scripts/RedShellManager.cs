using UnityEngine;
using System.Collections;

public class RedShellManager : MonoBehaviour
{
    // PRIVATE GLOBAL VARIABLES
    private Rigidbody shellRigi;
    [SerializeField]
    private float constantSpeed = 50f;
    private Quaternion constRot;
    private float constHigh = .6f;
    private GameObject target;
    private float deltaTime = 0.0f;

    //SPIN VARIABLES
    private Transform carTrans;
    private Rigidbody carRigi;
    private bool isSpin = false;
    private float spinTour = 0;
    private float accDeg = 0;
    private float angleRot = 7f;
    private float currentangle;
    private Vector3 velo;
    private float SlowDownRatio = 0.5f;


    [SerializeField]
    public float nbBounce = 1;
    // Use this for initialization
    void Start()
    {
        shellRigi = this.GetComponent<Rigidbody>();
        constRot = this.transform.rotation;
        shellRigi.AddRelativeForce(new Vector3(0.0f, 0.0f, 300.0f), ForceMode.Acceleration);

        target = FindClosestEnemy();
    }

    void FixedUpdate()
    {
        if (target != null && deltaTime > .2f)
        {
            shellRigi.velocity = (target.transform.position - transform.position).normalized * constantSpeed;
        }
        else
        {
            shellRigi.velocity = constantSpeed * (shellRigi.velocity.normalized);
            target = FindClosestEnemy();
            deltaTime += Time.fixedDeltaTime;
        }
        if (this.transform.position.y > constHigh)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - .1f, this.transform.position.z);
        }
        else if (this.transform.position.y < constHigh)
        {
            this.transform.position = new Vector3(this.transform.position.x, constHigh, this.transform.position.z);
        }

    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.y = constHigh;
        this.transform.position = newPos;
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

    //Script adapted from unity's exemples : https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject findClosest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            if (Vector3.Dot(shellRigi.transform.TransformDirection(Vector3.forward), diff) > 0)
            {
                float curDistance = diff.sqrMagnitude;

                if (curDistance < distance && curDistance > 16.5f && curDistance < 1000f)
                {
                    findClosest = go;
                    distance = curDistance;
                }
            }
        }
        return findClosest;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            shellRigi.velocity = Vector3.zero;
            this.transform.localScale = Vector3.zero;
            this.transform.position = new Vector3(this.transform.position.x, 3.0f, this.transform.position.z);
            constHigh = -3.0f;

            // MAKE PLAYER SPIN WHEN HIT WITH THE SHELL
            makePlayerSpin(col.gameObject);
        }
        if (col.gameObject.tag == "wall") Destroy(gameObject);
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
