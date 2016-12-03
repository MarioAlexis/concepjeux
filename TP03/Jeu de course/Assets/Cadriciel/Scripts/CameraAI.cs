using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CameraAI : MonoBehaviour
    {
        private Rigidbody CamRigi;
        private float constantSpeed = 25f;
        private GameObject target;
        float rotation;
        bool doRotate = false;
        float constantRot = 1f;
        float startTime;
        float timeToRotate;

        //RayCast
        RaycastHit hit1;
        float distanceToGround1;
        RaycastHit hit2;
        float distanceToGround2;

        void Start()
        {
            CamRigi = this.GetComponent<Rigidbody>();
            target = FindNextWaypoint();
            CamRigi.transform.Rotate(0, rotation, 0);
        }

        void FixedUpdate()
        {
            if (target != null && (target.transform.position - transform.position).magnitude > 0.5)
            {

                CamRigi.velocity = (target.transform.position - transform.position).normalized * constantSpeed;
                /*hit1 = new RaycastHit();
                hit2 = new RaycastHit();
                Vector3 posRay1 = new Vector3 (transform.position.x, transform.position.y, transform.position.z+2f);
                Vector3 posRay2 = new Vector3 (transform.position.x, transform.position.y, transform.position.z-2f);
                if (Physics.Raycast(posRay1, -Vector3.up, out hit1) && Physics.Raycast(posRay2, -Vector3.up, out hit2))
                {
                    distanceToGround1 = hit1.distance;
                    distanceToGround2 = hit2.distance;
                    Debug.Log(distanceToGround1);
                    Debug.Log(distanceToGround2);
                }

                if (distanceToGround1 - distanceToGround2 > 1) CamRigi.transform.Rotate(constantRot, 0, 0);
                if (distanceToGround2 - distanceToGround1 > 1) CamRigi.transform.Rotate(-constantRot, 0, 0);*/
                if (Time.time >= 34 && Time.time <= 76)
                {
                    //CamRigi.velocity = transform.forward * constantSpeed;
                    if (Time.time <= 35) CamRigi.transform.Rotate(-35 * Time.fixedDeltaTime, 0, 0);
                    if (Time.time >= 50 && Time.time <= 59) CamRigi.transform.Rotate(8 * Time.fixedDeltaTime, 0, 0);
                    if (Time.time >= 75) CamRigi.transform.Rotate(-36 * Time.fixedDeltaTime, 0, 0);
                }
                else
                {
                    
                    if (doRotate)
                    {
                        CamRigi.transform.Rotate(0, rotation * Time.fixedDeltaTime / timeToRotate, 0);
                    }
                    if (Time.time > timeToRotate + startTime) doRotate = false;
                }
            }
            else
            {
                target = FindNextWaypoint();
                doRotate = true;
                startTime = Time.time;
                timeToRotate = (target.transform.position - transform.position).magnitude / constantSpeed;
            }
        }

        GameObject FindNextWaypoint()
        {
            GameObject[] waypoints;
            waypoints = GameObject.FindGameObjectsWithTag("waypoint");
            GameObject findClosest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject waypoint in waypoints)
            {
                Vector3 diff = waypoint.transform.position - position;
                if (Vector3.Dot(CamRigi.transform.TransformDirection(Vector3.forward), diff) > 0)
                {
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance && curDistance > 0.5)
                    {
                        findClosest = waypoint;
                        distance = curDistance;
                        rotation = Vector3.Angle(CamRigi.transform.TransformDirection(Vector3.forward), diff)* Mathf.Sign(Vector3.Cross(CamRigi.transform.TransformDirection(Vector3.forward), diff).y);
                    }
                }
            }
            return findClosest;
        }
    }
}
