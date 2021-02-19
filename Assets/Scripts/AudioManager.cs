using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource sfxSource;
    private AudioSource musicSource;
    public static void PlaySFX(AudioClip clip, float volume, float pitch)
    {
        Instance.Play(clip, volume, pitch);
    }

    public static void PlayMusic(AudioClip clip)
    {
        Instance.Play(clip);
    }

    private void Play(AudioClip clip, float volume, float pitch)
    {
        if  (clip == null) return;

        if (sfxSource == null)
        {
            // Create audio source
            GameObject go = new GameObject("SFX Audio Source");
            go.transform.parent = transform;
            sfxSource = go.AddComponent<AudioSource>();
        }

        // Play sfx
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, volume);
    }

    private void Play(AudioClip musicClip)
    {
        Debug.Log("Play Music");
        if  (musicClip == null) return;

        // Create audio source
        if (musicSource == null)
        {
            GameObject go = new GameObject("Music Audio Source");
            go.transform.parent = transform;
            musicSource = go.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeMusic(() => Play(musicClip)));
        }
        else
        {
            musicSource.clip = musicClip;
            // Audio Source 
            musicSource.volume = 0.5f;
            musicSource.Play();
        }
    }

    private IEnumerator FadeMusic(System.Action onMusicStop = null)
    {
        float timeToLerp = 2f;
        float time = 0;

        while (musicSource.volume > 0)
        {
            musicSource.volume = Mathf.Lerp(1, 0, time / timeToLerp);
            time += Time.deltaTime;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = 1;
        onMusicStop?.Invoke();
    }
}
