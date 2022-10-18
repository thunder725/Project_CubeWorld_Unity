using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum ValidSounds { CoinPickup, Dash, HitTree }
    [SerializeField] AudioClipsDictionary_ScriptableObject AudioClipsDictionary;
    [SerializeField] AudioSource audioSource1, audioSource2, audioSource3;
    AudioSource _currentSource;

    // =============== [DEFAULT UNITY METHODS] =================

    void Awake()
    {
        // Spawn and get reference to the Audio Sources 
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource3 = gameObject.AddComponent<AudioSource>();
    }


    // ================ [GENERAL AUDIO METHODS] ====================

    public void PlaySoundWithName(ValidSounds soundName)
    {
        // Reference the name of the sound
        string _name = soundName.ToString();

        // Assign the right Audio Source
        // Use a pre-created _currentSource Variable to avoid creating one each time we play a sound
        if (!audioSource1.isPlaying)
        {
            _currentSource = audioSource1;
        }
        else if (!audioSource2.isPlaying)
        {
            _currentSource = audioSource2;
        }
        else if (!audioSource3.isPlaying)
        {
            _currentSource = audioSource3;
        }
        else
        {
            // Debug to know I need to increase the number of sources
            Debug.LogWarning("Trying to play sound " + _name + " but every Audio Source is already taken!\n Source1 taken by " + audioSource1.clip.name + ".\n Source2 taken by " + audioSource2.clip.name + ".\n Source3 taken by " + audioSource3.clip.name + ".");
        }

        _currentSource.clip = AudioClipsDictionary.GetAudioClipFromName(_name);
        _currentSource.volume = AudioClipsDictionary.GetVolumeFromName(_name);
        _currentSource.pitch = AudioClipsDictionary.GetPitchFromName(_name);

        _currentSource.Play();
    }

    public void PlaySoundWithNamePitch(ValidSounds soundName, float pitchRange)
    {
        // Reference the name of the sound
        string _name = soundName.ToString();

        // Assign the right Audio Source
        // Use a pre-created _currentSource Variable to avoid creating one each time we play a sound
        if (!audioSource1.isPlaying)
        {
            _currentSource = audioSource1;
        }
        else if (!audioSource2.isPlaying)
        {
            _currentSource = audioSource2;
        }
        else if (!audioSource3.isPlaying)
        {
            _currentSource = audioSource3;
        }
        else
        {
            // Debug to know I need to increase the number of sources
            Debug.LogWarning("Trying to play sound " + _name + " but every Audio Source is already taken!\n Source1 taken by " + audioSource1.clip.name + ".\n Source2 taken by " + audioSource2.clip.name + ".\n Source3 taken by " + audioSource3.clip.name + ".");
        }

        _currentSource.clip = AudioClipsDictionary.GetAudioClipFromName(_name);
        _currentSource.volume = AudioClipsDictionary.GetVolumeFromName(_name);
        _currentSource.pitch = AudioClipsDictionary.GetPitchFromName(_name) + Random.Range(-pitchRange, +pitchRange);

        _currentSource.Play();
    }


}