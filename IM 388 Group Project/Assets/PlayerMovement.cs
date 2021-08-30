/*****************************************************************************
// File Name :         PlayerMovement.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField]
    [Tooltip("How much force cannons shoot out objects")]
    [Range(500, 5000)]
    private float shootForce = 1000;

    [SerializeField]
    [Tooltip("How much force pushes the cannon back when it shoots")]
    [Range(100, 1000)]
    private float shootPushBackForce = 100;

    [SerializeField]
    [Tooltip("How much angular momentum is added to the cannon when it is shot")]
    [Range(200, 4000)]
    private float shootTorque = 800;

    [SerializeField]
    [Tooltip("The max velocity of cannons")]
    [Range(5, 30)]
    private float maxSpeed = 10;

    [SerializeField]
    [Tooltip("The max amount of spin a cannon can have")]
    [Range(300, 1000)]
    private float maxAngularVelocity = 600;

    [SerializeField]
    [Tooltip("The position the cannon is shot from")]
    private GameObject shootPos;

    [SerializeField]
    [Tooltip("The cannon prefab that is left behind")]
    private GameObject cannonPrefab;

    /// <summary>
    /// The Rigidbody2D component of this cannon.
    /// </summary>
    private Rigidbody2D rb2d;
    #endregion

    #region Functions
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(rb2d.velocity.magnitude > maxSpeed)
        {
            Vector2 newVel = rb2d.velocity.normalized * maxSpeed;
            rb2d.velocity = newVel;
        }

        if(rb2d.angularVelocity > maxAngularVelocity)
        {
            rb2d.angularVelocity = maxAngularVelocity;
        }
    }

    public void OnShootPlayer()
    {
        GameObject tempCannon = Instantiate(cannonPrefab, transform.position, Quaternion.identity);

        tempCannon.transform.localScale = transform.localScale;
        gameObject.transform.localScale = transform.localScale / 1.2f;

        Vector2 shootDir = shootPos.transform.position - transform.position;
        shootDir.Normalize();
        tempCannon.GetComponent<Rigidbody2D>().AddForce(-shootDir * shootPushBackForce);

        transform.position = shootPos.transform.position;
        rb2d.AddForce(shootDir * shootForce);
        rb2d.AddTorque(shootTorque);
    }
    #endregion
}
