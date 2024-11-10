using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ===== | Variables | =====
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;

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
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
