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

    private bool hasPlayed = false;

    /// <summary>
    /// Initializes values and starts transitions.
    /// </summary>
    private void Awake()
    {
        PM.CanShoot = false;
        PlayerMovement.CanAim = false;
        StartCoroutine(CamTransitions());
        ChangeTransitionTime(1, 2);
    }

    public void OnSkipSequence()
    {
        //Play();
    }

    private void Play()
    {
        if (!hasPlayed)
        {
            vcamOverview.m_Priority = 0;
            PM.CanShoot = true;
            PlayerMovement.CanAim = true;
            hasPlayed = true;
        }
        /*
        else if(vcamOverview.m_Priority == 0)
        {
             ChangeTransitionTime(1, 0.5f);
            //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>().activeCannons.Count = false;
            vcamOverview.m_Priority = 100;
        }
        else
        {
            //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerMovement>().CanShoot = true;
            vcamOverview.m_Priority = 0;
        }*/
    }

    private void FixedUpdate()
    {
        if(vcamOverview.m_Priority == 100)
        {
            PM.CanShoot = false;
        }
    }

    private void ChangeTransitionTime(int blend, float duration)
    {
        CinemachineBlendDefinition def = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseIn, duration);
        def.m_Time = duration;
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend = def;
        Camera.main.GetComponent<CinemachineBrain>().m_CustomBlends.m_CustomBlends[blend].m_Blend = def;
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

        yield return new WaitForSeconds(2);
        Play();
    }
}
