using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Thanks to https://gamedev.stackexchange.com/questions/74393/how-to-edit-key-value-pairs-like-a-dictionary-in-unitys-inspector
[CreateAssetMenu(fileName = "Dictionnary", menuName = "Scriptable Objects/Create Name to Audio Clips Dictionary")]
public class AudioClipsDictionary_ScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class AudioClipEntry
    {
        public string name;
        public AudioClip audioClip;
        public float volume;
        public float pitch = 1f;
    }

    public AudioClipEntry[] AudioClips;


    public AudioClip GetAudioClipFromName(string _researchedName)
    {
        foreach (AudioClipEntry entry in AudioClips)
        {
            if (entry.name == _researchedName)
            {
                return entry.audioClip;
            }
        }

        Debug.LogWarning("Trying to find an unamed Audio Clip: " + _researchedName);
        return null;
    }

    public float GetVolumeFromName(string _researchedName)
    {
        foreach (AudioClipEntry entry in AudioClips)
        {
            if (entry.name == _researchedName)
            {
                return entry.volume;
            }
        }

        Debug.LogWarning("Trying to find an unamed Audio Clip: " + _researchedName);
        return 0;
    }

    public float GetPitchFromName(string _researchedName)
    {
        foreach (AudioClipEntry entry in AudioClips)
        {
            if (entry.name == _researchedName)
            {
                return entry.pitch;
            }
        }

        Debug.LogWarning("Trying to find an unamed Audio Clip: " + _researchedName);
        return 0;
    }

}
