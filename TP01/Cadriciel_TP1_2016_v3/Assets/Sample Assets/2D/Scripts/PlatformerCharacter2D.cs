using UnityEngine;
using UnityEngine.UI;

public class PlatformerCharacter2D : MonoBehaviour
{
    bool facingRight = true;                            // For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.
    [SerializeField] float jumpTime = 1f;               // Duration of a jump
    [SerializeField] bool verticalSpeedRestartAtJump=true;  // Is the vertical speed reset when we jump

    //WallJump
    [SerializeField] float wallJumpForce = 400f;		// Amount of force added when the player jumps against walls.
    private float timerWallJump = 0f;
    private float wallJumpTime = .4f;
    private float airControlBuffer = 0;
    private bool isWallJumping = false;

    [Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%

    [Range(0, 1)]
    [SerializeField] float reductionConsecutiveJump = 1f;// Power reduction between 2 consecutive Jump. 1 = 100% = no reduction. 0 = big reduction.

    [Range(0, 1)]
    [SerializeField] float airControl = 1.0f;			    // Whether or not a player can steer while jumping;
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

    //airControl variables
    private float initJumpDirection = 0;
    private bool hasChanged = false;

    //teleport variables
    private GameObject lineTop, lineBottom;
    private LineRendererController lineControlTop, lineControlBottom;
    private GameObject shadow;
    private Vector2 shadow_topPoint, shadow_diagonalBottomPoint;
    private float shadow_height, shadow_width;
    private bool shadow_isOverLapping;
    private bool initTeleport = false;
    private float actualRadius = 0f;
    private ParticleSystem teleportarticleControl;
    private GameObject teleportEffect;
    private Image teleportLoadingBar;
    private bool isTeleportRdy = true;

    //Jump indicator variables
    private GameObject maxHeightIndicator;
    private LineRendererController maxHeightIndicatorControl;
    private float maxHeight = 0;
    private float tempTimer = 0;
    private float detectForceChange = 0;
    private float detectMassChange = 0;
    private float detectJumpTimeChange = 0;
    [SerializeField]
    bool drawJumpIndicator = false;
    private float posinit = 0f;
    private float posfinal = 0f;
    private float vinit = 0f;
    private float vfinal = 0f;
    private float acceleration = 0f;

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

        // Destroy the teleport shockwave particles system if is not active
        if (teleportarticleControl != null)
            if (!teleportarticleControl.IsAlive())
                Destroy(teleportEffect);

        // if the teleport state is not ready. This mean we need to increase the teleport loading bar
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
        if (!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);
        anim.SetBool("WallJump", isWallJumping);

        //only control the player if grounded
        if (grounded)
		{
            hasChanged = false;
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

        //used to know if user tries to air control
        if (jump) initJumpDirection = Mathf.Sign(move);

        //only control the player if airControl is turned on
        else if (airControl>0 && inAir())
        {

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(move));

            if (initJumpDirection != Mathf.Sign(move) && !hasChanged) hasChanged = true;

            Vector2 velo = GetComponent<Rigidbody2D>().velocity;
            velo += new Vector2(move * airControl * maxSpeed, 0f);
            if (velo.x >= airControl * maxSpeed && hasChanged)
                velo.x = airControl * maxSpeed;
            else if (velo.x >= maxSpeed && !hasChanged)
                velo.x = maxSpeed;
            else if (velo.x <= -airControl * maxSpeed && hasChanged)
                velo.x = -airControl * maxSpeed;
            else if (velo.x <= -maxSpeed && !hasChanged)
                velo.x = -maxSpeed;

            GetComponent<Rigidbody2D>().velocity = velo;

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
                // ... flip the player.
                Flip();
        }

        if (jump && canJump())// && !againstWall)
        {
            InitializeJump();
        }

        if ((jump && inAir() && againstWall) || timerWallJump != 0f && timerWallJump < wallJumpTime)
        {
            isWallJumping = true;
            
            if (airControlBuffer == 0)
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                if (facingRight)
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-wallJumpForce, jumpForce));
                else
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(wallJumpForce, jumpForce));
                airControlBuffer = airControl;
                airControl = 0;
                Flip();
            }
            timerWallJump += Time.fixedDeltaTime;
        }
        else if (timerWallJump >= wallJumpTime)
        {
            timerWallJump = 0f;
            airControl = airControlBuffer;
            airControlBuffer = 0;
            isWallJumping = false;
        }

        if (!jumpButtonPressed)
        {
            timer = 0;
            if (!canJump())
                blockJump = true;
            else if (!againstWall)
                blockJump = false;
        }

        if (jumpButtonPressed && timer < jumpTime && !blockJump && !isWallJumping)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame

            float proportionCompleted = (1 - Mathf.Sqrt(Mathf.Sqrt(timer / jumpTime))) * powerJump;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, proportionCompleted*jumpForce));
            timer += Time.deltaTime;
        }

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

            if (lineTop == null && lineBottom == null  && actualRadius < maxTeleportRadius)
            {
                // Instantiate the shadow but in disable mode --> this will allow to get the size of the shadow
                shadow = Instantiate(Resources.Load("TeleportShadow", typeof(GameObject)), mousePos, Quaternion.identity) as GameObject;
                shadow.transform.localScale = this.transform.localScale;
                shadow.SetActive(false);
                
                // Get the height and the wodth of the character shadow
                shadow_height = shadow.GetComponent<SpriteRenderer>().bounds.extents.y;
                shadow_width = shadow.GetComponent<SpriteRenderer>().bounds.extents.x;

                // Get Vector of opposite diagonal point to make a rectagular shape
                shadow_topPoint = new Vector2(mousePos.x - shadow_width, mousePos.y + shadow_height);
                shadow_diagonalBottomPoint = new Vector2(mousePos.x + shadow_width, mousePos.y - shadow_height);

                // Check if the shadow is over lapping with "what is ground layer"
                shadow_isOverLapping = Physics2D.OverlapArea(shadow_topPoint, shadow_diagonalBottomPoint, whatIsGround);

                shadow.SetActive(!shadow_isOverLapping);

                // Instantiate "laser beam" at the top and bottom of the character
                lineTop = Instantiate(Resources.Load("LineRenderer", typeof(GameObject))) as GameObject;
                lineBottom = Instantiate(Resources.Load("LineRenderer", typeof(GameObject))) as GameObject;
                lineControlTop = lineTop.GetComponent<LineRendererController>();
                lineControlBottom = lineBottom.GetComponent<LineRendererController>();
            }
            else if(lineTop != null && lineBottom != null)
            {
                if(actualRadius > maxTeleportRadius)
                {
                    shadow.SetActive(false);
                    lineTop.SetActive(false);
                    lineBottom.SetActive(false);
                }
                else if(shadow.activeSelf)
                {
                    // Set back the visual effect of the red lines
                    lineTop.SetActive(true);
                    lineBottom.SetActive(true);

                    // Get point for the red line visual
                    Vector2 topPoint = new Vector2(mousePos.x, mousePos.y + shadow_height);
                    Vector2 bottomPoint = new Vector2(mousePos.x, mousePos.y - shadow_height);

                    // Get Vector of opposite diagonal point to make a rectagular shape
                    shadow_topPoint = new Vector2(mousePos.x - shadow_width, mousePos.y + shadow_height);
                    shadow_diagonalBottomPoint = new Vector2(mousePos.x + shadow_width, mousePos.y - shadow_height);

                    // Check if the shadow is over lapping with "what is ground layer"
                    shadow_isOverLapping = Physics2D.OverlapArea(shadow_topPoint, shadow_diagonalBottomPoint, whatIsGround);

                    shadow.SetActive(!shadow_isOverLapping);

                    lineControlTop.setLineParameters(charPos, topPoint);
                    lineControlBottom.setLineParameters(charPos, bottomPoint);

                    shadow.transform.localScale = this.transform.localScale;
                    shadow.transform.position = mousePos;
                }
                else if (!shadow.activeSelf)
                {
                    // Set back the visual effect of the red lines
                    lineTop.SetActive(true);
                    lineBottom.SetActive(true);

                    // Get point for the red line visual
                    Vector2 topPoint = new Vector2(mousePos.x, mousePos.y + shadow_height);
                    Vector2 bottomPoint = new Vector2(mousePos.x, mousePos.y - shadow_height);

                    // Get Vector of opposite diagonal point to make a rectagular shape
                    shadow_topPoint = new Vector2(mousePos.x - shadow_width, mousePos.y + shadow_height);
                    shadow_diagonalBottomPoint = new Vector2(mousePos.x + shadow_width, mousePos.y - shadow_height);

                    // Check if the shadow is over lapping with "what is ground layer"
                    shadow_isOverLapping = Physics2D.OverlapArea(shadow_topPoint, shadow_diagonalBottomPoint, whatIsGround);

                    shadow.SetActive(!shadow_isOverLapping);

                    lineControlTop.setLineParameters(charPos, topPoint);
                    lineControlBottom.setLineParameters(charPos, bottomPoint);

                }
            }
        }
        // Player release the shift button
        else if (!initTP && !TP && initTeleport)
        {
            if (shadow.activeSelf)
            {
                Destroy(lineTop);
                Destroy(lineBottom);
                Destroy(shadow);
                teleportEffect = Instantiate(Resources.Load("TeleportEffect", typeof(GameObject)), mousePos, Quaternion.identity) as GameObject;
                teleportarticleControl = teleportEffect.GetComponent<ParticleSystem>();
                Vector2 charVelocity = (keepSpeedOnTeleport ? this.GetComponent<Rigidbody2D>().velocity : new Vector2(0.0f, 0.0f));
                this.transform.position = mousePos;
                this.GetComponent<Rigidbody2D>().velocity = charVelocity;
                TeleportCooldownBar.GetComponent<Image>().fillAmount = 0.0f;
                isTeleportRdy = false;
            }
            else if(!shadow.activeSelf)
            {
                Destroy(lineTop);
                Destroy(lineBottom);
                Destroy(shadow);
            }

            initTeleport = false;
        }
    }

    public void drawMaxJumpHeight(Vector2 charPos)
    {
        float gravity = GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity.magnitude;
        if (drawJumpIndicator)
        {
            if (maxHeightIndicator == null && maxHeightIndicatorControl == null)
            {
                maxHeightIndicator = Instantiate(Resources.Load("LineRenderer", typeof(GameObject))) as GameObject;
                maxHeightIndicatorControl = maxHeightIndicator.GetComponent<LineRendererController>();
            }
            //On relance le calcul si jumpForce, la masse ou jumpTime a été modifié
            if (detectForceChange != jumpForce 
                || detectMassChange != GetComponent<Rigidbody2D>().mass
                || detectJumpTimeChange != jumpTime)
            {
                tempTimer = 0;
                acceleration = 0;
                posinit = 0;
                vinit = 0;
                posfinal = 0;
                vfinal = 0;
                detectForceChange = jumpForce;
                detectMassChange = GetComponent<Rigidbody2D>().mass;
                detectJumpTimeChange = jumpTime;
            }
            //Calcul of the max height with the equations of motion
            if (tempTimer < jumpTime)
            {
                posinit = posfinal;
                vinit = vfinal;
                acceleration = (((1 - Mathf.Sqrt(Mathf.Sqrt(tempTimer / jumpTime))) * initialJumpPower * jumpForce) - gravity)/ GetComponent<Rigidbody2D>().mass;
                vfinal = vinit + (acceleration * Time.deltaTime);
                posfinal = posinit + vinit * (Time.deltaTime) + ((acceleration / 2) * (Time.deltaTime) * (Time.deltaTime));
                tempTimer += Time.deltaTime;
            }
            else
            {
                //The character position is determined from its feets
                float characterPositionY = charPos.y - GetComponent<SpriteRenderer>().bounds.extents.y + .5f ;
                //Freezes the indicator while jumping
                if (grounded)
                    if (posfinal < 0)
                        maxHeight = characterPositionY;
                    else
                        maxHeight = characterPositionY + posfinal;
                
                Vector2 leftPoint = new Vector2(charPos.x - 1f, maxHeight);
                Vector2 rightPoint = new Vector2(charPos.x + 1f, maxHeight);
                //Drawing the line between the two calculated points
                maxHeightIndicatorControl.setLineParameters(leftPoint, rightPoint);
            }
        }
        else
            Destroy(maxHeightIndicator);
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
