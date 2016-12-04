using UnityEngine;
using System.Collections;

public class redShellRefill : MonoBehaviour {

    public shellSpawn shell;

    public float RespawnDelaySec = 5.0f;

    private Vector3 refillInitPos;

    float timer = 0f;

    bool needToReset = false;

    void OnTriggerEnter(Collider car)
    {
        if (car.gameObject.tag == "player")
        {
            shell.refillRedShell();
            this.transform.position = new Vector3(this.transform.position.x, -3.0f, this.transform.position.z);
            timer = 0f;
            needToReset = true;
        }
    }

    // Use this for initialization
    void Start()
    {
        refillInitPos = this.transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        timer += (Time.deltaTime);
        if (needToReset && timer > RespawnDelaySec)
        {
            this.transform.position = refillInitPos;
            needToReset = false;
        }
        transform.Rotate(0, Time.deltaTime * 80, 0, Space.World);
    }
}
