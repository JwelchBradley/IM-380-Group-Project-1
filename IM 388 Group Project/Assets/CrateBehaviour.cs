using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehaviour : MonoBehaviour, IPlayerInteractable
{
    [SerializeField]
    private GameObject slicedVersion;

    public void CollisionEvent(GameObject other)
    {
        slicedVersion.SetActive(true);
        gameObject.SetActive(false);
    }
}
