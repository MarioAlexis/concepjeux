using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour 
{
	private PlatformerCharacter2D character;
    private bool jump;
    private bool jumpButtonPressed;
    private bool initTeleport, teleport;


	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
#if CROSS_PLATFORM_INPUT
        if (CrossPlatformInput.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            jumpButtonPressed = true;
        }
        else
            jumpButtonPressed = false;

        // TELEPORTING
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            initTeleport = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            teleport = true;
        }
        else
            teleport = false;

#else
		if (Input.GetButtonDown("Jump")) jump = true;
#endif

    }

	void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		#else
		float h = Input.GetAxis("Horizontal");
		#endif

		// Pass all parameters to the character control script.
		character.Move( h, crouch , jump, jumpButtonPressed);

        // Pass all the parameters for teleporting
        character.Teleport(character.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), initTeleport, teleport);

        // Reset the jump input once it has been used.
	    jump = false;
        initTeleport = false;

    }

}
