using UnityEngine;
using System.Collections;

public class LineRendererController : MonoBehaviour {

    private LineRenderer line;
    private Vector2 origin_, destination_;

	// Use this for initialization
	void Start ()
    {
        line = GetComponent<LineRenderer>();
        line.sortingLayerName = "Foreground";
        line.SetWidth(0.05f, 0.05F);
  	}
	
	// Update is called once per frame
	void Update ()
    {
        line.SetPosition(0, origin_);
        line.SetPosition(1, destination_);
	}

    public void setLineParameters(Vector2 origin, Vector2 destination)
    {
        origin_ = origin;
        destination_ = destination;
    }
}
