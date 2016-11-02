using UnityEngine;
using System.Collections;

public class shellSpawn : MonoBehaviour {


    [SerializeField]
    GameObject missile;
    private float constantSpeed = 30f;
    private GameObject missileManager;
    // Use this for initialization
    void Start()
    {
       // greenMissile = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Cadriciel/Prefabs/greenShell.prefab", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            missileManager = Instantiate(missile, this.transform.position, this.transform.rotation) as GameObject;
            //missileManager.GetComponent<Rigidbody>().velocity = transform.forward.normalized * constantSpeed;

            /*greenMissileClone = Instantiate(greenMissile) as GameObject;
            greenMissileClone.transform.localScale.Set(.1f, .1f, .1f);
            greenMissileClone.transform.position = transform.position + transform.forward;
            greenMissileClone.GetComponent<Rigidbody>().velocity = transform.forward.normalized * constantSpeed;*/
            //30 => changer valeur de la vitesse
        }
       /* if (missileManager != null)
        {
            missileManager.GetComponent<Rigidbody>().velocity = constantSpeed * (missileManager.GetComponent<Rigidbody>().velocity.normalized);
        }*/
    }
}
