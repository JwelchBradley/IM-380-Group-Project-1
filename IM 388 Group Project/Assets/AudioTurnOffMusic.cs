using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTurnOffMusic : MonoBehaviour
{
    [SerializeField]
    private float minVol = 0.05f;

    [SerializeField]
    private float maxVol = 1;

    [SerializeField]
    private float tranistionSpeed = 1;

    private float t = 0;

    AudioSource musicAud;

    [SerializeField]
    float waitTime = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        musicAud = GameObject.Find("Level Music").GetComponent<AudioSource>();

        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        while(t < 1)
        {
            ChangeVolume(1);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(waitTime);

        while (t > 0)
        {
            ChangeVolume(-1);
            yield return new WaitForFixedUpdate();
        }
    }

    private void ChangeVolume(int sign)
    {
        musicAud.volume = Mathf.Lerp(maxVol, minVol, t);
        t += sign * Time.fixedDeltaTime * tranistionSpeed;
    }
}
