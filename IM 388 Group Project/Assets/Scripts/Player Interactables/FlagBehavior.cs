using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehavior : MenuBehavior, IPlayerInteractable
{
    [SerializeField]
    private GameObject winScreen = null;

    public void CollisionEvent(GameObject other)
    {
        PlayerMovement tempPM = other.GetComponentInChildren<PlayerMovement>();
        tempPM.CanShoot = false;
        tempPM.HasWon = true;
        winScreen.SetActive(true);
    }
}
