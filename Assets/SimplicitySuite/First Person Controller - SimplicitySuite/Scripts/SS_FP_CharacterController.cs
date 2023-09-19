using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace SimplicitySuite.FirstPersonController
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SS_Keybinds))]
    public class SS_FP_CharacterController : MonoBehaviour
    {
        //Player variables
        [Header("Movement Speed")]
        [Tooltip("The speed of the player when walking.")]
        [SerializeField] private float walkSpeed;                   //The walking speed of the player
        [Tooltip("The speed of the player when crouching")]
        [SerializeField] private float crouchSpeed;                 //The crouching speed of the player
        [Tooltip("The speed multiplier applied to the player when sprinting")]
        [SerializeField] private float sprintSpeedMultiplier;       //The multiplier applied when sprinting

        [Header("Movement Control")]
        [Tooltip("The rate at which the player accelerates")]
        [SerializeField] private float acceleration;                //The acceleration rate of the player
        [Tooltip("The rate at which the player decelerates")]
        [SerializeField] private float deceleration;                //The deceleration rate of the player

        [Header("Camera Control")]
        [Tooltip("The amount the head bobs up and down")]
        [SerializeField] private float headBobHeight;               //The height the head will bob up and down when moving

        [Header("Jumping")]
        [Tooltip("The height the player jumps when performing a regular jump")]
        [SerializeField] private float jumpHeight;                  //The heigh of the player's jump
        [Tooltip("The amount of control the player has in the air")]
        [SerializeField, Range(0f, 1f)] private float airControl;   //The amount of control the player has in the air
        [Tooltip("Determines whether the player can jump in the air")]
        [SerializeField] private bool canAirJump;                   //Determines if the player can jump in the air
        [Tooltip("The number of times the playere can jump before touching the ground")]
        [SerializeField] private int jumpCount;                     //The number of jumps the player can perform before touching the ground
        [Tooltip("The amount of stamina that gets used when jumping")]
        [SerializeField] private float staminaDrainJumping;         //The amount of stamina that gets used when jumping

        [Header("Stamina")]
        [Tooltip("Determines whether the player has a stamina system")]
        [SerializeField] private bool hasStamina;                   //Determines if the player has a stamina system
        [Tooltip("The maximum amount of stamina the player has")]
        [SerializeField] private float maxStamina;                  //The maximum amount of stamina the player has
        [Tooltip("The rate at which the player's stamina drains when performing certain actions")]
        [SerializeField] private float staminaDrain;                //The rate at which the player's stamina drains
        [Tooltip("The rate at which the player's stamina regenerates over time")]
        [SerializeField] private float staminaRegen;                //The rate at which the player's stamina regenerates
        [Tooltip("The time it takes until the stamina starts regenerating")]
        [SerializeField] private float staminaHoldTime;             //The time it takes until the stamina starts regenerating
        [Tooltip("Determines whether the player has to wait until they are at max stamina to start draining again")]
        [SerializeField] private bool mustWaitMaxStamina;           //Determines wheter teh player has to wait until they are at max stamina to start draining again

        [Header("Physics")]
        [Tooltip("The strength of gravity applied to the player")]
        [SerializeField] private float gravity;                     //The strength of gravity applied to the player
        [SerializeField] private Rigidbody rb;                      //The rigidbody component attached to the player

        [Header("Wall Interaction")]
        [Tooltip("Determines whether the player can interact with walls")]
        [SerializeField] private bool canWallInteract;              //Determines if the player can interact with walls
        [Tooltip("The height the player jumps when performing a wall jump")]
        [SerializeField] private int wallJumpHeight;                //The height of the player's wall jump
        [Tooltip("The amount of time the player can run on a wall before falling")]
        [SerializeField] private int wallRunTime;                   //The duration of the player's wall run

        [Header("Collision Detection")]
        [Tooltip("The layer used to detect ground collisions")]
        [SerializeField] private LayerMask groundLayer;             //The layer used to detect ground collisions
        [Tooltip("The distance from the player's feet to check for ground collisions")]
        [SerializeField] private float groundCheckDistance;         //The distance from the player's feet to check for ground collisions
        [Tooltip("The transform used to check for ground collisions")]
        [SerializeField] private Transform groundCheck;             //The transform used to check for ground collisions

        [Header("Miscellaneous")]
        [Tooltip("The height the player can step up without jumping")]
        [SerializeField] private float stepHeight;                  //The maximum height the player can step up without jumping
        [Tooltip("The maximum height the player can fall without taking damage")]
        [SerializeField] private float maxFallHeight;               //The maximum height the player can fall without taking damage
        
        [Header("UI")]
        [Tooltip("The stamina bar")]
        [SerializeField] private Slider staminaBar;
        
        //Keybinds
        private SS_Keybinds keybinds;

        //Movement inputs
        private float inputX;
        private float inputZ;
        private Vector3 movementInput;
        //Sprint variables
        private float sprintInput;
        //Air inputs
        private float yInput;
        //Ground check bool
        private bool isGrounded;
        //Jump variables
        private bool hasJumped;
        private int jumpCounter;
        //Head bob variables
        private float headBobTime;
        private bool headBobUp = true;
        //Actual speed variable
        private float actualSpeed;
        //Stamina variables
        private float currentStamina;
        private bool staminaRecharging;
        private bool drainingStamina;
        private float staminaHoldTimer;

        private bool canWallJump;
        private Vector3 wallNormal;

        private void Start()
        {
            //Get components
            rb = GetComponent<Rigidbody>();
            keybinds = GetComponent<SS_Keybinds>();

            //Set the current stamina to the maximum
            currentStamina = maxStamina;
        }

        //TO DO:
        //Step height | CURRENTLY USING A PHYSICS MATERIAL, NOT THE BEST SOLUTION
        //Crouching, Sliding
        //Wall interactions | Wall Jump, Wall run, Wall climb
        //Maybe mantling?
        //Health, damage, fall height, etc

        private void Update()
        {
            //Check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);

            //Run movement function
            Movement();

            //Run sprint function
            Sprint();

            //Run jump function
            Jump();

            //Run gravity function
            Gravity();

            //Run head bob function
            HeadBob();

            //Run stamina function
            Stamina();

            if (canWallInteract)
            {
                //Run wall jump function
                WallJump();
            }
            

            //Normalize the movement
            Vector3 normalizedMovement = new Vector3(Mathf.Abs(inputX), 0, Mathf.Abs(inputZ)).normalized;

            //If the player is sprinting, set their actual speed to their current speed * the sprint multiplier
            if (sprintInput > 0)
                actualSpeed = walkSpeed * sprintSpeedMultiplier * sprintInput + walkSpeed * (1 - sprintInput);
            //If the player is not sprinting, set their actual speed to their current speed
            else
                actualSpeed = walkSpeed;

            //Set the movement input to the inputs * the current movement speed
            movementInput = transform.TransformDirection(new Vector3(normalizedMovement.x * inputX, 0, normalizedMovement.z * inputZ)) * actualSpeed;

            //Increase movementInput by the gravity variable
            movementInput += new Vector3(0, yInput, 0);

            //Set the player's velocity
            rb.velocity = movementInput;
        }

        //Movement function
        private void Movement()
        {
            //MOVEMENT INPUTS
            //Increase inputZ by the acceleration amount every second when the back & forward keys are being held down
            if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "moveForward").key) && inputZ < 1)
                inputZ += Time.deltaTime * acceleration;
            else if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "moveBack").key) && inputZ > -1)
                inputZ -= Time.deltaTime * acceleration;
            else
            {
                //If the inputZ is low enough, set it to 0
                if (inputZ < 0.1f && inputZ > -0.1f)
                    inputZ = 0;
                //Decrease the inputZ when the back & forward keys are not being held down
                else
                    inputZ += Time.deltaTime * (-Mathf.Sign(inputZ) * deceleration);
            }

            //Increase inputX by the acceleration amount every second when the strafe keys are being held down
            if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "moveRight").key) && inputX < 1)
                inputX += Time.deltaTime * acceleration;
            else if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "moveLeft").key) && inputX > -1)
                inputX -= Time.deltaTime * acceleration;
            else
            {
                //If the inputX is low enough, set it to 0
                if (inputX < 0.1f && inputX > -0.1f)
                    inputX = 0;
                //Decrease the inputX when the strafe keys are not being held down
                else
                    inputX += Time.deltaTime * (-Mathf.Sign(inputX) * deceleration); 
            }
        }

        //Sprint function
        private void Sprint()
        {
            //Increase sprintInput by the acceleration amount every second wehn the sprint key is held down
            if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "sprint").key) && sprintInput < 1 && !staminaRecharging)
            {
                sprintInput += Time.deltaTime * acceleration;
            }
            else
            {
                //If the sprintInput is low enough, reset it to 0
                if (sprintInput < 0.1f)
                    sprintInput = 0;
                //Decrease the sprintInput when the sprint key is not held down
                else
                    sprintInput -= Time.deltaTime * deceleration;
            }

            //If the player is sprinting, start draining stamina
            if (Input.GetKey(keybinds.keybinds.Find(k => k.name == "sprint").key) && inputX != 0 && !staminaRecharging || Input.GetKey(keybinds.keybinds.Find(k => k.name == "sprint").key) && inputZ != 0 && !staminaRecharging)
                drainingStamina = true;
            //If the player is no longer sprinting, stop draining stamina
            else
                drainingStamina = false;
        }

        //Jump function
        private void Jump()
        {
            //If the player can jump in the air, check if they jump
            if (canAirJump)
            {
                //If the player has pressed the jump key and have not exceeded their jumpCounter, jump using the jumpHeight
                if (Input.GetKeyDown(keybinds.keybinds.Find(k => k.name == "jump").key) && jumpCounter < jumpCount && !staminaRecharging && currentStamina >= staminaDrainJumping)
                {
                    yInput = jumpHeight;
                    hasJumped = true;
                    //Add to the jumpCounter
                    jumpCounter++;

                    //Decrease currentStamina by the staminaDrainJumping
                    if (currentStamina > 0)
                    {
                        currentStamina -= staminaDrainJumping;
                        staminaHoldTimer = 0;
                    }
                }
            }
            //If the player can not jump in the air, check if they jump and are grounded
            else
            {
                //If the player has pressed the jump key and is grounded, jump using the jumpHeight
                if (Input.GetKeyDown(keybinds.keybinds.Find(k => k.name == "jump").key) && isGrounded && !staminaRecharging && currentStamina >= staminaDrainJumping)
                {
                    yInput = jumpHeight;
                    hasJumped = true;

                    //Decrease currentStamina by the staminaDrainJumping
                    if (currentStamina > 0)
                    {
                        currentStamina -= staminaDrainJumping;
                        staminaHoldTimer = 0;
                    }
                }
            }
        }

        //Gravity function
        private void Gravity()
        {
            //If the player is grounded, set the yInput to a low value as to always keep the player on the ground
            if (isGrounded && !hasJumped)
            {
                yInput = -0.15f;
                jumpCounter = 0;
            }
            //If the player is not grounded but has jumped, reset the hasJumped boolean to allow for re-jumping
            else if (!isGrounded && hasJumped)
            {
                hasJumped = false;
            }
            //If the player is not grounded, increase the yInput by the gravity variable
            else
            {
                //Increase the yInput by the gravity variable every second
                yInput += Time.deltaTime * gravity;
            }
        }

        //Head bob function
        private void HeadBob()
        {
            //Check if the player is grounded and moving
            if (isGrounded)
            {
                //If the player is moving, bob their head up and down
                if (inputX != 0 || inputZ != 0)
                {
                    //Bob head up until it reaches the bob height
                    if (headBobTime < headBobHeight / 2 && headBobUp)
                        headBobTime += Time.deltaTime * actualSpeed;
                    //Reverse direction if head bob has reached the bob height
                    else if (headBobTime >= headBobHeight / 2 && headBobUp)
                        headBobUp = false;
                    //Bob head down until it reaches the bob height
                    else if (headBobTime > -Mathf.Sign(headBobHeight) * headBobHeight / 2 && !headBobUp)
                        headBobTime -= Time.deltaTime * actualSpeed;
                    //Reverse direction if head bob has reached the bob height
                    else if (headBobTime <= headBobHeight / 2 && !headBobUp)
                        headBobUp = true;
                }
                //If the player has stopped moving, reset their headBobTime and the camera position
                else
                {
                    //Smoothly move the camera back to the default position
                    Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(0, 0.681225f, 0.1876417f), 0.5f * Time.deltaTime);

                    //Smoothly move the headBobTime with the camera
                    headBobTime = Mathf.MoveTowards(headBobTime, 0, 5 * Time.deltaTime * Mathf.Abs(5 - headBobTime));

                    //If the camera has reached its default position, reset the headBob direction to up
                    if (Camera.main.transform.localPosition == new Vector3(0, 0.681225f, 0.1876417f))
                        headBobUp = true;
                }
                //Move the camera along the headBobTime
                Camera.main.transform.Translate(Vector3.up * headBobTime * Time.deltaTime, Space.World);
            }
        }

        //Stamina function
        private void Stamina()
        {
            //Check if the player has to wait until they reach max stamina before they can start draining again
            if (!mustWaitMaxStamina)
                staminaRecharging = false;

            //If the player's stamina is not recharging but is draining, drain stamina
            if (!staminaRecharging)
            {
                //If stamina is being drained and the current stamina is greater than 0, drain the current stamina by the staminaDrain variable
                if (drainingStamina && currentStamina > 0)
                {
                    currentStamina -= staminaDrain * Time.deltaTime;
                }
                //If the current stamina is less than or equal to 0, start recharging stamina
                else if (currentStamina <= 0)
                {
                    staminaRecharging = true;
                }
            }

            //If the player is not draining stamina, go through recharging checks
            if (!drainingStamina)
            {
                //If the staminaHoldTimer has not reached the staminaHoldTime, wait until the specified staminaHoldTime has passed
                if (staminaHoldTimer < staminaHoldTime)
                {
                    staminaHoldTimer += Time.deltaTime;
                }
                else
                {
                    //If the currentStamina is not set to max, increase it by the staminaRegen * Time.deltaTime
                    if (currentStamina < maxStamina)
                    {
                        currentStamina += staminaRegen * Time.deltaTime;
                    }
                    //If the currentStamina has reached the maxStamina, stop recharging
                    else
                        staminaRecharging = false;
                }
            }
            //If the player is draining stamina, reset the staminaHoldTimer
            else
                staminaHoldTimer = 0;

            //Set the staminaBar.value to the currentStamina
            staminaBar.value = currentStamina / maxStamina;
        }

        //Wall jump function
        private void WallJump( )
        {
            RaycastHit hit;

            //Check if the player is close to and moving towards a wall
            if (Physics.Raycast(transform.position, new Vector3(rb.velocity.normalized.x, 0, rb.velocity.normalized.z), out hit, 2))
            {
                //If the player is not grounded and tries to jump, jump
                if (!isGrounded && Input.GetKeyDown(keybinds.keybinds.Find(k => k.name == "jump").key) && !staminaRecharging && currentStamina >= staminaDrainJumping)
                {
                    //Launch the player up by the jumpHeight
                    yInput = jumpHeight;

                    //Decrease currentStamina by the staminaDrainJumping
                    if (currentStamina > 0)
                    {
                        currentStamina -= staminaDrainJumping;
                        staminaHoldTimer = 0;
                    }
                }
            }
        }
    }
}