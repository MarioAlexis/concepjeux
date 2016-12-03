using UnityEngine;
using System.Collections;

public class PlaneTexture : MonoBehaviour {

    public Texture minimapBegin;
    public Texture minimapEnd;
    public Texture minimapAll;

    private bool minimapBeginAdd = false;
    private bool minimapEndAdd = false;

    private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void WallBreak1()
    {
        if (minimapBegin != null) meshRenderer.material.mainTexture = minimapBegin;
        minimapBeginAdd = true;
        dispAllMap(minimapBeginAdd, minimapEndAdd);
    }
    void WallBreak2()
    {
        if (minimapEnd != null) meshRenderer.material.mainTexture = minimapEnd;
        minimapEndAdd = true;
        dispAllMap(minimapBeginAdd, minimapEndAdd);
    }
    void dispAllMap(bool map1, bool map2)
    {
        if (map1 && map2) meshRenderer.material.mainTexture = minimapAll;
    }
}
