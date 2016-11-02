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

    void OnCollisionEnter(Collision projectile)
    {
        Debug.Log(projectile.gameObject.name);
        if(projectile.gameObject.name == "greenshell")
        {
            GameObject tmpwall = Instantiate(DebrisPrefab, this.transform.position, this.transform.rotation) as GameObject;
            tmpwall.transform.localScale = this.transform.localScale;
            Destroy(this.gameObject);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
