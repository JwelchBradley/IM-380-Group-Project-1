/*****************************************************************************
// File Name :         IPlayerInteractable.cs
// Author :            Jacob T. Welch
// Creation Date :     August 30, 2021
//
// Brief Description : An interface for player object collision interactions.
*****************************************************************************/
using UnityEngine;

interface IPlayerInteractable
{
    /// <summary>
    /// Handles the collision event of the player and the other object.
    /// </summary>
    /// <param name="other">The player.</param>
    public void CollisionEvent(GameObject other);
}
