using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapezeBehaviour : MonoBehaviour, IPlayerInteractable
{
    GameObject attachedCannon;
    BoxCollider2D bc2d;

    private float animation = 0;

    private float animationRate = .5f;

    private bool animationReverse = false;

    [SerializeField]
    private GameObject startPos;

    [SerializeField]
    private GameObject middlePos;

    [SerializeField]
    private GameObject endPos;

    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    public void CollisionEvent(GameObject other)
    {
        if(attachedCannon == null)
        {
            attachedCannon = other;
            Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
            rb2d.angularVelocity = 0;
            other.transform.rotation = Quaternion.Euler(Vector3.zero);
            rb2d.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (!animationReverse)
        {
            if (animation > 0.5f)
            {
                AnimationUpdate(2, 0.1f, animation, 1);
            }
            else
            {
                AnimationUpdate(0.1f, 2, animation, 1);
            }
        }
        else
        {
            if(animation > 0.5f)
            {
                AnimationUpdate(2, 0.1f, animation, -1);
            }
            else
            {
                AnimationUpdate(0.1f, 2, animation, -1);
            }
        }

        if(animation >= 1)
        {
            animationReverse = true;
        }
        else if(animation < 0)
        {
            animationReverse = false;
        }

        transform.position = QuadraticLerp(startPos.transform.position, middlePos.transform.position, endPos.transform.position, animation);

        AttachCannon();
    }

    private void AnimationUpdate(float min, float max, float t, int dir)
    {
        animation += Time.fixedDeltaTime * Mathf.Lerp(min, max, t) * dir;
    }

    private void AttachCannon()
    {
        if (attachedCannon != null)
        {
            attachedCannon.transform.position = bc2d.bounds.center;
        }
    }

    private Vector2 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector2.Lerp(ab, bc, t);
    }
}
