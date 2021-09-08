/*****************************************************************************
// File Name :         DontFollowParent.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : Makes it so that this object doesnt follow the parent.
*****************************************************************************/
using UnityEngine;

public class DontFollowParent : MonoBehaviour
{
    /// <summary>
    /// The start position of this object.
    /// </summary>
    private Vector2 startPos;

    /// <summary>
    /// Finds the start position
    /// </summary>
    private void Awake()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// Keeps the object in its start location.
    /// </summary>
    void FixedUpdate()
    {
        transform.position = startPos;    
    }
}
