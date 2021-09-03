using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOnCollision : MonoBehaviour, IPlayerInteractable
{
    private bool canFirePlayer = true;

    [SerializeField]
    private float fireAgainWaitTime = 1;

    public void CollisionEvent(GameObject other)
    {
        if (canFirePlayer)
        {
            canFirePlayer = false;
            other.GetComponent<PlayerMovement>().OnShootPlayer();
            StartCoroutine(AllowFire());
        }
    }

    private IEnumerator AllowFire()
    {
        yield return new WaitForSeconds(fireAgainWaitTime);
        canFirePlayer = true;
    }
}
