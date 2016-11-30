using UnityEngine;
using System.Collections;

public class PlaneTexture : MonoBehaviour {

    public Texture minimap2;

    private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void WallBreak()
    {
        meshRenderer.material.mainTexture = minimap2;
    }
}
