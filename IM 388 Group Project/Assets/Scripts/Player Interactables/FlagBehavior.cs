using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehavior : MenuBehavior, IPlayerInteractable
{
    [SerializeField]
    private GameObject winScreen = null;

    public void CollisionEvent(GameObject other)
    {
        other.GetComponentInChildren<PlayerMovement>().CanShoot = false;
        winScreen.SetActive(true);
    }
}
