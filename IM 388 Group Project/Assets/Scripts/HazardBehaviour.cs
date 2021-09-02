using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehaviour : MonoBehaviour, IPlayerInteractable
{
    public void CollisionEvent(GameObject other)
    {
        GameObject.Find("Pause Menu Templates Canvas").GetComponent<PauseMenuBehavior>().RestartLevel();

        Destroy(other);
    }
}
