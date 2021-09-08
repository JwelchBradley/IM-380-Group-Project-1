/*****************************************************************************
// File Name :         LevelStartCameraBehaviour.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : Handles the cinematic at the start of levels.
*****************************************************************************/
using Cinemachine;
using System.Collections;
using UnityEngine;

public class LevelStartCameraBehaviour : MonoBehaviour
{
    [Header("Flag")]
    [SerializeField]
    [Tooltip("The flag cam")]
    private CinemachineVirtualCamera vcamFlag;

    [SerializeField]
    [Tooltip("How long the game should look at the flag")]
    private float flagCamWaitTime = 1f;

    [Header("Overview")]
    [SerializeField]
    [Tooltip("The overview cam")]
    private CinemachineVirtualCamera vcamOverview;

    [SerializeField]
    [Tooltip("How long the game should show an overview")]
    private float overviewWaitTime = 1f;

    [SerializeField]
    [Tooltip("The playermovement script of the player")]
    private PlayerMovement PM;

    /// <summary>
    /// Initializes values and starts transitions.
    /// </summary>
    private void Awake()
    {
        PM.CanShoot = false;
        PlayerMovement.CanAim = false;
        StartCoroutine(CamTransitions());
    }

    /// <summary>
    /// The transitions are handled.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CamTransitions()
    {
        yield return new WaitForSeconds(flagCamWaitTime);
        vcamFlag.m_Priority = 0;

        yield return new WaitForSeconds(overviewWaitTime);
        vcamOverview.m_Priority = 0;
        PM.CanShoot = true;
        PlayerMovement.CanAim = true;
    }
}
