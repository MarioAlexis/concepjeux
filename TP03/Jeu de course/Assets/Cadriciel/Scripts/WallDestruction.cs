using UnityEngine;
using System.Collections;

public class WallDestruction : MonoBehaviour
{
    [SerializeField]
    GameObject DebrisPrefab;

    public GameObject Minimap;
    public GameObject car;

	// Use this for initialization

    void OnTriggerEnter(Collider projectile)
    {

        if (projectile.gameObject.tag == "shell")
        {
            Destroy(this.gameObject);
            GameObject tmpwall = Instantiate(DebrisPrefab, this.transform.position, this.transform.rotation) as GameObject;
            tmpwall.transform.localScale = this.transform.localScale;
            if (this.transform.parent.gameObject.name == "WallBegin")
            {
                car.gameObject.SendMessage("SwitchPath");
                Minimap.gameObject.SendMessage("WallBreak1");
            }
            if (this.transform.parent.gameObject.name == "WallEnd") Minimap.gameObject.SendMessage("WallBreak2");
        }
    }
}
