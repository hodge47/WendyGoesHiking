
//Glide Player Controller Developed by John Ellis, 2019.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GlideController : MonoBehaviour
{

    PlayerControlActions playerControls;
    public StaminaSystem staminaSystem;


    public static GlideController current; //Just call GlideController.current to access the current player's variables from any other script!

    /*
        NOTICE:
        - Player rotation is divided between two separate parts! The camera deals with pitch, and the body deals with yaw. Remember this!
        - Tooltips are there for a reason! Mouse over a variable in the inspector for more information on how to use it!
        - Bugs suck! Report anything you run into and I will get onto it as soon as possible!
        - For player sounds, the Player Controller needs an AudioManager component attached to the same object! Just use the prefab provided for reference.
    */

    [Header("Glide")]
    [Tooltip("Switch between first and third person! Made this an enum in case other types of 3D player type are implemented into the tool...")] public GlideMode playerMode = GlideMode.firstPerson;
    [Min(0f)] public float playerRadius = 0.3f;
    [Min(0f)] public float playerHeight = 1.64f;

    [Header("Glide - Controls")]
    [Tooltip("Changes how jumping will behave.\n\nNone: Disables jumping entirely.\n\nNormal: Jumping is unaffected by anything else. Just pure jump power, baby.\n\nEnhanced: If you're sprinting, your jump power is scaled to be 115% of the original. If you aren't sprinting, it's just normal.\n\nLeaping: Suggested by Aaron. Leaping will make it so that if you're sprinting, you'll leap in the direction you're running, scaled by 2x the current sprint speed. Good for games focused on mobility. If you aren't sprinting, things are normal.")] public GlideJump jumpMode = GlideJump.enhanced;
    [Space]
    [Tooltip("Changes how sprinting will behave.\n\nNone: Disables sprinting entirely.\n\nNormal: Sprinting engages when the sprint key is held down, and stops when you release.\n\nClassic: If you're walking, pressing the sprint key engages sprintng. Releasing it won't stop sprinting, it's only when you stop walking that sprinting will stop.")] public GlideSprintSetting sprintMode = GlideSprintSetting.normal;
    [Space]
    [Tooltip("The key for crouching.")] private KeyCode crouchButton = KeyCode.LeftControl;
    [Tooltip("Changes how crouching will behave.\n\nNone: Disables crouching entirely.\n\nNormal: Crouching won't affect how sprinting behaves.\n\nNo Sprint: Sprinting cannot be enabled while you're crouching.")] public GlideCrouchSetting crouchMode = GlideCrouchSetting.normal;
    [Space]
    [Tooltip("The key for sliding.")] private KeyCode slideButton = KeyCode.C;
    [Space]
    [Tooltip("How sensitive the player's mouse is.")] public float mouseSensitivity = 1.5f;

    [Header("Glide - Movement")]
    [Tooltip("Whether or not the player can walk around, sprint or jump.")] public bool lockMovement = false;
    [Space]
    [Tooltip("The amount of gravity the player experiences per frame.")] public float gravity = 0.1f;
    [Tooltip("The maximum fall speed possible.")] public float gravityCap = -100f;
    [Tooltip("0.01 means that the player will control like they're on ice, and 1 means the player moves almost instantaneously in the direction you choose. Your input settings account for friction as well, so check Edit>Project Settings>Input for more information.")] [Range(0.01f, 1f)] public float movementShiftRate = 0.2f;
    [Tooltip("The amount of control a player gets in the air. 0 means absolutely none, 1 means same amount as on the ground.")] [Range(0f, 1f)] public float airControl = 1f;
    [Space]
    [Tooltip("How fast the player is when walking normally.")] public float moveSpeed = 5f;
    [Tooltip("How fast the player is when crouch-walking like a creepy little crab.")] public float crouchSpeed = 3f;
    [Tooltip("How fast the player moves when sprinting.")] public float sprintSpeed = 8f;
    [Tooltip("How fast the player moves when sliding.")] public float slideSpeed = 20f;
    [Space]
    [Tooltip("The amount of force the player jumps with.")] public float jumpPower = 4f;
    [Space]
    [Tooltip("Whether or not the player is affected by slopes.")] public bool slidingOnSlopes = true;
    [Tooltip("Determines at what normal value the player will slide down a sloped surface (assuming slidingOnSlopes is enabled). The higher this is, the steeper a surface a player can walk up.")] [Range(0f, 1f)] public float slopeBias = 0.7f;
    [Tooltip("A precentage of the player's current height that represents how low they can crouch.")] [Range(0.5f, 1f)] public float crouchPercent = 0.4f;
    [Tooltip("A precentage of the player's current height that represents how low they slide.")] [Range(0.01f, 1f)] public float slidePercent = 0.5f;

    [Tooltip("The length of time (in seconds) that a slide lasts")] public float lengthOfSlide = 1;

    public Vector3 movement; //The vector3 representing player motion.

    [Header("Glide - Camera")]
    [Tooltip("Whether or not the player can look around or zoom in.")] public bool lockCamera = false;
    [Space]
    [Tooltip("A direct reference to the player's camera.")] public Camera playerCamera;
    [Tooltip("If this is enabled: Left-Clicking on the screen will lock your cursor in. Pressing Escape unlocks the cursor.")] public bool cursorManagement = true;
    [Tooltip("The angles at which the player's camera can look up and down. For technical reasons, third person mode completely ignores this and uses its own restraints.")] public Vector2 verticalRestraint = new Vector2(-90f, 90f);
    [Space]
    [Tooltip("Whether or not the camera bobs at all.")] public bool enableViewbob;
    [Tooltip("The rate at which the player's camera bobs.")] public float viewBobRate = 1f;
    [Tooltip("The intensity at which the player's camera bobs.")] public float viewBobPower = 1f;
    [Space]
    [Tooltip("Whether or not the camera should respond to landing.")] public bool landingEffects;
    [Space]
    [Tooltip("If true, the system will always bring the camera close to the player if something is blocking it.\n\nIf false, the system won't bring the camera in if a non-kinematic rigidbody is between the player and the camera.")] public bool rigidbodyOcclusion = true;
    [Tooltip("How far the third person camera should be from the player while in third person mode.")] public float thirdPersonOrbitDistance;

    [Header("Glide - Zoom")]
    [Tooltip("The FOV the camera will set to when the player sprints. To disable this effect, just set it to the same value as the default FOV!")] public float sprintIntensity = 15f;
    [Tooltip("The FOV the camera will set to when the player slides. To disable this effect, just set it to the same value as the default FOV!")] public float slideFOV = 95f;
    [Tooltip("This value represents how many degrees the zoom changes. The higher this value is, the further in the camera will zoom.")] public float zoomIntensity = 30f;

    [Header("Glide - Sounds")]
    [Tooltip("Whether or not sounds will play from the player.")] public bool enableSounds = true;
    [Space]
    [Tooltip("How loud the player's sounds are.")] [Range(0f, 1f)] public float soundVolume = 1f;
    [Tooltip("How long the player will wait before allowing the landing sound to play again. Check m_landingTimer in the code for more details.")] public float landingSoundTimer = 1f;
    [Tooltip("The sound that plays whenever the player walks. The rate this plays at is scaled based on speed.")] public AudioClip[] walkSounds;
    [Tooltip("The sound that plays whenever the player jumps.")] public AudioClip jumpingSound;
    [Tooltip("The sound that plays whenever the player lands on the ground.")] public AudioClip landingSound;
    [Tooltip("The sound that plays whenever the player slides.")] public AudioClip slidingSound;

    [Header("Glide - Animation")]
    [Tooltip("^ Leave this unassigned to ignore animations. ^\n\nExplanation of used animator parameters below:")] public Animator playerAnimator; //This is important, please read the tooltip for a comprehensive list of the animator parameters used.
    [Space]
    [Tooltip("^ Leave this empty to ignore parameter.^\n\nWalking: Boolean, whether or not the player is moving.")] public string walkingParameter = string.Empty;
    [Tooltip("^ Leave this empty to ignore parameter.^\n\nSprinting: Boolean, whether or not sprinting is active.")] public string sprintingParameter = string.Empty;
    [Tooltip("^ Leave this empty to ignore parameter.^\n\nCrouching: Boolean, whether or not the player is crouching.")] public string crouchingParameter = string.Empty;
    [Tooltip("^ Leave this empty to ignore parameter.^\n\nGrounded: Boolean, whether or not the player is on the ground.")] public string groundedParamter = string.Empty;
    [Tooltip("^ Leave this empty to ignore parameter.^\n\nRelativeSpeed: Float, a percentage comparing the player's current speed to the normal walk speed. Used either for smooth animation blending or directly adjusting the speed of a walking animation.")] public string relativeSpeedParameter = string.Empty;
    [Space]
    [Tooltip("^ Leave this empty to ignore weight.^\n\nCrouching Percent: Float, scales from 0 to 1, representing how crouched the player is.")] public string crouchingWeight = string.Empty;
    [Space]
    [Tooltip("In circumstances where you would rather have your animator manage step sounds via animation events, you can mark this true.")] public bool overrideFootsteps = false;

    [Header("Glide - Misc.")]
    [Tooltip("Whether or not the game should have its framerate locked.")] public bool lockFramerate = true;
    [Tooltip("The framerate the game will be locked at whenever a player is spawned in. Requires lockFramerate to be active first.")] [Range(1, 60)] public int frameRate = 60;

    [HideInInspector]
    public bool isGrounded = false; //Internal boolean to tell whether or not the player is on the ground. Depends on several conditions seen in the code below.
    [HideInInspector]
    public bool isCrouching = false; //Internal boolean, name is self explanatory.
    [HideInInspector]
    public bool isSprinting; //Whether or not the player is sprinting.
    [HideInInspector]
    public bool isSliding; //Whether or not the player is sliding. 

    private float slide_time;
    private Vector3 storedAngles;
    private Vector3 slide_direction;

    /// Private variables.
    private float FOV = 60f; //The default FOV value the camera will be set to. 
    private bool m_stepped = false; //Used per-frame to play walking sounds. Prevents rapid-fire sound being played.
    private bool m_canJump; //Decided based on the angle of the ground below the player.
    private float m_zoomAdditive; //Added onto the current FOV value to decide how far the camera is zoomed in/out.
    private float m_zoomGoal; //The total amount of zoom m_zoomAdditive is trying to reach.
    private float m_topSpeed; //The maximum amount of speed the player may move at any time. Doesn't include vertical speed.
    private float m_grav; //The current gravity affecting the player. Increases by the gravity value above every frame.
    private float m_lastGrav; //The gravity as of last frame. Used for a handful of calculations.
    private Vector3 m_goalAngles; //The true angles of the camera. Essentially where the camera will lerp to in some frames.
    private Vector3 m_camOrigin; //The original local position of the player's camera.
    private Vector3 m_camPosTracer; //Internal vector3 to lerp the camera's position with. Designed to smooth landing effects.
    private float m_landingTimer; //Internal timer to prevent the landing sound from playing 1000 times every time a series of landing events are registered. Comparable to a debouncing script.
    private float m_camOriginBaseHeight; //Essentially, the best method of keeping the camera from messing up while crouching is to modify the camOrigin's y value if we're crouched or not. This is the base.
    private float m_jumpTimer; //Prevents key bouncing issues;
    private Vector3 m_animatorObjectOrigin = Vector3.zero; // This is used to maintain the visual aspects of the player while crouching.
    private bool m_sliding; //Used when blocking player motion up a slope.
    private Vector3 m_slidingNormal; //The normal of the slope the player is touching.

    RaycastHit m_hit; //A cached raycast for performance. The script recycles this often to avoid allocation issues.

    private Rigidbody m_rig; //Reference to the rigidbody component attached to the player object;
    private CapsuleCollider m_capsule; //Reference to the capsule collider component, cached for performance reasons.

    public Transform weaponParent;
    private Vector3 weaponParentOrigin;
    private Vector3 weaponParentBase;

    //jpostAudio
    public FootstepManager footstepManager;
    

    ///

    // Weapon Variables
    private float movementCounter;  //weapon bob var
    private float idleCounter;      //weapon bob var
    private float sprintCounter;   //weapon bob var
    private Vector3 targetWeaponBobPosition;

    // Input
    private PlayerControlActions playerControlActions;

    void Start()
    {
        FOV = playerCamera.fieldOfView;

        /// Initialization details.
        current = this; //The current player controller is assigned so you can access it whenever you need to.

        gameObject.GetComponent<AudioManager>().Init(); //The AudioManager is initialized so that player sounds can be objectpooled.

        m_topSpeed = moveSpeed; //The top speed is set to the default moveSpeed.

        m_goalAngles = playerCamera.gameObject.transform.rotation.eulerAngles; //Goal angles are based on the player camera's rotation.
        m_camOrigin = playerCamera.transform.localPosition; //The camera's origin is cached so bobbing and landing effects can be applied without the camera losing its default position.
        m_camOriginBaseHeight = m_camOrigin.y;
        m_camPosTracer = m_camOrigin; //This is where the camera truly lies in a given frame.

        Cursor.lockState = CursorLockMode.Locked; //Cursor is locked.
        Cursor.visible = false; //Cursor is hidden.



        if (playerAnimator != null) //This is used to help keep animator objects in place when crouching.
            m_animatorObjectOrigin = playerAnimator.transform.localPosition;

        switch (playerMode) //This is where we set the camera up for third person, popping it off of the player and getting the position set correctly before the game starts.
        {
            case (GlideMode.thirdPerson):
                playerCamera.transform.SetParent(null);

                Vector3 m_projection = new Vector3(Mathf.Cos((-m_goalAngles.y - 90f) * Mathf.Deg2Rad) * Mathf.Cos(m_goalAngles.z * Mathf.Deg2Rad), Mathf.Sin(m_goalAngles.x * Mathf.Deg2Rad), Mathf.Cos(m_goalAngles.x * Mathf.Deg2Rad) * Mathf.Sin((-m_goalAngles.y - 90f) * Mathf.Deg2Rad));
                m_goalAngles.x = Mathf.Clamp(m_goalAngles.x, -70f, 70f); //The camera angles are restricted here, so the player can't flip their head completely down and snap their neck.

                playerCamera.transform.position = transform.position + (m_projection * thirdPersonOrbitDistance) + movement * Time.deltaTime;
                break;
        }


        m_capsule = gameObject.AddComponent<CapsuleCollider>();
        m_capsule.radius = playerRadius;
        m_capsule.height = playerHeight;
        PhysicMaterial m_phy = new PhysicMaterial
        {
            dynamicFriction = 0f,
            staticFriction = 0f,
            frictionCombine = PhysicMaterialCombine.Minimum

        };

        m_capsule.material = m_phy;

        m_rig = gameObject.AddComponent<Rigidbody>();
        m_rig.freezeRotation = true;
        m_rig.useGravity = false;
        m_rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        ///Framerate locking, if you so please.
        if (lockFramerate)
        {
            if (frameRate < 60) //If the framerate is below sixty, we have to disable V-Sync to forcibly change the framerate.
            {
                QualitySettings.vSyncCount = 0;
            }

            Application.targetFrameRate = frameRate; //Framerate is set at the value you specified above.
        }
        ///


        weaponParentOrigin = weaponParent.localPosition;
        weaponParentBase = weaponParentOrigin;

        // Initialize input
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
        // Load the player's bindings
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            string saveData = PlayerPrefs.GetString("InputBindings");
            playerControlActions.Load(saveData);
        }

        //jpost Audio initialization
        footstepManager = GetComponentInChildren<FootstepManager>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1)) {                                 /////////////////////////////////////////////////////// GET RID OF THIS
            Scene s = SceneManager.GetActiveScene();
            SceneManager.LoadScene(s.name);
        }

        ///Crouching control/logic (check just a bit further down for the part where the height is actually affected!)
        if (crouchMode != GlideCrouchSetting.none) //Assuming we're actually letting the player crouch
        {
            if (!Physics.SphereCast(new Ray(transform.position, Vector3.up), playerRadius * 0.9f, playerHeight * 0.52f)) //We fire a check above the player to make sure they're not trying to stand up while a ceiling is present.
            {
                isCrouching = Input.GetKey(crouchButton); //Check if the player wants to crouch.

                if (lockMovement)
                    isCrouching = false; //If our movement is locked, we can prevent crouching.
            }
            else
            {
                if (Input.GetKeyDown(crouchButton))
                    isCrouching = true;

                if (Mathf.Abs(m_capsule.height - playerHeight) > 0.05f)
                    isCrouching = true;

            }
        }
        else
        {
            isCrouching = false;
        }
        ///


        // MOVEMENT MANAGEMENT

        float m_shift = 0f;

        ///Crouch motion
        if (isCrouching)
        {


            weaponParentBase = weaponParentOrigin + Vector3.down * crouchPercent * 1.625f;

            if (Mathf.Abs((playerHeight * crouchPercent) - m_capsule.height) > 0.02f) //If we're outside the snapping threshold for crouching, we do the following:
            {
                m_shift = ((playerHeight * crouchPercent) - m_capsule.height) * 0.1f; //We get the difference between the current height and the goal here, then multiply by a smoothing value.
                m_capsule.height += m_shift; //Apply the shift to the current height.
                transform.position += Vector3.up * m_shift * 0.5f; //Position is shifted to help keep crouching smooth.
            }
            else
                m_capsule.height = playerHeight * crouchPercent; //Snap to the crouch height if we're within the threshold.
        }
        else
        {
            weaponParentBase = weaponParentOrigin;

            if (Mathf.Abs((playerHeight) - m_capsule.height) > 0.02f) //When returning to the normal height, check to make sure we're outside of the snapping threshold. If so, do the following:
            {
                m_shift = (playerHeight - m_capsule.height) * 0.1f; //Get the difference between our current height and the normal height.
                m_capsule.height += m_shift; //Shift the position to keep the transition smooth.
                transform.position += Vector3.up * m_shift * 0.3f; //Shift the height up.
            }
            else
                m_capsule.height = playerHeight; //Snap to the normal height if we're within the threshold.
        }



        if (crouchMode != GlideCrouchSetting.none)
        {
            if (playerAnimator != null)
            {
                //This section is dedicated to shifting the animator object so it doesn't sink into the ground when crouching.
                playerAnimator.transform.localPosition = new Vector3(m_animatorObjectOrigin.x, m_animatorObjectOrigin.y + (playerHeight - m_capsule.height) * 0.5f, m_animatorObjectOrigin.z);
                //This is where we assign the crouching weight in case you want to transition to crouched variants of your current animations.

                if (crouchingWeight != string.Empty)
                    playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex(crouchingWeight), 1f - Mathf.Abs(playerHeight * crouchPercent - m_capsule.height) / Mathf.Abs(playerHeight * crouchPercent - playerHeight));

            }
        }
        ///

        /// Sprinting. Code for managing the camera's FOV, the methods sprinting can be enabled, and the player's top speed.
        switch (sprintMode)
        {
            case (GlideSprintSetting.normal):
                if (Mathf.Abs(playerControlActions.MoveY) > 0 && staminaSystem.hasEnoughStamina)
                    isSprinting = playerControlActions.Sprint.IsPressed;
                else
                    isSprinting = false;
                break;

            case (GlideSprintSetting.classic):
                if (playerControlActions.Sprint.IsPressed && Mathf.Abs(playerControlActions.MoveY) > 0)
                    isSprinting = true;
                if ((Mathf.Abs(playerControlActions.MoveX) + Mathf.Abs(playerControlActions.MoveY)) * 0.5f < 0.5f) //Player's intended movement is averaged on intensity and analyzed. If it falls below a threshold, sprinting turns off.
                    isSprinting = false;
                break;
        }

        //Added in to work with the StaminaSystem
        if (!staminaSystem.hasEnoughStamina)
            isSprinting = false;

        if (crouchMode == GlideCrouchSetting.noSprint && isCrouching)
            isSprinting = false;

        playerCamera.fieldOfView += ((FOV + m_zoomAdditive) - playerCamera.fieldOfView) * 0.2f;

        if (!isCrouching)
        {
            if (isSprinting)
                m_topSpeed = sprintSpeed;
            else
                m_topSpeed = moveSpeed;
        }
        else
        {
            if (isSprinting)
                m_topSpeed = crouchSpeed * (sprintSpeed / moveSpeed);
            else
                m_topSpeed = crouchSpeed;
        }
        ///



        /// Sliding 

        if (Input.GetKey(slideButton) && !isSliding && isSprinting)
        {
            slide_time = 0.0f;
            isSliding = true;
            slide_direction = movement;
            AudioManager.PlaySound(slidingSound, soundVolume);

        }

        if (isSliding)
        {


            if (Mathf.Abs((playerHeight * slidePercent) - m_capsule.height) > 0.02f) //If we're outside the snapping threshold for crouching, we do the following:
            {
                m_shift = ((playerHeight * slidePercent) - m_capsule.height) * 0.1f; //We get the difference between the current height and the goal here, then multiply by a smoothing value.
                m_capsule.height += m_shift; //Apply the shift to the current height.
                transform.position += Vector3.up * m_shift * 0.5f; //Position is shifted to help keep crouching smooth.
            }
            else
            {
                m_capsule.height = playerHeight * slidePercent; //Snap to the crouch height if we're within the threshold.
            }

            weaponParentBase = weaponParentOrigin + Vector3.down * slidePercent * 1.625f;


            // m_capsule.height = playerHeight * slidePercent; //Snap to the crouch height if we're within the threshold.

            // m_capsule.height += ((m_camOriginBaseHeight * slidePercent) - m_capsule.height) * 0.1f;
            m_topSpeed += (slideSpeed - m_topSpeed) * 0.2f;
            playerCamera.fieldOfView += ((slideFOV + m_zoomAdditive) - playerCamera.fieldOfView) * 0.2f;
            storedAngles = m_goalAngles;
            m_goalAngles = new Vector3(m_goalAngles.x, m_goalAngles.y, -10);

            slide_time += Time.deltaTime;
            if (slide_time > lengthOfSlide)
            {
                isSliding = false;
                m_goalAngles = new Vector3(m_goalAngles.x, m_goalAngles.y, 0);
            }
        }
        else if (!isSliding && !isCrouching)
        {
            if (Mathf.Abs((playerHeight) - m_capsule.height) > 0.02f) //When returning to the normal height, check to make sure we're outside of the snapping threshold. If so, do the following:
            {
                m_shift = (playerHeight - m_capsule.height) * 0.1f; //Get the difference between our current height and the normal height.
                m_capsule.height += m_shift; //Shift the position to keep the transition smooth.
                transform.position += Vector3.up * m_shift * 0.3f; //Shift the height up.
            }
            else
                m_capsule.height = playerHeight; //Snap to the normal height if we're within the threshold.
        }





        /// General Motion.
        if (!lockMovement) //If the movement isn't locked, manage player controls to figure out where they want to go.
        {
            Vector3 m_attemptVelocity = Vector3.zero;

            switch (playerMode)
            {
                case (GlideMode.firstPerson):

                    m_attemptVelocity = (gameObject.transform.TransformDirection(new Vector3(playerControlActions.MoveX * m_topSpeed, 0f, playerControlActions.MoveY * m_topSpeed)) - movement) * movementShiftRate * ((!isGrounded) ? airControl : 1f);

                    if (m_sliding)
                    {
                        m_canJump = false;
                        Vector3 m_pos = m_slidingNormal;

                        //For sliding, we make sure we block the player from pressing into a slope by clamping their velocity. I cleaned this code up and condensed it into a new Vector3() declaration.

                        m_attemptVelocity = new Vector3(
                            (m_slidingNormal.x >= 0f) ? Mathf.Clamp(m_attemptVelocity.x, 0f, Mathf.Infinity) : Mathf.Clamp(m_attemptVelocity.x, -Mathf.Infinity, 0f)
                            ,
                            m_attemptVelocity.y
                            ,
                            (m_slidingNormal.z >= 0f) ? Mathf.Clamp(m_attemptVelocity.z, 0f, Mathf.Infinity) : Mathf.Clamp(m_attemptVelocity.z, -Mathf.Infinity, 0f)
                            );


                    }

                    break;

                case (GlideMode.thirdPerson):
                    m_attemptVelocity = (new Vector3((playerControlActions.MoveY * m_topSpeed * Mathf.Cos(Mathf.Deg2Rad * (-m_goalAngles.y + 90f))) + (playerControlActions.MoveX * m_topSpeed * Mathf.Cos(Mathf.Deg2Rad * (-m_goalAngles.y))), 0f, (playerControlActions.MoveY * m_topSpeed * Mathf.Sin(Mathf.Deg2Rad * (-m_goalAngles.y + 90f))) + (playerControlActions.MoveX * m_topSpeed * Mathf.Sin(Mathf.Deg2Rad * (-m_goalAngles.y)))) - movement) * movementShiftRate * ((!isGrounded) ? airControl : 1f);

                    if (m_sliding)
                    {
                        m_canJump = false;
                        Vector3 m_pos = m_slidingNormal;

                        //For sliding, we make sure we block the player from pressing into a slope by clamping their velocity. I cleaned this code up and condensed it into a new Vector3() declaration.
                        m_attemptVelocity = new Vector3(
                            (m_slidingNormal.x >= 0f) ? Mathf.Clamp(m_attemptVelocity.x, 0f, Mathf.Infinity) : Mathf.Clamp(m_attemptVelocity.x, -Mathf.Infinity, 0f)
                            ,
                            m_attemptVelocity.y
                            ,
                            (m_slidingNormal.z >= 0f) ? Mathf.Clamp(m_attemptVelocity.z, 0f, Mathf.Infinity) : Mathf.Clamp(m_attemptVelocity.z, -Mathf.Infinity, 0f)
                            );
                    }

                    break;
            }

            movement += m_attemptVelocity;

        }
        else
            movement += (Vector3.zero - movement) * movementShiftRate; //Assuming movement's locked, the player is recursively slowed down to zero.
        ///

        movement.y = m_grav; //The player's current gravity is applied to the movement vector. This is done because CharacterControllers take control over the y value, and managing gravity in a different variable and reassigning gives us the control instead. We can leave this alone for the sake of rigidbody physics as well.


        //Checking for ground.
        if (Physics.SphereCast(transform.position, playerRadius * 0.99f, Vector3.down, out m_hit, m_capsule.height * 0.51f, ~0, QueryTriggerInteraction.Ignore))
        {
            if (m_hit.normal.y > slopeBias)
                isGrounded = true;
        }
        else
            isGrounded = false;

        if (m_sliding)
            isGrounded = false;

        /// This snaps the player down to a surface if the conditions are just right. Needed on slopes to prevent the player from sliding off of a surface and floating down to the ground instead of, you know, walking down the slope like a normal human.
        float snapDistance = (playerHeight * 0.66f); //How far down the player will search for snappable terrain.
        if (!isGrounded && m_grav < 0f) //If we're midair and we're also falling down.
        {

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, snapDistance, ~0, QueryTriggerInteraction.Ignore)) //A raycast is fired below the player.
            {

                if (hitInfo.normal.y > slopeBias)
                {
                    if ((1f - hitInfo.normal.y) < slopeBias) //If the surface is flat enough, 
                        m_rig.MovePosition(transform.position + ((hitInfo.point + (Vector3.up * m_capsule.height * 0.5f)) - transform.position)); //The player is shifted down to the surface for landing.

                    isGrounded = true;
                }

            }

        }
        ///

        /// Ceiling bumping. Prevents that obnoxious "sticky" feel you get whenever you jump into something above you.

        if (Physics.Raycast(transform.position, Vector3.up, out m_hit, m_capsule.height * 0.55f, ~0, QueryTriggerInteraction.Ignore))
        {
            if (m_hit.collider.GetComponent<Rigidbody>() != null)
            {
                if (m_hit.collider.GetComponent<Rigidbody>().isKinematic)
                    if (m_grav > 0f) m_grav = 0f;
            }
            else
                if (m_grav > 0f) m_grav = 0f;
        }

        ///


        /*
         * Gravity is affected here. We clamp it so if there's a bug, the gravity won't grow so great that players just fly through geometry.
         * Think of it like terminal velocity, if you drop an object it doesn't just keep accelerating until it punches a hole through the Earth, right?
        */

        m_grav = Mathf.Clamp(m_grav - gravity, gravityCap, Mathf.Infinity);


        /*
         * We use this in case jump controls get rapid inputs, whether this is a hardware fault or a deliberate action.
         * If this weren't here, it'd be possible to chain enough jumps in the frames before the player is no longer considered "grounded" to fly into the air.
        */
        if (m_jumpTimer > 0f)
            m_jumpTimer -= Time.deltaTime;

        /// Jumping management.
        if (!isGrounded)
        {
            if (!lockMovement)
            {
                if (m_grav > 0f &&  playerControlActions.Jump.WasPressed) //Whenever the player is in midair and going up, we allow them to halve their vertical speed by releasing the jump button.
                    m_grav -= m_grav * 0.5f;
            }

        }
        else
        {

            if (!lockMovement)
            {
                if (jumpMode != GlideJump.none)
                {
                    if (m_canJump)
                    {
                        if (playerControlActions.Jump.IsPressed)
                        {
                            if (m_jumpTimer <= 0f)
                            {
                                if (enableSounds)
                                    AudioManager.PlaySound(jumpingSound, soundVolume);

                                switch (jumpMode)
                                {
                                    case (GlideJump.normal):
                                        m_grav = jumpPower; //No matter what, you will always jump with consistent power.
                                        break;

                                    case (GlideJump.enhanced):
                                        m_grav = jumpPower + ((isSprinting) ? jumpPower * 0.15f : 0f); //Jumppower is scaled up if enhanced jumping is enabled.
                                        break;

                                    case (GlideJump.leaping):
                                        m_grav = jumpPower; //Jumping is normal here, but the next line flings the player if they're sprinting.
                                        if (isSprinting)
                                        {
                                            Vector3 m_applied = Vector3.zero;
                                            switch (playerMode)
                                            {
                                                case (GlideMode.firstPerson):
                                                    m_applied = transform.TransformDirection(new Vector3(playerControlActions.MoveX * sprintSpeed * 2f, 0f, playerControlActions.MoveY * sprintSpeed * 2f));
                                                    m_applied.y = jumpPower * 0.5f;
                                                    movement += m_applied;
                                                    break;

                                                case (GlideMode.thirdPerson):
                                                    m_applied = new Vector3((playerControlActions.MoveY * sprintSpeed * 2f * Mathf.Cos(Mathf.Deg2Rad * (-m_goalAngles.y + 90f))) + (playerControlActions.MoveX * sprintSpeed * 2f * Mathf.Cos(Mathf.Deg2Rad * (-m_goalAngles.y))), 0f, (playerControlActions.MoveY * sprintSpeed * 2f * Mathf.Sin(Mathf.Deg2Rad * (-m_goalAngles.y + 90f))) + (playerControlActions.MoveX * sprintSpeed * 2f * Mathf.Sin(Mathf.Deg2Rad * (-m_goalAngles.y))));
                                                    m_applied.y = jumpPower * 0.5f;
                                                    movement += m_applied;
                                                    break;
                                            }


                                        }
                                        break;
                                }

                                m_jumpTimer = 0.1f;
                            }
                        }
                    }
                }
            }


            /*
             * If the surface below us is flat enough to count as ground, we clamp the player's gravity to prevent it 
             * from stacking up and making the player fall through terrain!
             * 
             * We leave some gravity so when the player walks down a slope, they move to meet the terrain. Otherwise they'd walk off slopes like they were flat
             * ground, and this looks extremely ugly.
            */

            m_grav = Mathf.Clamp(m_grav, -0.5f, Mathf.Infinity);
            RaycastHit m_slope;
            if (Physics.Raycast(transform.position, Vector3.down, out m_slope, playerHeight * 0.66f, ~0, QueryTriggerInteraction.Ignore))
            {
                if (m_slope.normal.y > slopeBias && m_grav > -2f && m_grav < 0f && m_slope.normal.y < 0.99f) //If we're not set to slide down the slope normally
                    transform.position = new Vector3(transform.position.x, m_slope.point.y + playerHeight * 0.55f, transform.position.z);
            }
        }
        ///

        // CAMERA MANAGEMENT

        m_camOrigin.y = m_camOriginBaseHeight * (m_capsule.height / playerHeight);


        /// Cursor Management. If you feel like you want to take control over the cursor more, you can disable cursorManagement in the inspector.
        if (cursorManagement)
        {
            if (Input.GetMouseButtonDown(0)) //Here we lock the mouse whenever the player clicks the screen.
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) //Press escape to unlock the cursor.
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        ///

        if (!lockCamera)
        {

            /// Controls landing effects (if enabled). Uses m_lastGrav to compare against the player's previous gravity and figure out if they've landed.
            if (m_landingTimer > 0f)
            {
                if (!isGrounded)
                    m_landingTimer -= Time.deltaTime; //If we're not on the ground, we count the timer down for landing effects! This makes it so that small drops don't cause the landing sound to play.
                else
                    m_landingTimer = landingSoundTimer; //The timer is reset if we hit the ground.
            }

            if (m_grav - m_lastGrav > 5f && m_lastGrav < -5f && isGrounded) //This checks to make sure the player is falling a certain speed before using landing effects.
            {
                if (enableSounds)
                {
                    if (m_landingTimer <= 0f)
                    {
                        AudioManager.PlaySound(landingSound, soundVolume); //If we're allowed to play sounds on landing, we do it here. The timer is reset to prevent spamming.
                        m_landingTimer = landingSoundTimer;
                    }
                }

                if (landingEffects)
                {
                    m_camPosTracer -= Vector3.up * -m_lastGrav * 0.3f; //If effects are enabled, the camera is bowed down for the landing.
                    m_camPosTracer.y = Mathf.Clamp(m_camPosTracer.y, -4f, 4f); //This effect is clamped to prevent the camera from bowing too low.
                }
            }
            ///

            /// Dedicated to controlling viewbobbing and walkSounds.
            if (!lockMovement)
            {
                if (isGrounded)
                {
                    if (new Vector3(playerControlActions.MoveX, 0f, playerControlActions.MoveY).magnitude > 0.3f)
                    {
                        /// Viewbob effects and management for the m_walkTime value. Scales relative to your current speed and however fast you normally move, meaning a low moveSpeed and high sprintSpeed result in quick footsteps.
                        float m_walkTime = 0f;
                        m_walkTime += viewBobRate * Time.time * ((isSprinting) ? (10f + (sprintSpeed / moveSpeed) * 2f) : 10f); //m_walkTime controls the viewbob's rate! It's scaled depending on speed.

                        if (enableViewbob && !WeaponManager.current.isAiming)
                            m_camPosTracer += (m_camOrigin + (Vector3.up * viewBobPower * Mathf.Sin(m_walkTime) * ((isSprinting) ? 0.15f : 0.1f)) - m_camPosTracer) * 0.4f; //Bobbing effects.
                        else
                            m_camPosTracer += (m_camOrigin - m_camPosTracer) * 0.4f; //The camera is lerped to its default position if viewbobbing is disabled.
                                                                                     ///


                        /// This section is dedicated to step sounds, which play on the lowest part of the camera's viewbob curve. Disabling viewbob doesn't affect the m_walkTime value!
                        if (enableSounds && !overrideFootsteps)
                        {
                            if (Mathf.Sin(m_walkTime) < -0.8f)
                            {
                                if (!m_stepped)
                                {
                                    //jpost Audio
                                    switch (footstepManager.currentFootstepType)
                                    {
                                        case "Grass":
                                            //jpost Audio testing out FMOD footsteps                                    
                                            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/sx_wgh_game_plr_footstep_grass", GetComponent<Transform>().position);
                                            break;
                                        case "Dirt":
                                            //jpost Audio testing out FMOD footsteps                                    
                                            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/sx_wgh_game_plr_footstep_dirt", GetComponent<Transform>().position);
                                            break;
                                        default:
                                            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/sx_wgh_game_plr_footstep_dirt", GetComponent<Transform>().position);
                                            break;
                                    }

                                    //AudioManager.PlaySound(walkSounds[Random.Range(0, walkSounds.Length)], soundVolume);
                                    m_stepped = true;
                                }
                            }
                            else
                            {
                                m_stepped = false;
                            }
                        }
                        ///
                    }
                    else
                    {
                        m_camPosTracer += (m_camOrigin - m_camPosTracer) * 0.4f; //If the player isn't moving fast enough to be constituted as "walking", the camera returns to normal.
                    }
                }
                else
                {
                    m_camPosTracer += (m_camOrigin - m_camPosTracer) * 0.4f; //If the player isn't on the ground, the camera goes back to its default position.
                }
            }

            ///

            /// Various outputs are assigned. The individual comments explain this in detail.

            /*
             * Third person outputs aren't assigned here because the third person camera has to be moved separate to the player controller.
             * This doesn't sound like a big deal, but if the motion isn't done in FixedUpdate, moving objects look EXTREMELY choppy.
             * If you want to see the third-person outputs, check FixedUpdate.
            */

            switch (playerMode)
            {
                case (GlideMode.firstPerson):

                    playerCamera.transform.localPosition += (m_camPosTracer - playerCamera.transform.localPosition) * 0.1f; //The position of the camera is shifted as needed.

                    m_goalAngles += new Vector3(-playerControlActions.LookY * mouseSensitivity, playerControlActions.LookX * mouseSensitivity); //The current motion of the mouse is taken in, multiplied by the mouse sensitivity, and then added onto the goal camera angles.

                    m_goalAngles.x = Mathf.Clamp(m_goalAngles.x, verticalRestraint.x, verticalRestraint.y); //The camera angles are restricted here, so the player can't flip their head completely down and snap their neck.

                    gameObject.transform.rotation = Quaternion.Euler(0f, m_goalAngles.y, 0f); //The horizontal rotation is applied to the player's body.
                    playerCamera.transform.rotation = Quaternion.Euler(m_goalAngles); //The vertical rotation is applied to the player's head.

                    weaponParent.rotation = playerCamera.transform.rotation; //sets the gun to follow the rotation of the camera

                    break;

            }
            ///

        }


        /// Zoom Feature. Fairly straightforward, if the zoomButton isn't assigned to the none slot and we're pressing it, we zoom in. Otherwise we zoom out. Controlled with a lil' recursive linear interpolation formula.

        m_zoomGoal = 0f; //This is reset per-frame.

        if (!lockCamera) //We first determine if we're zooming with the zoom button.
        {
            if (playerControlActions.Zoom.IsPressed)
            {
                m_zoomGoal += -zoomIntensity;
            }
        }

        if (isSprinting)
            m_zoomGoal += sprintIntensity;

        m_zoomAdditive += (m_zoomGoal - m_zoomAdditive) * 0.2f;

        ///


        //ANIMATIONS

        /// Just assigning all of the parameters here if they've been marked for use. If you need a better explanation, mouse over the parameters in the inspector.

        if (playerAnimator != null)
        {
            if (walkingParameter != string.Empty)
                playerAnimator.SetBool(walkingParameter, playerControlActions.MoveY || playerControlActions.MoveX);
            if (sprintingParameter != string.Empty)
                playerAnimator.SetBool(sprintingParameter, isSprinting);
            if (crouchingParameter != string.Empty)
                playerAnimator.SetBool(crouchingParameter, isCrouching);
            if (groundedParamter != string.Empty)
                playerAnimator.SetBool(groundedParamter, isGrounded);
            if (relativeSpeedParameter != string.Empty && moveSpeed != 0f)
                playerAnimator.SetFloat(relativeSpeedParameter, m_topSpeed / moveSpeed);
        }


        /// WEAPON BOB
        /// 
        if (!WeaponManager.current.isAiming)
        {
            if (playerControlActions.MoveY == 0 && playerControlActions.MoveX == 0)
            {
                WeaponBob(idleCounter, 0.025f, 0.025f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);

            }
            else if (!isSprinting)
            {
                WeaponBob(movementCounter, 0.035f, 0.035f);
                movementCounter += Time.deltaTime * 3f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);

            }
            else if (isSprinting)
            {

                WeaponBob(sprintCounter, 0.05f, 0.05f);
                sprintCounter += Time.deltaTime * 6f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 9f);
            }
        }
        else
        {
            weaponParent.localPosition = weaponParentBase;

        }

        m_lastGrav = m_grav; //The gravity from the last frame is set here. This is mostly used to compare against prior frame motion.

    }

    void FixedUpdate()
    {

        if (m_sliding)
        {
            m_canJump = false;
            Vector3 m_pos = m_slidingNormal;

            m_slidingNormal = m_slidingNormal.normalized;

            movement.x += (1f - m_slidingNormal.y) * m_slidingNormal.x * 0.5f;
            movement.z += (1f - m_slidingNormal.y) * m_slidingNormal.z * 0.5f;
        }

        m_rig.velocity = movement; //Movement is applied here. Motion is scaled on framerate to smooth things out.

        ///Third-person outputs are assigned here!

        switch (playerMode)
        {
            case (GlideMode.thirdPerson):
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z), 0f), 0.2f * (new Vector3(movement.x, 0f, movement.z).magnitude / moveSpeed));

                m_goalAngles += new Vector3(-playerControlActions.LookY * mouseSensitivity, playerControlActions.LookX * mouseSensitivity); //The current motion of the mouse is taken in, multiplied by the mouse sensitivity, and then added onto the goal camera angles.

                Vector3 m_camPos = Vector3.zero;

                Vector3 m_projection = new Vector3(Mathf.Cos((-m_goalAngles.y - 90f) * Mathf.Deg2Rad) * Mathf.Cos(m_goalAngles.z * Mathf.Deg2Rad), Mathf.Sin(m_goalAngles.x * Mathf.Deg2Rad), Mathf.Cos(m_goalAngles.x * Mathf.Deg2Rad) * Mathf.Sin((-m_goalAngles.y - 90f) * Mathf.Deg2Rad));
                m_goalAngles.x = Mathf.Clamp(m_goalAngles.x, -70f, 70f); //The camera angles are restricted here, so the player can't flip their head completely down and snap their neck.

                m_camPos = transform.position + (m_projection * thirdPersonOrbitDistance) + movement * Time.deltaTime;

                if (Physics.SphereCast(transform.position, 0.5f, (m_camPos - transform.position).normalized, out m_hit, thirdPersonOrbitDistance, ~0, QueryTriggerInteraction.Ignore))
                {

                    if (rigidbodyOcclusion)
                    {
                        m_camPos = m_hit.point + m_hit.normal * 0.1f;
                    }
                    else
                    {
                        if (m_hit.collider.GetComponent<Rigidbody>() != null)
                        {
                            if (m_hit.collider.GetComponent<Rigidbody>().isKinematic)
                                m_camPos = m_hit.point + m_hit.normal * 0.1f;
                        }
                        else
                            m_camPos = m_hit.point + m_hit.normal * 0.1f;
                    }

                }

                playerCamera.transform.position += (m_camPos - playerCamera.transform.position) * 0.2f;
                //playerCamera.transform.position = m_camPos;

                playerCamera.transform.rotation = Quaternion.LookRotation((transform.position - playerCamera.transform.position).normalized);
                break;
        }

        ///

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        /// This entire section focuses on preventing the player from climbing or jumping up slopes. While sliding can be disabled, for the sake of gameplay, you cannot change settings that prevent the player from jumping up hills. If it's too steep, it ain't happening.
        if (slidingOnSlopes)
        {
            if (m_grav < 0f)
            {
                if (hit.collider.GetComponent<Rigidbody>() != null)
                    if (!hit.collider.GetComponent<Rigidbody>().isKinematic)
                        return;

                if (Vector3.Dot(hit.normal, Vector3.up) > 0f)
                {
                    if ((1f - hit.normal.y) > slopeBias) //If sliding on slopes is enabled, we check to see if the current surface is too steep. If so, the player can no longer jump, and they are shunted down the slope. If the slope isn't too steep, the player is then allowed to jump.
                    {
                        m_sliding = true;
                        m_slidingNormal = hit.normal;
                    }
                    else
                    {
                        m_sliding = false;
                        m_slidingNormal = Vector3.zero;
                        m_canJump = true;
                    }
                }
            }
        }
        ///
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.collider.GetComponent<Rigidbody>() != null)
            if (!collision.collider.GetComponent<Rigidbody>().isKinematic)
                return;

        /// This entire section focuses on preventing the player from climbing or jumping up slopes. While sliding can be disabled, for the sake of gameplay, you cannot change settings that prevent the player from jumping up hills. If it's too steep, it ain't happening.
        Vector3 normal = Vector3.zero;

        /*
        foreach (ContactPoint m_pnt in collision.contacts)
            normal += m_pnt.normal;
        */

        if (Physics.Raycast(transform.position, Vector3.down, out m_hit))
            normal = m_hit.normal;

        normal = normal.normalized;

        if (Vector3.Dot(normal, Vector3.up) > 0.1f) //We only need to make the following decisions if the object we're colliding with is beneath us.
        {
            if (slidingOnSlopes)
            {
                if (m_grav < 0f)
                {
                    if ((1f - normal.y) > slopeBias) //If sliding on slopes is enabled, we check to see if the current surface is too steep. If so, the player can no longer jump, and they are shunted down the slope. If the slope isn't too steep, the player is then allowed to jump.
                    {
                        m_sliding = true;
                        m_slidingNormal = normal;
                    }
                    else
                    {
                        m_sliding = false;
                        m_slidingNormal = Vector3.zero;
                        m_canJump = true;
                    }
                }
            }
            else
                m_canJump = true;
        }
        ///

    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;

        switch (playerMode)
        {
            case (GlideMode.thirdPerson):
                Vector3 m_camPos = Vector3.zero;

                Vector3 m_projection = new Vector3(Mathf.Cos((-m_goalAngles.y - 90f) * Mathf.Deg2Rad) * Mathf.Cos(m_goalAngles.z * Mathf.Deg2Rad), Mathf.Sin(m_goalAngles.x * Mathf.Deg2Rad), Mathf.Cos(m_goalAngles.x * Mathf.Deg2Rad) * Mathf.Sin((-m_goalAngles.y - 90f) * Mathf.Deg2Rad));
                m_goalAngles.x = Mathf.Clamp(m_goalAngles.x, -70f, 70f); //The camera angles are restricted here, so the player can't flip their head completely down and snap their neck.

                m_camPos = transform.position + (m_projection * thirdPersonOrbitDistance) + movement * Time.deltaTime;


                if (Physics.SphereCast(transform.position, 0.5f, (m_camPos - transform.position).normalized, out m_hit, thirdPersonOrbitDistance, ~0, QueryTriggerInteraction.Ignore))
                {

                    if (rigidbodyOcclusion)
                    {
                        m_camPos = m_hit.point + m_hit.normal * 0.1f;
                    }
                    else
                    {
                        if (m_hit.collider.GetComponent<Rigidbody>() != null)
                        {
                            if (m_hit.collider.GetComponent<Rigidbody>().isKinematic)
                                m_camPos = m_hit.point + m_hit.normal * 0.1f;
                        }
                        else
                            m_camPos = m_hit.point + m_hit.normal * 0.1f;
                    }

                }

                playerCamera.transform.position = m_camPos;

                break;
        }
    }

    void WeaponBob(float parameterZ, float xIntensity, float yIntensity)
    {

        targetWeaponBobPosition = weaponParentBase + new Vector3(Mathf.Cos(parameterZ) * xIntensity, Mathf.Sin(parameterZ * 2) * yIntensity, 0);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(playerRadius * 2f, playerHeight, playerRadius * 2f));
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * playerHeight * 0.66f);
    }

    public enum GlideMouseSetting
    { //Enumerator for mouse buttons.

        leftMouse = 0,
        rightMouse = 1,
        middleMouse = 2,
        extraMouse1 = 3,
        extraMouse2 = 4,
        none = 5

    }

    public enum GlideSprintSetting
    { //Changes how sprint behaves.

        none = 0, //Self explanatory.
        normal = 1, //Hold sprint to sprint. When released, you will return to normal speed.
        classic = 2 //While you're running, pressing the sprint key will engage sprinting. Sprinting will only stop when you do.

    }

    public enum GlideCrouchSetting
    { //Changes how crouching behaves.
        none = 0,
        normal = 1, //Crouching doesn't affect whether or not the player can sprint. Get a move on!
        noSprint = 2 //Crouching disables the ability to sprint. Good if you're practical or whatever.
    }

    public enum GlideMode
    { //Determines which player type we're working with. Important!
        firstPerson = 0,
        thirdPerson = 1
    }

    public enum GlideMovement
    { //This determines how our motion is calculated. Waltz utilizes the Character Controller component, but for versatility we're adding more control.
        characterController = 0,
        rigidbody = 1
    }

    public enum GlideJump
    { //Determines how jumping is calculated.
        none = 0,
        normal = 1,
        enhanced = 2,
        leaping = 3
    }

    private void OnDestroy()
    {
        // Destroy the player action set
        playerControlActions.Destroy();
    }
}
