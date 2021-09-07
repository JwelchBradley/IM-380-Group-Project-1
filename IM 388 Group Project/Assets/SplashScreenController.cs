using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenController : MonoBehaviour
{
    private VideoPlayer vp;

    // Start is called before the first frame update
    void Awake()
    {
        vp = GetComponent<VideoPlayer>();

        MenuBehavior.fromSplash = true;

        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        while (!vp.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds((float)vp.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    private IEnumerator SkipSplash()
    {
        vp.frame = 168;

        yield return new WaitForFixedUpdate();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && MenuBehavior.fromSplash)
        {
            StartCoroutine(SkipSplash());
        }
    }
}
