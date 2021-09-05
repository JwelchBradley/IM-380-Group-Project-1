using Cinemachine;
using System.Collections;
using UnityEngine;

public class LevelStartCameraBehaviour : MonoBehaviour
{
    [Header("Flag")]
    [SerializeField]
    private CinemachineVirtualCamera vcamFlag;

    [SerializeField]
    private float flagCamWaitTime = 1f;

    [Header("Overview")]
    [SerializeField]
    private CinemachineVirtualCamera vcamOverview;

    [SerializeField]
    private float overviewWaitTime = 1f;

    [SerializeField]
    private PlayerMovement PM;

    // Start is called before the first frame update
    void Awake()
    {
        PM.CanShoot = false;
        PlayerMovement.CanAim = false;
        StartCoroutine(CamTransitions());
    }

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
