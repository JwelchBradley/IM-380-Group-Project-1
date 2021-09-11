using System.Collections;
using UnityEngine;

public class UnicycleBehaviour : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField]
    [Tooltip("Will move through these position in forwards and then reverse order")]
    private Vector2[] movePos = null;

    [Header("Speed")]
    [SerializeField]
    [Tooltip("The time it takes to move from this ")]
    private float[] moveTime = null;

    /// <summary>
    /// Current position to move to.
    /// </summary>
    private int currentMovePos = 1;

    /// <summary>
    /// Current move time.
    /// </summary>
    private int currentMoveTime = 0;

    /// <summary>
    /// Holds true until it moves in reverse order.
    /// </summary>
    private bool forwardOrder = true;

    private SpriteRenderer sr;

    /// <summary>
    /// Starts the moving process.
    /// </summary>
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SawRoutine());
    }

    /// <summary>
    /// The routine for this saws movement.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SawRoutine()
    {
        while (true)
        {
            if (movePos.Length > 2)
            {
                CheckDirectionMultiple();
            }
            else
            {
                CheckDirection();
            }

            transform.position = Vector2.MoveTowards(transform.position, movePos[currentMovePos], moveTime[currentMoveTime] * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Changes saws direction between 2 points.
    /// </summary>
    private void CheckDirection()
    {
        if ((Vector2)transform.position == movePos[currentMovePos])
        {
            if (!forwardOrder)
            {
                sr.flipX = true;
                forwardOrder = true;
                currentMovePos = 0;
            }
            else
            {
                sr.flipX = false;
                forwardOrder = false;
                currentMovePos = 1;
            }
        }
    }

    /// <summary>
    /// Changes the saws direction and speed between 3 or more points.
    /// </summary>
    private void CheckDirectionMultiple()
    {
        if ((Vector2)transform.position == movePos[currentMovePos])
        {
            if (forwardOrder)
            {
                if (currentMovePos == movePos.Length - 1)
                {
                    currentMovePos--;
                    forwardOrder = false;
                }
                else
                {
                    currentMovePos++;
                    currentMoveTime++;
                }
            }
            else
            {
                if (currentMovePos == 0)
                {
                    currentMovePos++;
                    forwardOrder = true;
                }
                else
                {
                    currentMovePos--;
                    currentMoveTime--;
                }
            }
        }
    }
}
