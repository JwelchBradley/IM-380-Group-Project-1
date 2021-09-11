/*****************************************************************************
// File Name :         ExplosionBehaviour.cs
// Author :            Jacob T. Welch
// Creation Date :     January 28, 2021
//
// Brief Description : This script controls the functionality of explosions. It
                       destroys explosion effects when their animation is
                       finished.
*****************************************************************************/
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    /// <summary>
    /// The explosion effect's animator.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Upon creation the animator component is found.
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Checks if the explosion's animation has ended once per frame.
    /// </summary>
    private void Update()
    {
        CheckExplosionState();
    }

    /// <summary>
    /// Check if the explosion's animation has ended.
    /// </summary>
    private void CheckExplosionState()
    {
        // If the animation is no longer playing the gameObject is destroyed
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
