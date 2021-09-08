/*****************************************************************************
// File Name :         PlayerMovement.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    #region Shoot Forces
    [Header("Shoot forces")]
    [SerializeField]
    [Tooltip("How much force cannons shoot out objects")]
    [Range(0, 5000)]
    private float shootForce = 1000;

    [SerializeField]
    [Tooltip("How much force pushes the cannon back when it shoots")]
    [Range(0, 1000)]
    private float shootPushBackForce = 100;

    [SerializeField]
    [Tooltip("How much angular momentum is added to the cannon when it is shot")]
    [Range(0, 4000)]
    private float shootAngularVelocity = 800;
    #endregion

    #region Bounce Variables
    [Header("Bounce Variables")]
    [SerializeField]
    [Tooltip("The max amount of angular velocity on the cannon after bouncing")]
    [Range(200, 1000)]
    private float maxAngularVelocity = 800;

    [SerializeField]
    [Tooltip("The min amount of angular velocity on the cannon after bouncing")]
    [Range(0, 600)]
    private float minAngularVelocity = 400;

    [SerializeField]
    [Tooltip("How much the cannon bounces up after hitting the ground")]
    private float groundBounceUpVelocity = 2;

    [SerializeField]
    [Tooltip("Min amount of bounce off of the walls and cieling")]
    [Range(0, 10)]
    private float minBounceVelocity = 2;

    [SerializeField]
    [Tooltip("Max amount of bounce off of the walls and cieling")]
    [Range(5, 40)]
    private float maxBounceVelocity = 15;
    #endregion

    #region Limits
    [Header("Limits")]
    [SerializeField]
    [Tooltip("The max velocity of cannons")]
    [Range(0, 30)]
    private float maxSpeed = 10;

    [SerializeField]
    [Tooltip("The number of cannon that can be fired out of this cannon")]
    [Range(0, 10)]
    private int numCannons = 5;

    /// <summary>
    /// Holds reference to the starting amount of cannons on this level.
    /// </summary>
    private static int levelNumCannons = 0;

    [SerializeField]
    [Tooltip("Time the player waits after shooting their last cannon")]
    [Range(0, 10)]
    private float waitAfterNoCannons = 5;

    [Tooltip("How small the player can get")]
    [SerializeField]
    private float minSizeScale = 0.25f;

    /// <summary>
    /// The amount of size change on each shot.
    /// </summary>
    private static float sizeChangeAmount = 0;

    /// <summary>
    /// Allows other scripts to modify the size change amount.
    /// </summary>
    public static float SizeChangeAmount
    {
        set
        {
            sizeChangeAmount = value;
        }
    }

    /// <summary>
    /// Getter and setter for the temp number of cannons.
    /// </summary>
    [HideInInspector]
    public int NumCannons
    {
        get => numCannons;

        set
        {
            numCannons = value;
        }
    }

    /// <summary>
    /// Allows other scripts to modify the number of cannons for this level.
    /// </summary>
    public static int LevelNumCannons
    {
        set
        {
            levelNumCannons = value;
        }
    }
    #endregion

    #region Shoot Positions
    [Header("Shoot positions")]
    [SerializeField]
    [Tooltip("The position the cannon is shot from")]
    private GameObject shootPos;

    [SerializeField]
    [Tooltip("The position the cannon pivots from")]
    private GameObject pivotPos;
    #endregion

    #region Extras
    [Header("Extras")]
    [SerializeField]
    [Tooltip("The cannon part of the heirarchy")]
    private GameObject cannon;

    [SerializeField]
    [Tooltip("The Visuals of the cannon")]
    private GameObject visuals;

    [SerializeField]
    [Tooltip("The visuals for the amount of cannons")]
    private Sprite[] cannonAmountImages;

    [Tooltip("The renderering for the amount of cannons")]
    public Image cannonAmountImage;

    [SerializeField]
    private float groundDist = 3;

    [SerializeField]
    [Tooltip("The physics material with friction")]
    private PhysicsMaterial2D frictionMat;

    [SerializeField]
    private PolygonCollider2D cannonBarrelCollider;

    /// <summary>
    /// The layer mask of the environment.
    /// </summary>
    private LayerMask environmentMask;

    /// <summary>
    /// The start position of the cannon.
    /// </summary>
    private static Vector2 startPos = Vector2.zero;

    /// <summary>
    /// The starting scale of the cannon.
    /// </summary>
    public static float startScale = 0;

    /// <summary>
    /// A list of the active cannons in the scene.
    /// </summary>
    [HideInInspector]
    public List<GameObject> activeCannons = new List<GameObject>();
    #endregion

    #region Components
    /// <summary>
    /// Current Cinemachine being used.
    /// </summary>
    [HideInInspector]
    public CinemachineVirtualCamera currentVCam;

    /// <summary>
    /// The Rigidbody2D component of this cannon.
    /// </summary>
    [HideInInspector]
    public Rigidbody2D rb2d;

    /// <summary>
    /// Reference to the main camera in this scene.
    /// </summary>
    private Camera mainCam;

    /// <summary>
    /// The audiosource of the cannon.
    /// </summary>
    private AudioSource aud;

    /// <summary>
    /// The TextMeshPro that represents the cannons numCannons.
    /// </summary>
    private TextMeshPro tmp;
    #endregion

    #region Bools
    /// <summary>
    /// Holds true if the player is allowed to aim the cannon.
    /// </summary>
    private static bool canAim = true;

    /// <summary>
    /// Allows other scripts to modify the players ability to aim.
    /// </summary>
    public static bool CanAim
    {
        get => canAim;

        set
        {
            canAim = value;
        }
    }

    /// <summary>
    /// Allows or disables the cannons ability to shoot.
    /// </summary>
    private bool canShoot = true;

    /// <summary>
    /// Allows other scripts to disable the cannons ability to shoot.
    /// </summary>
    public bool CanShoot
    {
        get => canShoot;

        set
        {
            canShoot = value;
        }

    }

    /// <summary>
    /// Allows the player to reset and aim again from their current spot.
    /// </summary>
    private bool canReAim = false;

    /// <summary>
    /// Holds true if this is the currently active cannon.
    /// </summary>
    private bool isActive = true;
    #endregion
    #endregion

    #region Functions
    #region Initialize
    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        #region Cannon Components
        currentVCam = transform.parent.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
        tmp = gameObject.GetComponentInChildren<TextMeshPro>();
        #endregion

        #region Cannon Variables
        environmentMask = LayerMask.GetMask("Environment");
        activeCannons.Add(gameObject);
        UpdateCannonAmountUI();
        FindSizeChanging();
        FindStartPos();
        #endregion
    }

    /// <summary>
    /// Finds the original values of the cannon.
    /// </summary>
    private void FindStartPos()
    {
        if(startScale == 0)
        {
            startPos = transform.parent.position;
            startScale = transform.parent.localScale.x;
            levelNumCannons = numCannons;
        }
    }

    /// <summary>
    /// Updates the cannons UI.
    /// </summary>
    public void UpdateCannonAmountUI()
    {
        if(true)
        {
            tmp.text = numCannons.ToString();
        }
    }

    /// <summary>
    /// Finds at what rate the cannon should be changed in scale.
    /// </summary>
    private void FindSizeChanging()
    {
        if (sizeChangeAmount == 0 && numCannons != 0)
        {
            sizeChangeAmount = transform.localScale.x - minSizeScale;
            sizeChangeAmount /= numCannons;
        }
    }
    #endregion

    #region Speed Limit
    /// <summary>
    /// Resets the speed and angular velocity.
    /// </summary>
    private void FixedUpdate()
    {
        //LimitSpeed();
    }

    /// <summary>
    /// Limits how fast the player can move.
    /// </summary>
    private void LimitSpeed()
    {
        if (rb2d.velocity.magnitude > maxSpeed)
        {
            Vector2 newVel = rb2d.velocity.normalized * maxSpeed;
            rb2d.velocity = newVel;
        }
    }
    #endregion

    #region Aim and Shoot
    /// <summary>
    /// Checks if the player wants to shoot and allows them to aim.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootPlayer();
        }

        LookAtCursor(Input.mousePosition);
    }

    /// <summary>
    /// Calculates the angle the cannon should be.
    /// </summary>
    private void LookAtCursor(Vector2 mousePos)
    {
        if (canAim && canShoot && Time.deltaTime != 0)
        {
            Vector3 aimDir = (mainCam.ScreenToWorldPoint(mousePos) - cannon.transform.position).normalized;
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            cannon.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    /// <summary>
    /// If the shoot key is pressed then a new character is shot out.
    /// </summary>
    public void ShootPlayer()
    {
        if (canShoot && numCannons > 0 && Time.deltaTime != 0)
        {
            // Disables cannons ability to shoot, aim, and sets it to be not active
            canShoot = false;
            canAim = false;
            isActive = false;

            ApplyForces();

            aud.Play();

            // Disables components
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    /// <summary>
    /// Spawns a new cannon and shoots it forward with its new values.
    /// </summary>
    private void ApplyForces()
    {
        //Spawns cannon
        GameObject tempCannon = Instantiate(Resources.Load("Prefabs/Cannon and Camera", typeof(GameObject)), (Vector2)shootPos.transform.position, Quaternion.identity) as GameObject;
        tempCannon.transform.localScale = transform.parent.transform.localScale - new Vector3(sizeChangeAmount, sizeChangeAmount, sizeChangeAmount);

        // Gets direction cannon should be shot
        Vector2 shootDir = shootPos.transform.position - pivotPos.transform.position;
        shootDir.Normalize();

        // Adds forces to both cannons and changes the current camera
        rb2d.AddForce(-shootDir * shootPushBackForce);
        PlayerMovement tempPM = tempCannon.GetComponentInChildren<PlayerMovement>();
        tempPM.rb2d.AddForce(shootDir * shootForce);
        tempPM.rb2d.angularVelocity = shootAngularVelocity;

        // Sets the values the prefab
        tempPM.currentVCam.Priority = currentVCam.Priority + 1;
        tempPM.NumCannons = numCannons - 1;
        tempPM.cannonAmountImage = cannonAmountImage;
        tempPM.UpdateCannonAmountUI();
        tempPM.activeCannons.AddRange(activeCannons);

        // Restarts the level if this is the last cannon
        if (numCannons - 1 == 0)
        {
            StartCoroutine(tempPM.RestartLevel());
        }
    }

    /// <summary>
    /// If d is pressed while on the ground then the player can aim again.
    /// </summary>
    public void OnDebugTask()
    {
        if (canReAim)
        {
            ResetPlayerAim();
        }
    }

    /// <summary>
    /// Allows the player to aim again and reset their rotation.
    /// </summary>
    private void ResetPlayerAim()
    {
        if (canReAim)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            canAim = true;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            LookAtCursor(Mouse.current.position.ReadValue());

            canReAim = false;
        }
    }
    #endregion

    #region Restart Level
    /// <summary>
    /// Calls for the level to be restarted sometime after the last cannon is shot.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartLevel()
    {
        yield return new WaitForFixedUpdate();

        while(rb2d.velocity.magnitude > 0.0000001f)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(waitAfterNoCannons);

        if (!FlagBehavior.HasWon)
        {
            Restart();
        }
    }

    /// <summary>
    /// Restarts the level.
    /// </summary>
    public void Restart()
    {
        // Spawns the cannon and sets its values.
        GameObject tempCannon = Instantiate(Resources.Load("Prefabs/Cannon and Camera", typeof(GameObject)), startPos, Quaternion.identity) as GameObject;
        PlayerMovement tempPM = tempCannon.GetComponentInChildren<PlayerMovement>();
        tempPM.NumCannons = levelNumCannons;
        tempPM.UpdateCannonAmountUI();
        tempCannon.transform.localScale = new Vector3(startScale, startScale, startScale);
        canAim = true;

        // Destroys old cannons
        activeCannons.Remove(gameObject);
        foreach (GameObject cannon in activeCannons)
        {
            Destroy(cannon.transform.parent.gameObject);
        }

        // Blends back to the original cannon
        currentVCam.m_Priority = 0;

        // Destroys the cannon and then its camera
        Destroy(transform.parent.gameObject, 0.5f);
        Destroy(gameObject);
    }
    #endregion

    #region Collision Events
    /// <summary>
    /// Checks if the player collide with a wall or the floor.
    /// </summary>
    /// <param name="other">The object collided with.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CheckBelow())
        {
            GroundCollision();
        }
        else
        {
            WallsCollision();
        }

        if (isActive)
        {
            IPlayerInteractable hazardCollision = other.gameObject.GetComponent<IPlayerInteractable>();

            if (hazardCollision != null)
            {
                hazardCollision.CollisionEvent(gameObject);
            }
        }
    }

    /// <summary>
    /// Checks if the ground is below.
    /// </summary>
    /// <returns></returns>
    private bool CheckBelow()
    {
        RaycastHit2D hit = Physics2D.Linecast(visuals.transform.position, (Vector2)visuals.transform.position + Vector2.down * groundDist, environmentMask);

        return hit.transform != null;
    }

    /// <summary>
    /// Handles the event of colliding with the ground.
    /// </summary>
    private void GroundCollision()
    {
        if (rb2d.sharedMaterial != frictionMat)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 2);
        }

        rb2d.sharedMaterial = frictionMat;
        cannonBarrelCollider.sharedMaterial = frictionMat;
    }

    /// <summary>
    /// Handles the event of colliding with a wall.
    /// </summary>
    private void WallsCollision()
    {
        //FindBounceVelocity();
        FindBounceAngularVelocity();
    }

    /// <summary>
    /// Sets the velocity of the player bouncing off of an object.
    /// </summary>
    private void FindBounceVelocity()
    {
        int signX = (int)Mathf.Sign(rb2d.velocity.x);
        int signY = (int)Mathf.Sign(rb2d.velocity.y);

        float xVel = 0;
        float yVel = 0;

        if(signX > 0)
        {
            xVel = Mathf.Clamp(rb2d.velocity.x, minBounceVelocity, maxBounceVelocity);
        }
        else
        {
            xVel = Mathf.Clamp(rb2d.velocity.x, -maxBounceVelocity, -minBounceVelocity);
        }

        if(signY > 0)
        {
            xVel = Mathf.Clamp(rb2d.velocity.x, minBounceVelocity, maxBounceVelocity);
        }
        else
        {
            yVel = Mathf.Clamp(rb2d.velocity.y, -maxBounceVelocity, -minBounceVelocity);
        }

        rb2d.velocity = new Vector2(xVel, yVel);
    }

    /// <summary>
    /// Sets the angular velocity of the player bouncing off of a wall.
    /// </summary>
    private void FindBounceAngularVelocity()
    {
        //float spin = rb2d.angularVelocity;

        int sign = (int)Mathf.Sign(rb2d.angularVelocity);

        if(sign > 0)
        {
            rb2d.angularVelocity = Mathf.Clamp(rb2d.angularVelocity, minAngularVelocity, maxAngularVelocity);
        }
        else
        {
            rb2d.angularVelocity = Mathf.Clamp(rb2d.angularVelocity, -maxAngularVelocity, -minAngularVelocity);
        }

        /*
        if(spin > maxAngularVelocity)
        {
            rb2d.angularVelocity = maxAngularVelocity;
        }
        else if(spin < -maxAngularVelocity)
        {
            rb2d.angularVelocity = -maxAngularVelocity;
        }
        else if(spin < minAngularVelocity && spin > -minAngularVelocity)
        {
            rb2d.angularVelocity = minAngularVelocity;
        }*/
    }

    /// <summary>
    /// Allows the player to reaim if they are on the ground.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!canReAim)
        {
            if (canShoot && CheckBelow())
            {
                canReAim = true;
            }
            else
            {
                canReAim = false;
            }
        }
    }


    /// <summary>
    /// Disables the player ability to aim if they leave the ground.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        canReAim = false;
    }

    /// <summary>
    /// Calls for collision events when colliding with an interactable.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive)
        {
            IPlayerInteractable hazardCollision = other.gameObject.GetComponent<IPlayerInteractable>();

            if(hazardCollision != null)
            {
                hazardCollision.CollisionEvent(gameObject);
            }
        }
    }
    #endregion

    #endregion
}
