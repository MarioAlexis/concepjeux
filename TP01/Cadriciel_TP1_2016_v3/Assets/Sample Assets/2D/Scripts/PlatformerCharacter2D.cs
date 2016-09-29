﻿using UnityEngine;
using UnityEngine.UI;

public class PlatformerCharacter2D : MonoBehaviour
{
    bool facingRight = true;                            // For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.
    [SerializeField] float wallJumpForce = 400f;		// Amount of force added when the player jumps against walls.
    [SerializeField] float jumpTime = 1f;               // Duration of a jump
    [SerializeField] bool verticalSpeedRestartAtJump=true;	// Is the vertical speed reset when we jump

    [Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%

    [Range(0, 1)]
    [SerializeField] float reductionConsecutiveJump = 1f;// Power reduction between 2 consecutive Jump. 1 = 100% = no reduction. 0 = big reduction.

    [SerializeField] float airControl = 1;			    // Whether or not a player can steer while jumping;
    [SerializeField] float numberMaxOfConsecutivesJumps = 1;// Max Number Of Jump;
    [SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	// TELEPORT SETIING
	[SerializeField] bool keepSpeedOnTeleport;          // Player is able to keep his initial speed when teleporting
    [SerializeField] float maxTeleportRadius;			// Set a maximum radius/distance for teleportation
    [SerializeField] float cooldownTeleportSec;         // Set the cooldown for the teleport effect in Second
    [SerializeField] Transform TeleportCooldownBar;     // Select the UI element which the character manipulate on teleport action

	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
    Transform wallCheck;                                // A position marking where to check if the player is against a wall.
    float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded

	bool grounded = false;								// Whether or not the player is grounded.
    bool againstWall = false;                           // Whether or not the player is against a wall.
    Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.
    float currentNumberOfJump = 0;
    float timer = 0;                                    // timer in jump

    //teleport variables
    private GameObject lineTop, lineBottom;
    private LineRendererController lineControlTop, lineControlBottom;
    private GameObject shadow;
    private float charHeight;
    private bool initTeleport = false;
    private float actualRadius = 0f;
    private RaycastHit2D hitTop, hitMiddle, hitBottom;
    private LayerMask ignoreHitMask;
    private ParticleSystem teleportarticleControl;
    private GameObject teleportEffect;
    private Image teleportLoadingBar;
    private bool isTeleportRdy = true;

    float initialJumpPower = 0.15f;
    float powerJump;
    bool blockJump = false;

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
        wallCheck = transform.Find("WallCheck");
		anim = GetComponent<Animator>();

        // Set the Teleport cooldown as ready at the begin
        teleportLoadingBar = TeleportCooldownBar.GetComponent<Image>();
        teleportLoadingBar.fillAmount = 1.0f;
        isTeleportRdy = true;

        // Getting layerMask to ingore in rayCastHit2D
        ignoreHitMask =  (1 << (LayerMask.NameToLayer("Characters"))) |  (1 << (LayerMask.NameToLayer("Background")));
	}

    bool inAir()
    {
        return !Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }

    bool canJump()
    {
        return currentNumberOfJump < numberMaxOfConsecutivesJumps;
    }


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        againstWall = Physics2D.OverlapCircle(wallCheck.position, groundedRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (teleportarticleControl != null)
            if (!teleportarticleControl.IsAlive())
                Destroy(teleportEffect);
        if (!isTeleportRdy)
        {
            float addFilledAmount = Time.deltaTime / cooldownTeleportSec;
            float currentLoading = teleportLoadingBar.fillAmount;
            teleportLoadingBar.fillAmount = currentLoading + addFilledAmount;
            if (teleportLoadingBar.fillAmount >= 1.0f)
            {
                teleportLoadingBar.fillAmount = 1.0f;
                isTeleportRdy = true;
            }
        }

        if (inAir() && currentNumberOfJump == 0)
            currentNumberOfJump = 1;
	}

    void InitializeJump()
    {
        currentNumberOfJump++;
        if (verticalSpeedRestartAtJump)
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
        if (currentNumberOfJump > 1)
        {
            powerJump *= (reductionConsecutiveJump/5f)+0.8f;
        }

    }


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
            powerJump = initialJumpPower;
            if (numberMaxOfConsecutivesJumps == 0)
                blockJump = true;
            else
                blockJump = false;

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
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
                // ... flip the player.
                Flip();
        }

        if (againstWall)
        {
            blockJump = true;
        }

        if (jump && canJump() && !againstWall)
        {
            InitializeJump();
        }

        if (jump && againstWall)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(wallJumpForce, 0f));
        }

        if (!jumpButtonPressed)
        {
            timer = 0;
            if (!canJump())
                blockJump = true;
            else if (!againstWall)
                blockJump = false;
        }

        if (jumpButtonPressed && timer < jumpTime && !blockJump)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame

            float proportionCompleted = (1 - Mathf.Sqrt(Mathf.Sqrt(timer / jumpTime))) * powerJump;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, proportionCompleted*jumpForce));
            timer += Time.deltaTime;
        }

        Debug.Log("against wall : " + againstWall);

        if (!jumpButtonPressed)
            timer = 0;
    }

    public void Teleport(Vector2 charPos, Vector2 mousePos, bool initTP, bool TP)
    {
        if (initTP && TP && isTeleportRdy)
        {
            initTeleport = true;
        }
        else if (!initTP && TP && initTeleport)
        {
            // Getting the distance between character and mouse position
            actualRadius = Vector2.Distance(mousePos, charPos);

            // Dont Instantiate the "shadow" if the middle Raycast is hitting with a collider
            hitMiddle = Physics2D.Raycast(charPos, mousePos - charPos, actualRadius + 0.5f, ~ignoreHitMask);

            if (lineTop == null && lineBottom == null  && hitMiddle.collider == null && actualRadius < maxTeleportRadius)
            {
                // Instantiate "laser beam" at the top and bottom of the character
                lineTop = Instantiate(Resources.Load("LineREnderer", typeof(GameObject))) as GameObject;
                lineBottom = Instantiate(Resources.Load("LineREnderer", typeof(GameObject))) as GameObject;
                lineControlTop = lineTop.GetComponent<LineRendererController>();
                lineControlBottom = lineBottom.GetComponent<LineRendererController>();

                // Instantiate character "shadow" for teleport location
                shadow = Instantiate(Resources.Load("TeleportShadow", typeof(GameObject)), mousePos, Quaternion.identity) as GameObject;
                shadow.transform.localScale = this.transform.localScale;
            }
            else if(lineTop != null && lineBottom != null)
            {
                if(actualRadius > maxTeleportRadius)
                {
                    Destroy(lineTop);
                    Destroy(lineBottom);
                    Destroy(shadow);
                }
                else if(shadow != null)
                {
                    Vector2 spriteBox = shadow.GetComponent<SpriteRenderer>().bounds.extents;
                    charHeight = spriteBox.y;
                    Vector2 topPoint = new Vector2(mousePos.x, mousePos.y + spriteBox.y);
                    Vector2 bottomPoint = new Vector2(mousePos.x, mousePos.y - spriteBox.y);

                    float distTop = Vector2.Distance(charPos, topPoint);
                    float distBottom = Vector2.Distance(charPos, bottomPoint);

                    hitTop = Physics2D.Raycast(charPos, topPoint - charPos, distTop, ~ignoreHitMask);
                    hitMiddle = Physics2D.Raycast(charPos, mousePos - charPos, actualRadius, ~ignoreHitMask);
                    hitBottom = Physics2D.Raycast(charPos, bottomPoint - charPos, distBottom, ~ignoreHitMask);

                    if(hitTop.collider != null  ||  hitMiddle.collider != null || hitBottom.collider != null)
                    {
                        Destroy(shadow);
                        topPoint = (hitTop.collider != null ? hitTop.point : topPoint);
                        bottomPoint = (hitBottom.collider != null ? hitBottom.point : bottomPoint);
                    }

                    lineControlTop.setLineParameters(charPos, topPoint);
                    lineControlBottom.setLineParameters(charPos, bottomPoint);

                    shadow.transform.localScale = this.transform.localScale;
                    shadow.transform.position = mousePos;
                }
                else if (shadow == null)
                {
                    Vector2 topPoint = new Vector2(mousePos.x, mousePos.y + charHeight);
                    Vector2 bottomPoint = new Vector2(mousePos.x, mousePos.y - charHeight);

                    float distTop = Vector2.Distance(charPos, topPoint);
                    float distBottom = Vector2.Distance(charPos, bottomPoint);

                    hitTop = Physics2D.Raycast(charPos, topPoint - charPos, distTop, ~ignoreHitMask);
                    hitMiddle = Physics2D.Raycast(charPos, mousePos - charPos, actualRadius, ~ignoreHitMask);
                    hitBottom = Physics2D.Raycast(charPos, bottomPoint - charPos, distBottom, ~ignoreHitMask);

                    if (hitTop.collider == null  && hitMiddle.collider == null && hitBottom.collider == null)
                    {
                        shadow = Instantiate(Resources.Load("TeleportShadow", typeof(GameObject)), mousePos, Quaternion.identity) as GameObject;
                        shadow.transform.localScale = this.transform.localScale;
                    }
                    else
                    {
                        topPoint = (hitTop.collider != null ? hitTop.point : topPoint);
                        bottomPoint = (hitBottom.collider != null ? hitBottom.point : bottomPoint);
                    }

                    lineControlTop.setLineParameters(charPos, topPoint);
                    lineControlBottom.setLineParameters(charPos, bottomPoint);

                }
            }
        }
        else if (!initTP && !TP && initTeleport)
        {
            if (lineTop != null && lineBottom != null && shadow != null)
            {
                Destroy(lineTop);
                Destroy(lineBottom);
                Destroy(shadow);
                teleportEffect = Instantiate(Resources.Load("TeleportEffect", typeof(GameObject)), mousePos, Quaternion.identity) as GameObject;
                teleportarticleControl = teleportEffect.GetComponent<ParticleSystem>();
                this.transform.position = mousePos;
                TeleportCooldownBar.GetComponent<Image>().fillAmount = 0.0f;
                isTeleportRdy = false;
            }
            else if(shadow == null)
            {
                Destroy(lineTop);
                Destroy(lineBottom);
                Destroy(shadow);
            }

            initTeleport = false;
        }
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

    public bool isGrounded()
    {
        return grounded;
    }
}
