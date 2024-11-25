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

        foreach (var soundEffect in soundEffects)
        {
            if (!soundDictionary.ContainsKey(soundEffect.soundName))
            {
                soundDictionary.Add(soundEffect.name, soundEffect.clip);
            }
        }
    }

    public void PlaySound(string soundName, float volume = 1.0f)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found");
        }
    }
}

