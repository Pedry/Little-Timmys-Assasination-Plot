using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullscreen(int index)
    {

        switch (index)
        {
            case 0:
                Screen.SetResolution((Int32)(3840), (Int32)(2160), FullScreenMode.ExclusiveFullScreen);
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case 1:
                Screen.SetResolution((Int32)(1920 * 0.6), (Int32)(1080 * 0.6), FullScreenMode.Windowed);
                Cursor.lockState = CursorLockMode.None;
                break;
            case 2:
                Screen.SetResolution((Int32)(1920 * 0.6), (Int32)(1080 * 0.6), FullScreenMode.FullScreenWindow);
                Cursor.lockState = CursorLockMode.None;
                break;
                
        }

        
    }
}
