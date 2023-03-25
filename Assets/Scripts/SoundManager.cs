using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1f;

    [Range(-3, 3)]
    public float pitch = 1f;

    [Range(0, 0.5f)]
    public float pitchRandomness = 0f;


    public bool loop = false;

    [HideInInspector]
    public AudioSource audioSource;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    List<Sound> sounds = new List<Sound>();

    public static SoundManager Instance = null;
    // public GameManager gameManag;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound i in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            i.audioSource = source;
            source.pitch = i.pitch;
            source.loop = i.loop;
            source.volume = i.volume;
            source.clip = i.clip;
            source.playOnAwake = false;
        }
    }
    public float randomNum(float nuo, float iki)
    {
        return Random.Range(nuo, iki);
    }
    public void Play(string name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                //sounds[i].audioSource.mute = !GameManager.sound;
                sounds[i].audioSource.pitch = sounds[i].pitch + randomNum(-sounds[i].pitchRandomness, sounds[i].pitchRandomness);
                sounds[i].audioSource.Play();
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }

    public void Stop(string name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                sounds[i].audioSource.Stop();
                //sounds[i].audioSource.st
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }
    public void PlayOneShot(string name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                //sounds[i].audioSource.mute = !GameManager.sound;
                sounds[i].audioSource.pitch = sounds[i].pitch + randomNum(-sounds[i].pitchRandomness, sounds[i].pitchRandomness);
                sounds[i].audioSource.PlayOneShot(sounds[i].clip);
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }

    public void Play(int id)
    {
        sounds[id].audioSource.pitch = sounds[id].pitch + randomNum(-sounds[id].pitchRandomness, sounds[id].pitchRandomness);
        sounds[id].audioSource.Play();
        //Debug.Log("Paspaude");
    }

    public void Stop(int id)
    {
        sounds[id].audioSource.Stop();
    }

    public void PlayWithChangedPitch(string name, float addedPitch)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                //sounds[i].audioSource.mute = !GameManager.sound;
                sounds[i].audioSource.pitch = sounds[i].pitch + randomNum(-sounds[i].pitchRandomness, sounds[i].pitchRandomness) + addedPitch;
                sounds[i].audioSource.Play();
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }

    public void ChangePitch(string name, float pitch)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                sounds[i].audioSource.pitch = pitch;
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }
    public void ChangeVolume(string name, float volume)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                sounds[i].audioSource.volume = volume;
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }

    public void ChangeLooping(string name, bool loop)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                sounds[i].audioSource.loop = loop;
                return;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
    }

    public float GetPitch(string name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                return sounds[i].pitch;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
        return 0;
    }

    public float GetVolume(string name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (name == sounds[i].name)
            {
                return sounds[i].volume;
            }
        }
        Debug.LogWarning("Audio: " + name + " not found!");
        return 0;
    }

}
