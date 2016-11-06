﻿using UnityEngine;
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
        if(projectile.gameObject.tag == "shell")
        {
            Destroy(this.gameObject);
            GameObject tmpwall = Instantiate(DebrisPrefab, this.transform.position, this.transform.rotation) as GameObject;
            tmpwall.transform.localScale = this.transform.localScale;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
