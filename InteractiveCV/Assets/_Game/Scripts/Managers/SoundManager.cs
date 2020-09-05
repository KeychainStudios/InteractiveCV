using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static int ID_SOUND_JUMP = 0;
    public static int ID_SOUND_ACTIVE_BUTTON = 1;
    public static SoundManager instance;

    [Header("References")]
    public AudioClip[] m_clips;

    AudioSource m_audio;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        m_audio = GetComponent<AudioSource>();
    }

    public void PlaySound(int id)
    {
        m_audio.clip = m_clips[id];
        m_audio.Play();
    }
}
