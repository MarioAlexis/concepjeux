using UnityEngine;
using System.Collections;

public class WallDestruction : MonoBehaviour
{
    [SerializeField]
    GameObject DebrisPrefab;
    public GameObject Minimap;
	// Use this for initialization

    void OnTriggerEnter(Collider projectile)
    {

        if (projectile.gameObject.tag == "shell")
        {
            Destroy(this.gameObject);
            GameObject tmpwall = Instantiate(DebrisPrefab, this.transform.position, this.transform.rotation) as GameObject;
            Minimap.gameObject.SendMessage("WallBreak");
            tmpwall.transform.localScale = this.transform.localScale;
        }

    }
}
