using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<SoundEffect> soundEffects;
    private Dictionary<string, AudioClip> soundDictionary;
    private HashSet<string> currentlyPlayingSounds;

    // ===== | Methods | =====
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        soundDictionary = new Dictionary<string, AudioClip>();
        currentlyPlayingSounds = new HashSet<string>();

        foreach (var soundEffect in soundEffects)
        {
            if (!soundDictionary.ContainsKey(soundEffect.soundName))
            {
                soundDictionary.Add(soundEffect.name, soundEffect.clip);
            }
        }
    }



    public void PlaySound(string soundName, bool varPitch, float volume = 0.8f)
    {
        if (!currentlyPlayingSounds.Contains(soundName))
        {
            if (varPitch)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                PlaySound(soundName, volume);
            }
            else
            {
                PlaySound(soundName, volume);
            }
        }
    }

    private void PlaySound(string soundName, float volume = 0.8f)
    {

        if (soundDictionary.ContainsKey(soundName))
        {
            currentlyPlayingSounds.Add(soundName);
            AudioClip audioToPlay = soundDictionary[soundName];
            audioSource.PlayOneShot(audioToPlay);

            StartCoroutine(RemoveFromCurrentlyPlaying(soundName, audioToPlay.length));
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found");
        }
    }

    public void PlaySpellSound(Spell spell)
    {
        switch (spell.Element) 
        {
            case Elements.Fire:
                PlaySound("Fire Spell");
                break;

            case Elements.Ice:
                PlaySound("Ice Spell");
                break;
        }
    }

    private IEnumerator RemoveFromCurrentlyPlaying(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);
        currentlyPlayingSounds.Remove(soundName);
    }
}

