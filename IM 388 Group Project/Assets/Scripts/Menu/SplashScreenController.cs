/*****************************************************************************
// File Name :         SplashScreenController.cs
// Author :            Jacob Welch
// Creation Date :     3 September 2021
//
// Brief Description : Handles the controls for the splash screen.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenController : MonoBehaviour
{
    /// <summary>
    /// The video player component of the splashscreen.
    /// </summary>
    private VideoPlayer vp;

    /// <summary>
    /// Initializes components and calls for the main menu to be loaded.
    /// </summary>
    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();

        MenuBehavior.fromSplash = true;

        StartCoroutine(LoadMainMenu());
    }

    /// <summary>
    /// Loads the main menu after video is done playing.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadMainMenu()
    {
        while (!vp.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds((float)vp.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Destroy(gameObject, 1);
    }

    /// <summary>
    /// Skips to the main menu from the splash screen.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SkipSplash()
    {
        vp.frame = 166;

        yield return new WaitForFixedUpdate();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Destroy(gameObject, 1);
    }

    /// <summary>
    /// If the player presses space then the main menu is loaded.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && MenuBehavior.fromSplash)
        {
            StartCoroutine(SkipSplash());
        }
    }
}
