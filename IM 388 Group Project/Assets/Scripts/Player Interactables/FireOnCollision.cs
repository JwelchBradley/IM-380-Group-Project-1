/*****************************************************************************
// File Name :         FireOnCollision.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : The cannon automatically fires when hitting this object.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class FireOnCollision : MonoBehaviour, IPlayerInteractable
{
    /// <summary>
    /// Holds true if this fire collision can fire the player.
    /// </summary>
    private bool canFirePlayer = true;

    [SerializeField]
    [Tooltip("How long before it can fire the player again.")]
    private float fireAgainWaitTime = 1;

    /// <summary>
    /// Handles the collision event between the player and this object. Automatically fires the player.
    /// </summary>
    /// <param name="other"></param>
    public void CollisionEvent(GameObject other)
    {
        if (canFirePlayer)
        {
            canFirePlayer = false;
            other.GetComponent<PlayerMovement>().ShootPlayer();
            StartCoroutine(AllowFire());
        }
    }

    /// <summary>
    /// Allows this object to fire the player again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllowFire()
    {
        yield return new WaitForSeconds(fireAgainWaitTime);
        canFirePlayer = true;
    }
}
