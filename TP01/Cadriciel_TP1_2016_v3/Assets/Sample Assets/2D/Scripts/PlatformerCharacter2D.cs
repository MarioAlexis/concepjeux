using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.
    [SerializeField] float jumpTime = 1f;

    [Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
    [Range(0, 1)]
	[SerializeField] float airControl = 0;			    // Whether or not a player can steer while jumping;
    [SerializeField] float numberMaxOfConsecutivesJumps = 1;// Max Number Of Jump;
    [SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

    // OUR GLOBAL VARIABLE
    float currentNumberOfJump = 0;
    float timer = 0;                                    // timer in jump
    float initJumpDirection = 0;                        // 1 mean jumping to the right | -1 mean jumping to the left
    float airControlSpeed = 0;
    bool initjump = false;
    float holdJumpForce = 0;



    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
	}

    bool inAir()
    {
        return !Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }

    bool canJump()
    {
        return currentNumberOfJump <= numberMaxOfConsecutivesJumps;
    }


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (inAir() && currentNumberOfJump == 0)
            currentNumberOfJump = 1;
	}


    /************************************/
    /*          GAME LOGIC              */
    /************************************/
	public void Move(float move, bool crouch, bool jump, bool jumpButtonPressed)
	{
		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded
		if(grounded)
		{
            currentNumberOfJump = 0;

            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * crouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

        //only control the player if airControl is turned on
        else if (airControl>0 && inAir())
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            airControlSpeed = (-initJumpDirection == Mathf.Sign(move) ? maxSpeed * airControl : maxSpeed);
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * airControlSpeed, GetComponent<Rigidbody2D>().velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
                // ... flip the player.
                Flip();
        }

        // If the player should jump...
        /*if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }*/

        // If the player should jump...
        /*if (jump && currentNumberOfJump<numberMaxOfConsecutivesJumps)
        {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            currentNumberOfJump++;
        }*/

        if (grounded && jump && jumpButtonPressed)
        {
            initjump = true;
        }
        else if (!jump && jumpButtonPressed && initjump)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
                timer = 1;
        }
        else if(!jump && !jumpButtonPressed && initjump)
        {
            initjump = false;
            initJumpDirection = Mathf.Sign(move);
            holdJumpForce = timer * (jumpForce/2);
            anim.SetBool("Ground", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce + holdJumpForce));
            currentNumberOfJump++;
            timer = 0;
        }



        //if (canJump() && jumpButtonPressed)
        //{
        //    //Calculate how far through the jump we are as a percentage
        //    //apply the full jump force on the first frame, then apply less force
        //    //each consecutive frame

        //    float proportionCompleted = (1 - (timer / jumpTime)) * 0.1f;
        //    anim.SetBool("Ground", false);

        //    GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        //    // don juan
        //    //GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, proportionCompleted * jumpForce));
        //    //timer += Time.deltaTime;
        //}

        // Debug.Log("jump button pressed : " + jumpButtonPressed);
        // Debug.Log("jump timer : " + timer);

        //if (!jumpButtonPressed)
        //    timer = 0;
    }

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
