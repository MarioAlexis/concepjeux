using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class SpeedometerUI : MonoBehaviour {
 

    public Texture2D dialTex;
    public Texture2D needleTex;
    public Vector2 dialPos;
    public CarController carConroller;
    public float topSpeed = 0;
    public float stopAngle = 0;
    public float topSpeedAngle = 0;
    public float speed = 0;

    // Use this for initialization
    void Start()
    {
        speed = 0;
        topSpeed = carConroller.MaxSpeed;
        
        dialPos = new Vector2(Screen.width*0.05f, Screen.height*0.6f);
        //stopAngle = 90;
    }

    // Update is called once per frame
    void Update()
    {
        speed = carConroller.CurrentSpeed;
    }

    void OnGUI()
    {

        GUI.DrawTexture(new Rect(dialPos.x, dialPos.y, dialTex.width, dialTex.height), dialTex);  
        Vector2 centre = new Vector2((dialPos.x) + dialTex.width / 2, (dialPos.y) + dialTex.height / 2);
        Matrix4x4 savedMatrix = GUI.matrix;
        float speedFraction = speed / topSpeed;
        float needleAngle = Mathf.Lerp(stopAngle, topSpeedAngle, speedFraction);
        GUIUtility.RotateAroundPivot(needleAngle, centre);
        GUI.DrawTexture(new Rect(centre.x, centre.y - needleTex.height / 2, needleTex.width, needleTex.height), needleTex);
        GUI.matrix = savedMatrix;
    }

}
