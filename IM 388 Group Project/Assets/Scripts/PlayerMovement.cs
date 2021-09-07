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

    #region Limits
    [Header("Limits")]
    [SerializeField]
    [Tooltip("The max velocity of cannons")]
    [Range(0, 30)]
    private float maxSpeed = 10;

    [SerializeField]
    private float maxAngularVelocity = 800;

    [SerializeField]
    private float minAngularVelocity = 400;

    [SerializeField]
    [Tooltip("The number of cannon that can be fired out of this cannon")]
    [Range(0, 10)]
    private int numCannons = 5;

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
    #endregion

    #region Bools
    /// <summary>
    /// Holds true if the player is allowed to aim the cannon.
    /// </summary>
    private static bool canAim = true;

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
        currentVCam = transform.parent.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
        environmentMask = LayerMask.GetMask("Environment");
        LookAtCursor(Mouse.current.position.ReadValue());

        UpdateCannonAmountUI();

        FindSizeChanging();
    }

    public void UpdateCannonAmountUI()
    {
        if (cannonAmountImage != null)
        {
            cannonAmountImage.sprite = cannonAmountImages[numCannons];
        }
    }

    private void FindSizeChanging()
    {
        if (sizeChangeAmount == 0 && numCannons != 0)
        {
            sizeChangeAmount = transform.localScale.x - minSizeScale;
            sizeChangeAmount /= numCannons;
        }
    }
    #endregion

    /// <summary>
    /// Resets the speed and angular velocity.
    /// </summary>
    private void FixedUpdate()
    {
        LimitSpeed();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootPlayer();
        }

        LookAtCursor(Input.mousePosition);
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

            if (numCannons - 1 == 0)
            {
                StartCoroutine(RestartLevel());
            }

            // Disables components
            GetComponent<PlayerInput>().enabled = false;
        }
    }

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
    }

    public void OnDebugTask()
    {
        if (canReAim)
        {
            ResetPlayerAim();
        }
    }

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

    /// <summary>
    /// Calls for the level to be restarted sometime after the last cannon is shot.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartLevel()
    {
        yield return new WaitForFixedUpdate();

        while(rb2d.velocity.magnitude > 0.00000001f)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(waitAfterNoCannons);

        if (!FlagBehavior.HasWon)
        {
            GameObject.Find("Pause Menu Templates Canvas").GetComponent<PauseMenuBehavior>().RestartLevel();
        }
    }

    #region Collision Events
    private void OnCollisionEnter2D(Collision2D other)
    {
        RaycastHit2D hit = Physics2D.Linecast(visuals.transform.position, (Vector2)visuals.transform.position + Vector2.down * groundDist, environmentMask);

        if (hit.transform != null)
        {
            if (rb2d.sharedMaterial != frictionMat)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 2);
            }

            rb2d.sharedMaterial = frictionMat;
            cannonBarrelCollider.sharedMaterial = frictionMat;
        }
        else
        {
            FindAngularVelocity();
        }
    }

    private void FindAngularVelocity()
    {
        float spin = rb2d.angularVelocity;

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
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!canReAim)
        {
            RaycastHit2D hit = Physics2D.Linecast(visuals.transform.position, (Vector2)visuals.transform.position + Vector2.down * groundDist, environmentMask);

            if (hit.transform != null && canShoot)
            {
                canReAim = true;
            }
            else
            {
                canReAim = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canReAim = false;
    }

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
