using UnityEngine;
using System.Collections;

public class DestroyStone : MonoBehaviour
{
    [SerializeField]
    GameObject DestroyEffect;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider stone)
    {
        if(stone.tag == "stone")
        {
            Destroy(stone.gameObject);
            GameObject effect = Instantiate(DestroyEffect, stone.transform.position, Quaternion.identity) as GameObject;
            Destroy(effect, 5.0f);
        }
    }
}
