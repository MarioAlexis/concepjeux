using UnityEngine;
using System.Collections;

public class shellSpawn : MonoBehaviour {


    [SerializeField]
    Object greenMissile;
    [SerializeField]
    Object redMissile;
    private GameObject missileManager;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            missileManager = Instantiate(greenMissile, transform.position, transform.rotation) as GameObject;
        }
        if (Input.GetMouseButtonDown(1))
        {
            missileManager = Instantiate(redMissile, transform.position, transform.rotation) as GameObject;
        }
    }
}
