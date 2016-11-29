using UnityEngine;
using System.Collections;

public class FallingStoneManager : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPosList;
    [SerializeField]
    GameObject Stone;
    [SerializeField]
    float RespawnTime = 2.0f;
    [SerializeField]
    float ForceToBottom = 10.0f;

    // PRIVATES VARIABLES
    private float timeAcc = 0.0f;
    private int actualPos = 0;

	// Use this for initialization
	void Start ()
    {
    }
	
    void FixedUpdate()
    {
        timeAcc += Time.deltaTime;
        if(timeAcc >= RespawnTime)
        {
            GameObject stoneinscene = Instantiate(Stone, spawnPosList[actualPos].position, Quaternion.identity) as GameObject;
            stoneinscene.GetComponent<Rigidbody>().AddRelativeForce((Vector3.down * ForceToBottom), ForceMode.Acceleration);
            actualPos = (actualPos + 1) % 3;
            timeAcc = 0.0f;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        
	
	}
}
