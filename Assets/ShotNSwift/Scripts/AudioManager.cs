// Oleg Kotov

using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public enum SoundType
{
    Tap,
    CoinPickup,
    ObstacleImpact,
    LevelUp
}

[System.Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<Sound> sounds;

    void Awake()
    {
        if ( instance == null )
        {
            instance = this;
        }
        else
        {
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );

        foreach ( Sound sound in sounds )
        {
            GameObject childGameObject = new GameObject( "AudioSource" );
            childGameObject.transform.parent = transform;

            sound.source = childGameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.playOnAwake = false;
        }
    }

    public void Play( SoundType type )
    {
        foreach ( Sound sound in sounds )
        {
            if ( sound.type == type )
            {
                sound.source.Play();
            }
        }
    }

    public void PlayTapSound()
    {
        Play( SoundType.Tap );
    }

    public void PlayCoinPickupSound()
    {
        Play( SoundType.CoinPickup );
    }

    public void PlayObstacleImpactSound()
    {
        Play( SoundType.ObstacleImpact );
    }

    public void PlayLevelUpSound()
    {
        Play( SoundType.LevelUp );
    }
}

