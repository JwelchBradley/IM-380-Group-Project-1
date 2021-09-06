using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehavior : MenuBehavior, IPlayerInteractable
{
    [SerializeField]
    private GameObject winScreen = null;

    private static bool hasWon = false;

    public static bool HasWon
    {
        get => hasWon;

        set
        {
            hasWon = value;
        }
    }

    public void CollisionEvent(GameObject other)
    {
        PlayerMovement tempPM = other.GetComponentInChildren<PlayerMovement>();
        tempPM.CanShoot = false;
        hasWon = true;
        winScreen.SetActive(true);
    }
}
