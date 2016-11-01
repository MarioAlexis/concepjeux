using UnityEngine;
using System.Collections;

public class WallDestruction : MonoBehaviour
{
    [SerializeField]
    GameObject DebrisPrefab;
	// Use this for initialization
	void Start ()
    {
	
	}

    void OnTriggerEnter(Collider projectile)
    {
        Instantiate(DebrisPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
