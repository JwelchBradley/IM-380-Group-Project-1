/*****************************************************************************
// File Name :         DontDestroyMusic.cs
// Author :            Jacob Welch
// Creation Date :     15 June 2021
//
// Brief Description : Carrys the level music between scenes.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMusic : MonoBehaviour
{
    /// <summary>
    /// Holds reference to the music in the scene.
    /// </summary>
    private static GameObject music;

    /// <summary>
    /// Checks if there is a music object already in the scene.
    /// </summary>
    void Awake()
    {
        if (music == null)
        {
            music = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Destroys the music on the main menu level.
    /// </summary>
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            //Destroy(gameObject);
        }
    }
}