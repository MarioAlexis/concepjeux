using UnityEngine;
using System.Collections;

public class BananaSpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionList;
    [SerializeField]
    GameObject ObjectToSpawn;

    // GLOBAL PRIVATE VARIABLES
    private int nbSection;
	// Use this for initialization
	void Start ()
    {
        nbSection = sectionList.Length;
        for(int i=0; i < nbSection; i++)
        {
            int nbChild = sectionList[i].transform.childCount;
            for(int j=0; j<nbChild; j++)
            {
                Instantiate(ObjectToSpawn, sectionList[i].transform.GetChild(j).transform.position, Quaternion.identity);
            }
        }
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
