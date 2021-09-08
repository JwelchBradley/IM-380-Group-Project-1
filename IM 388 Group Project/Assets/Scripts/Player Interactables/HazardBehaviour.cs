/*****************************************************************************
// File Name :         HazardBehaviour.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : Handles the interaction between the player and hazards.
*****************************************************************************/
using UnityEngine;

public class HazardBehaviour : MonoBehaviour, IPlayerInteractable
{
    /// <summary>
    /// Holds true if the player has died.
    /// </summary>
    private static bool hasDied = false;

    /// <summary>
    /// Handles the collision between the player and this hazard which restarts the level.
    /// </summary>
    /// <param name="other"></param>
    public void CollisionEvent(GameObject other)
    {
        if(!FlagBehavior.HasWon && !hasDied)
        {
            other.gameObject.GetComponent<PlayerMovement>().Restart();

            hasDied = true;

            Invoke("IsAlive", 1);
        }
    }

    private void IsAlive()
    {
        hasDied = false;
    }
}
