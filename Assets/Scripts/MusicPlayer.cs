using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        // Destroy new GO when new scene loads
        // Don't destroy this.GO when new scene loads

        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;

        if (numMusicPlayers > 1)
        {
            Destroy(this.gameObject);            
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
