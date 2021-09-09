/*****************************************************************************
// File Name :         DontDestroyMusic.cs
// Author :            Jacob Welch
// Creation Date :     15 June 2021
//
// Brief Description : Carrys the level music between scenes.
*****************************************************************************/
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    /// <summary>
    /// Holds reference to the music in the scene.
    /// </summary>
    private static GameObject music;

    private void Awake()
    {
        if (music == null)
        {
            music = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (gameObject.name == music.name)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(music);
                music = gameObject;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}