using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontFollowParent : MonoBehaviour
{
    Vector2 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = startPos;    
    }
}
