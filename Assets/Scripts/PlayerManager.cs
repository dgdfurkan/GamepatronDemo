using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    #region Serializable Variables

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject bulletPrefab; //Duvar ya da npc objelerine toklandığında spawnlanacak olan mermi prefabı
	[SerializeField] private Transform bulletSpawnPoint;
    
    #region Private Variables
	
    [Header("Data")] private PlayerData _data; // PlayerData variable to store player-related data like movement speed, jump force, etc.

    private Vector3 _moveDirection; // Stores the movement direction.
    private float _horizontalInput; // Stores the horizontal input received from input devices.
    private float _verticalVelocity; // Stores the vertical velocity of the character.
    private float _groundedTimer; // Timer to track the time since the character was grounded.
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Movement = Animator.StringToHash("Movement");

    #endregion

    #endregion

    #endregion

    #endregion

    private void Awake()
    {
	    _data = GetPlayerData(); // Initializes '_data' by loading player data from a scriptable object.
    }

    /// <summary>
    /// Loads PlayerData from a ScriptableObject stored in the Resources folder.
    /// </summary>
    /// <returns>The PlayerData loaded from the ScriptableObject.</returns>
    private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").PlayerData;
    
    private void Update()
    {
	    // Calls functions to apply various movement-related actions during each frame.
	    HandleGravity();
	    GetMovementInput();
	    HandleRotation();
	    HandleJump();
	    HandleMovement();
	    //ApllyAll();
    }

    private void LateUpdate()
    {
	    // Aligns the camera's x-position with the player's x-position to follow the player horizontally.
	    mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    #region Character Controller Events

    /// <summary>
    /// Applies gravity to the character based on the character controller's state.
    /// </summary>
    void HandleGravity()
    {
	    if (characterController.isGrounded && _verticalVelocity < 0) // Resets vertical velocity when grounded to prevent continuous acceleration when grounded.
	    {
		    _verticalVelocity = 0f;
	    }
	    
	    _verticalVelocity += Physics.gravity.y * 3 * Time.deltaTime; // Applies gravitational force to the vertical velocity.
    }

    /// <summary>
    /// Retrieves movement input from the player's input devices.
    /// </summary>
    void GetMovementInput()
    {
	    _horizontalInput = Input.GetAxis("Horizontal"); // Retrieves horizontal input from the input axis.
	    _moveDirection = new Vector3(_horizontalInput, 0, 0); // Sets the movement direction based on the horizontal input.
	    animator.SetFloat(Movement, Mathf.Abs(_horizontalInput), 0f, Time.deltaTime); // Sets the blend parameter in the animator based on the absolute value of the horizontal input.
	    _moveDirection *= _data.MovementData.MoveSpeed; // Multiplies the movement direction by the player's MoveSpeed.
    }

    /// <summary>
    /// Rotates the character based on the movement direction.
    /// </summary>
    void HandleRotation()
    {
	    if (_moveDirection.magnitude > 0.05f) // Rotates the character towards the movement direction if there's significant movement.
	    {
		    gameObject.transform.forward = _moveDirection;
	    }
    }

    /// <summary>
    /// Handles the jumping action of the character when the jump button is pressed.
    /// </summary>
    void HandleJump()
    {
	    if (characterController.isGrounded) // Resets grounded timer when the character is on the ground.
	    {
		    _groundedTimer = 0.2f;
	    }

	    if (_groundedTimer > 0) // Decreases the grounded timer over time.
	    {
		    _groundedTimer -= Time.deltaTime;
	    }

	    if (Input.GetButtonDown("Jump")) // Initiates a jump when the Jump button is pressed and the character is grounded.
	    {
		    if (_groundedTimer > 0)
		    {
			    _groundedTimer = 0;
			    animator.SetTrigger(Jump); // Triggers the jump animation.
			    _verticalVelocity = _data.MovementData.JumpForce; // Sets the vertical velocity to the jump force.
			    // Uncomment the next line to apply extra force during the jump.
			    //verticalVelocity += Physics.gravity.y * Time.deltaTime * 5f;
		    }
	    }
    }

    /// <summary>
    /// Applies movement to the character based on the calculated direction and velocity.
    /// </summary>
    void HandleMovement()
    {
	    _moveDirection.y = _verticalVelocity; 
	    characterController.Move(_moveDirection * Time.deltaTime); // Moves the character based on the combined movement direction and vertical velocity.
    }
    
    #endregion
    
    void ApllyAll()
    {
	    if (characterController.isGrounded)
	    {
		    // cooldown interval to allow reliable jumping even when coming down ramps
		    _groundedTimer = 0.2f;
	    }
	    if (_groundedTimer > 0)
	    {
		    _groundedTimer -= Time.deltaTime;
	    }
 
	    // slam into the ground
	    if (characterController.isGrounded && _verticalVelocity < 0)
	    {
		    // hit ground
		    _verticalVelocity = 0f;
	    }
 
	    // apply gravity always, to let us track down ramps properly
	    _verticalVelocity += Physics.gravity.y * Time.deltaTime;
 
	    // gather lateral input control
	    _horizontalInput = Input.GetAxis("Horizontal");
	    _moveDirection = new Vector3(_horizontalInput, 0, 0);
	    animator.SetFloat("Blend", Mathf.Abs(_horizontalInput), 0f, Time.deltaTime);
	    // scale by speed
	    _moveDirection *= _data.MovementData.MoveSpeed;
 
	    // only align to motion if we are providing enough input
	    if (_moveDirection.magnitude > 0.05f)
	    {
		    gameObject.transform.forward = _moveDirection;
	    }
 
	    // allow jump as long as the player is on the ground
	    if (Input.GetButtonDown("Jump"))
	    {
		    // must have been grounded recently to allow jump
		    if (_groundedTimer > 0)
		    {
			    // no more until we recontact ground
			    _groundedTimer = 0;
			    animator.SetTrigger("Jump");
			    // Physics dynamics formula for calculating jump up velocity based on height and gravity
			    _verticalVelocity = _data.MovementData.JumpForce;
			    //verticalVelocity += Mathf.Sqrt(_data.MovementData.JumpForce * -Physics.gravity.y);
		    }
	    }
 
	    // inject Y velocity before we use it
	    _moveDirection.y = _verticalVelocity;
 
	    // call .Move() once only
	    characterController.Move(_moveDirection * Time.deltaTime);
    }
}