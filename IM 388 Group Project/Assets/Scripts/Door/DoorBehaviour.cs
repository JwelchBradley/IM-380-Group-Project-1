/*****************************************************************************
// File Name :         DoorBehavior.cs
// Author :            Jacob T. Welch
// Creation Date :     March 25, 2021
//
// Brief Description : This Script hnadles the doors movement.
*****************************************************************************/
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Holds true if the door should open.
    /// </summary>
    [HideInInspector]
    public bool shouldOpen = false;

    [Header("Door Stats")]
    [SerializeField]
    [Tooltip("Holds true if the door should move horizontally")]
    private bool isHorizontal = false;

    [SerializeField]
    [Tooltip("How fast the door opens and closes")]
    private float moveSpeed = 2;

    [SerializeField]
    [Range(1, 10)]
    [Tooltip("Distance beneath the door that it checks for other objects")]
    private float beneathDist;

    [SerializeField]
    [Tooltip("The layers that the door checks beneath itself for")]
    private LayerMask mask;

    /*
    [Header("Sound")]
    [SerializeField]
    [Tooltip("The sound a door makes when opening")]
    private AudioClip doorOpenSound;

    [SerializeField]
    [Range(0, 2)]
    [Tooltip("The volume of doorOpenSound")]
    private float doorOpenSoundVolume;

    [SerializeField]
    [Tooltip("The sound a door makes when closing")]
    private AudioClip doorCloseSound;

    [SerializeField]
    [Range(0, 2)]
    [Tooltip("The volume of doorOpenSound")]
    private float doorCloseSoundVolume;
    */
    /// <summary>
    /// Holds true if the door is open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Boxcollider of this door.
    /// </summary>
    BoxCollider2D bc2d;

    /// <summary>
    /// The audiosource of this door.
    /// </summary>
    private new AudioSource audio;

    /// <summary>
    /// Starting position of the door.
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// Position the door is when closed.
    /// </summary>
    private Vector2 closePos;

    /// <summary>
    /// The order this object is in.
    /// </summary>
    private float zOrder = 1;

    /// <summary>
    /// How far into the audio the door currently is.
    /// </summary>
    private float timeInAudio = 0;

    /// <summary>
    /// Holds true if an object was beneath it.
    /// </summary>
    private bool hasStopped = false;
    #endregion

    #region Functions
    /// <summary>
    /// Initializes components and variables.
    /// </summary>
    private void Awake()
    {
        // Initializes components
        bc2d = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();

        // Initializes move positions
        FindStartClose();
    }

    /// <summary>
    /// Finds the start and close pos for the door.
    /// </summary>
    private void FindStartClose()
    {
        startPos = transform.position;

        if (isHorizontal)
        {
            closePos += (Vector2)transform.position + transform.up * new Vector2(bc2d.bounds.size.x, 0);
        }
        else
        {
            closePos += (Vector2)transform.position + transform.up * new Vector2(0, bc2d.bounds.size.y);
        }
    }

    /// <summary>
    /// Handles the opening and closing of a door.
    /// </summary>
    void FixedUpdate()
    {
        if (shouldOpen)
        {
            OpenDoor();
        }
        else if (transform.position != startPos)
        {
            CloseDoor();
        }
    }

    /// <summary>
    /// Opens door.
    /// </summary>
    private void OpenDoor()
    {
        MoveDoor(closePos);

        if (!isOpen)
        {
            PlayDoorAudio(true, timeInAudio);
        }
    }

    /// <summary>
    /// Closes door.
    /// </summary>
    private void CloseDoor()
    {
        if (!IsObjectBeneath())
        {
            if (hasStopped)
            {
                audio.time = timeInAudio;
                audio.Play();
            }

            MoveDoor(startPos);
        }
        else
        {
            audio.Stop();
            hasStopped = true;
        }

        if (isOpen)
        {
            PlayDoorAudio(false, 2.681f - timeInAudio);
        }
    }

    private void PlayDoorAudio(bool openState, float time)
    {
        isOpen = openState;
        audio.Stop();
        audio.time = time;
        audio.Play();
    }

    /// <summary>
    /// The door is moved to newPos.
    /// </summary>
    /// <param name="newPos">The position the door is moved to.</param>
    private void MoveDoor(Vector2 newPos)
    {
        Vector3 pos = Vector2.MoveTowards(transform.position,
                                          newPos,
                                          moveSpeed * Time.fixedDeltaTime);

        if (isOpen)
        {
            timeInAudio += Time.deltaTime;
        }
        else
        {
            timeInAudio -= Time.deltaTime;
        }

        if (timeInAudio > 2.682)
        {
            timeInAudio = 2.681f;
        }
        else if (timeInAudio < 0)
        {
            timeInAudio = 0;
        }

        pos.z = zOrder;
        transform.position = pos;
    }

    /// <summary>
    /// Checks if an object below it should stop the door movement
    /// </summary>
    /// <returns>True if there is an object beneath the door.</returns>
    private bool IsObjectBeneath()
    {
        Vector2 bottomOfDoor = bc2d.bounds.center;

        RaycastHit2D hit = Physics2D.BoxCast(bottomOfDoor,
                                             bc2d.bounds.size,
                                             0,
                                             -transform.up,
                                             beneathDist,
                                             mask);

        return hit.transform != null;
    }
    #endregion
}