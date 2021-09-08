/*****************************************************************************
// File Name :         FlagBehaviour.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : The flag allows the player to win the level.
*****************************************************************************/
using UnityEngine;

public class FlagBehavior : MenuBehavior, IPlayerInteractable
{
    [SerializeField]
    [Tooltip("The winscreen of this level")]
    private GameObject winScreen = null;

    /// <summary>
    /// Holds true if the player has won.
    /// </summary>
    private static bool hasWon = false;

    /// <summary>
    /// Allows other scripts to change and check the win state.
    /// </summary>
    public static bool HasWon
    {
        get => hasWon;

        set
        {
            hasWon = value;
        }
    }

    /// <summary>
    /// Handles the collision event between the player and this flag. The player wins the level.
    /// </summary>
    /// <param name="other">The player.</param>
    public void CollisionEvent(GameObject other)
    {
        PlayerMovement tempPM = other.GetComponentInChildren<PlayerMovement>();
        tempPM.CanShoot = false;
        hasWon = true;
        winScreen.SetActive(true);
    }
}
