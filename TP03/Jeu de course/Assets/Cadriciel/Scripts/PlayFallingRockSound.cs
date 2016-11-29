using UnityEngine;
using System.Collections;

public class PlayFallingRockSound : MonoBehaviour {

    [SerializeField]
    AudioClip soundToPlay;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float Volume = 1.0f;

    // PRIVATE VARIABLES
    private AudioSource audio;
    private bool soundAlreadyPlay = false;

	void Start ()
    {
        audio = this.GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider player)
    {
        if(player.tag == "player" && !soundAlreadyPlay)
        {
            audio.PlayOneShot(soundToPlay, Volume);
            soundAlreadyPlay = true;
        }
    }
}
