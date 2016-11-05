using UnityEngine;
using System.Collections;

public class StartBoostFlame : MonoBehaviour
{

	public void startBoostEffect()
    {
        this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
    }
}
