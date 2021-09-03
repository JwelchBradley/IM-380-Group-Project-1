/*****************************************************************************
// File Name :         PressurPlateBehvaiour.cs
// Author :            Jacob T. Welch
// Creation Date :     September 1, 2021
//
// Brief Description : Provides functionality for pressure plates. They can
                       open specific door when pressed down.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class PressurePlateBehaviour : MonoBehaviour, IPlayerInteractable
{
    [Header("Door")]
    [Tooltip("The door this pressure plate opens")]
    public GameObject door;

    [Header("Pressure Plate Attributes")]
    [Tooltip("The layermask th are in")]
    public LayerMask mask;

    [Tooltip("How fast this pressure plate gets pushed down")]
    public float plateMoveSpeed;

    [Tooltip("How fast down the pressure plate moves")]
    public float distForStartToLow = 0.2f;

    /// <summary>
    /// The lowest y position of the pressure plate.
    /// </summary>
    private Vector2 lowestYPos;

    /// <summary>
    /// The starting y position of the pressure plate.
    /// </summary>
    private Vector2 startingYPos;

    /// <summary>
    /// The DoorBehaviour of a door controlled by the pressure plate.
    /// </summary>
    private DoorBehaviour db;

    /// <summary>
    /// Holds true when something is on the pressure plate.
    /// </summary>
    private bool onPlate = false;

    /// <summary>
    /// Initializes starting fields.
    /// </summary>
    void Awake()
    {
        db = door.GetComponent<DoorBehaviour>();

        // Movement position of the pressure plate
        startingYPos = transform.position;
        lowestYPos = startingYPos - new Vector2(0, distForStartToLow);
    }

    private IEnumerator MovePlate(Vector2 newPos)
    {
        while ((Vector2)transform.position != lowestYPos)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position,
                                              newPos,
                                              plateMoveSpeed * Time.fixedDeltaTime);

            transform.position = pos;
            yield return new WaitForFixedUpdate();
        }
    }

    public void CollisionEvent(GameObject other)
    {
        StartCoroutine(MovePlate(lowestYPos));
        onPlate = true;

        db.shouldOpen = true;
    }
}