using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

    Quaternion toRotate;

    // Update is called once per frame
    void Update()
    {
        toRotate = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
        this.transform.rotation = toRotate;
    }
}
