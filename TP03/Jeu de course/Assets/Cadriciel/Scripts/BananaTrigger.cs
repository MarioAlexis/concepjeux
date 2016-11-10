using UnityEditor;
using UnityEngine;
using System.Collections;

public class BananaTrigger : MonoBehaviour
{
    // SERIALIZE CUSTOM EDITOR
    public float SlowDownRatio = 0.5f;
    public float RespawnDelaySec = 2.0f;
    public bool RespawnAfterTrigger = false;

    // PRIVATE GLOBAL VARIABLES
    private bool isSpin = false;
    private bool isAfterSpin = false;
    private float spinTour = 0;
    private float accDeg = 0;
    private float angleRot = 7f;
    private float currentangle;
    private float accDelay = 0.0f;
    private Transform carTrans;
    private Rigidbody carRigi;
    private Vector3 velo;
    private Vector3 bananaInitPos;
    void OnTriggerEnter(Collider car)
    {
        if(!isSpin && !(car.gameObject.tag == "shell"))
        {
            carTrans = car.transform.parent.transform.parent.gameObject.GetComponent<Transform>();
            carRigi = car.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>();
            velo = carRigi.velocity;
            if (velo.magnitude >= 0 && velo.magnitude <= 15)
                spinTour = 1;
            else if (velo.magnitude >= 16 && velo.magnitude <= 30)
                spinTour = 2;
            else if (velo.magnitude >= 31 && velo.magnitude <= 45)
                spinTour = 3;
            carRigi.velocity = Vector3.zero;
            if(RespawnAfterTrigger)
            {
                this.transform.position = new Vector3(this.transform.position.x, -3.0f, this.transform.position.z);
                isAfterSpin = true;
            }
            isSpin = true;
        }
    }

    void Start()
    {
        bananaInitPos = this.transform.position;
    }

    void FixedUpdate()
    {
        if (RespawnAfterTrigger && isAfterSpin)
        {
            if (accDelay < RespawnDelaySec)
            {
                accDelay += Time.deltaTime;
            }
            else
            {
                this.transform.position = bananaInitPos;
                accDelay = 0;
                isAfterSpin = false;
            }
        }
    }

    void Update()
    {
        if(isSpin && (accDeg < (360 * spinTour)))
        {
            Vector3 currentPos = carTrans.position;
            Vector3 newVelo = (velo * SlowDownRatio) * Time.deltaTime;
            Vector3 newPos = currentPos + newVelo; ;

            currentangle = carTrans.rotation.eulerAngles.y;
            carTrans.eulerAngles = new Vector3(0.0f, currentangle + angleRot, 0.0f);
            accDeg = accDeg + angleRot;
            carTrans.position = newPos;
        }
        else if (isSpin && (accDeg >= (360 * spinTour)))
        {
            // a repenser pour donner une petite acceleration
            carRigi.velocity = velo * SlowDownRatio * 0.5f;
            isSpin = false;
            accDeg = 0;
        }
    }
}

[CustomEditor(typeof(BananaTrigger))]
public class MyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BananaTrigger mybanana = (BananaTrigger)target;

        mybanana.SlowDownRatio = EditorGUILayout.Slider("Slow Down Ratio", mybanana.SlowDownRatio, 0.0f, 1.0f);
        mybanana.RespawnAfterTrigger = EditorGUILayout.Toggle("Respawn After Trigger ?", mybanana.RespawnAfterTrigger);

        if (mybanana.RespawnAfterTrigger)
        {
            mybanana.RespawnDelaySec = EditorGUILayout.Slider("Respawn Delay Sec", mybanana.RespawnDelaySec, 1.0f, 10.0f);
        }
    }
}
