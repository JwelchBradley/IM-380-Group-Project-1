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
    [Tooltip("The number of cannon that can be fired out of this cannon")]
    [Range(0, 10)]
    private int numCannons = 5;

    [SerializeField]
    [Tooltip("Time the player waits after shooting their last cannon")]
    [Range(0, 10)]
    private float waitAfterNoCannons = 5;

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

    private bool hasWon = false;

    public bool HasWon
    {
        set
        {
            hasWon = value;
        }
    }
    #endregion
    #endregion

    #region Functions
    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        currentVCam = transform.parent.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
        LookAtCursor(Mouse.current.position.ReadValue());
    }

    /// <summary>
    /// Resets the speed and angular velocity.
    /// </summary>
    private void FixedUpdate()
    {
        LimitSpeed();
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
    /// Aims the cannon.
    /// </summary>
    /// <param name="input">The mouse position.</param>
    public void OnLook(InputValue input)
    {
        LookAtCursor(input.Get<Vector2>());            
    }

    /// <summary>
    /// Calculates the angle the cannon should be.
    /// </summary>
    private void LookAtCursor(Vector2 mousePos)
    {
        if (canAim && Time.deltaTime != 0)
        {
            Vector3 aimDir = (mainCam.ScreenToWorldPoint(mousePos) - cannon.transform.position).normalized;
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            cannon.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    /// <summary>
    /// If the shoot key is pressed then a new character is shot out.
    /// </summary>
    public void OnShootPlayer()
    {
        if (canShoot && numCannons > 0 && Time.deltaTime != 0)
        {
            // Disables cannons ability to shoot, aim, and sets it to be not active
            canShoot = false;
            canAim = false;
            isActive = false;

            //Spawns cannon
            GameObject tempCannon = Instantiate(Resources.Load("Prefabs/Cannon and Camera", typeof(GameObject)), (Vector2)shootPos.transform.position, Quaternion.identity) as GameObject;
            tempCannon.transform.localScale = transform.parent.transform.localScale / 1.2f;

            // Gets direction cannon should be shot
            Vector2 shootDir = shootPos.transform.position - pivotPos.transform.position;
            shootDir.Normalize();

            // Adds forces to both cannons and changes the current camera
            rb2d.AddForce(-shootDir * shootPushBackForce);
            PlayerMovement tempPM = tempCannon.GetComponentInChildren<PlayerMovement>();
            tempPM.rb2d.velocity = rb2d.velocity;
            tempPM.rb2d.AddForce(shootDir * shootForce);
            tempPM.rb2d.angularVelocity = shootAngularVelocity;

            // Sets the values the prefab
            tempPM.currentVCam.Priority = currentVCam.Priority + 1;
            tempPM.NumCannons = numCannons - 1;

            aud.Play();

            if(numCannons - 1 == 0)
            {
                StartCoroutine(RestartLevel());
            }

            // Disables components
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    /// <summary>
    /// Calls for the level to be restarted sometime after the last cannon is shot.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartLevel()
    {
        while(rb2d.velocity.magnitude > 1)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(waitAfterNoCannons);

        if (!hasWon)
        {
            GameObject.Find("Pause Menu Templates Canvas").GetComponent<PauseMenuBehavior>().RestartLevel();
        }
    }

    public void OnDebugTask()
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

    private void OnCollisionStay2D(Collision2D other)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, Vector2.down, environmentMask);
        if(hit != null)
        {
            canReAim = true;
        }
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
}
