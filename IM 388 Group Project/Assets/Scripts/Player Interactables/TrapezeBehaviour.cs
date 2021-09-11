/*****************************************************************************
// File Name :         SplashScreenController.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : Handles the controls for the splash screen.
*****************************************************************************/
using UnityEngine;

public class TrapezeBehaviour : MonoBehaviour, IPlayerInteractable
{
    /// <summary>
    /// The cannon that is currently attached to this trapeze.
    /// </summary>
    private GameObject attachedCannon;

    /// <summary>
    /// Holds the position that the cannon will be at.
    /// </summary>
    BoxCollider2D bc2d;

    /// <summary>
    /// Where the rope is in its animation.
    /// </summary>
    private float ropeAnimation = 0;

    /// <summary>
    /// Holds reference to which direction the animation is currently playing in.
    /// </summary>
    private bool animationReverse = false;

    [Header("Animation Positions")]
    [SerializeField]
    [Tooltip("The starting position of the rope")]
    private GameObject startPos;

    [SerializeField]
    [Tooltip("The position at the middle of the animation")]
    private GameObject middlePos;

    [SerializeField]
    [Tooltip("The end position of the rope")]
    private GameObject endPos;

    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// The collision event between the player and the trapeze.
    /// </summary>
    /// <param name="other"></param>
    public void CollisionEvent(GameObject other)
    {
        if(attachedCannon == null)
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();

            if(pm != null && pm.NumCannons != 0)
            {
                attachedCannon = other;
                Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
                rb2d.angularVelocity = 0;
                other.transform.rotation = Quaternion.Euler(Vector3.zero);
                rb2d.isKinematic = true;

                PlayerMovement.CanAim = true;
            }
        }
    }

    /// <summary>
    /// Updates the trapeze animation.
    /// </summary>
    private void FixedUpdate()
    {
        if (!animationReverse)
        {
            if (ropeAnimation > 0.5f)
            {
                AnimationUpdate(ref ropeAnimation, 2, 0.1f, ropeAnimation, 1);
            }
            else
            {
                AnimationUpdate(ref ropeAnimation, 0.1f, 2, ropeAnimation, 1);
            }
        }
        else
        {
            if(ropeAnimation > 0.5f)
            {
                AnimationUpdate(ref ropeAnimation, 2, 0.1f, ropeAnimation, -1);
            }
            else
            {
                AnimationUpdate(ref ropeAnimation, 0.1f, 2, ropeAnimation, -1);
            }
        }

        if(ropeAnimation >= 1)
        {
            animationReverse = true;
        }
        else if(ropeAnimation < 0)
        {
            animationReverse = false;
        }

        transform.position = QuadraticLerp(startPos.transform.position, middlePos.transform.position, endPos.transform.position, ropeAnimation);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(-30, 30, ropeAnimation)));

        AttachCannon();
    }

    /// <summary>
    /// Updates the animation.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="t"></param>
    /// <param name="dir"></param>
    private void AnimationUpdate(ref float anim, float min, float max, float t, int dir)
    {
        anim += Time.fixedDeltaTime * Mathf.Lerp(min, max, t) * dir;
    }

    /// <summary>
    /// Keeps the player connected to the trapeze.
    /// </summary>
    private void AttachCannon()
    {
        if (attachedCannon != null)
        {
            attachedCannon.transform.position = bc2d.bounds.center;
        }
    }

    /// <summary>
    /// Quadratically interpolates between three points.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector2.Lerp(ab, bc, t);
    }
}
