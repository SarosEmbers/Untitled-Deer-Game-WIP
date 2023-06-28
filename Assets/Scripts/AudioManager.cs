using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioSource nowPlaying, nowLooping, nowChill, nowTran;

    public Sound[] sounds;
    public static AudioManager _Instance;
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        } 
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "landingScene")
        {
            PlaySong("Title");
        }
    }
    public void Play(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.pitch = UnityEngine.Random.Range(0.1f, 3f);
            s.source.Play();
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
    public void PlayRandPitch(string name, float minRand, float maxRand)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.pitch = UnityEngine.Random.Range(minRand, maxRand);
            
            //Debug.Log("SOUND: " + s.source.pitch);

            s.source.Play();
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }

    public void PlaySong(string name)
    {
        if (nowPlaying != null)
        {
            nowPlaying.Stop();
        }
        if (nowLooping != null)
        {
            nowLooping.Stop();
        }
        try
        {
            Sound i = Array.Find(sounds, sound => sound.name == name);
            Sound l = Array.Find(sounds, sound => sound.name == name + " LOOP");
            //Sound c = Array.Find(sounds, sound => sound.name == name + " CHILL");
            //Sound t = Array.Find(sounds, sound => sound.name == name + " TRAN");

            nowPlaying = i.source;

            i.source.volume = 1;

            i.source.Play();
            
            if (l.clip != null)
            {
                nowLooping = l.source;
                l.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
            }/*
            if (c.clip != null)
            {
                c.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
                c.source.volume = 0.0f;
            }
            if (t.clip != null)
            {
                t.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
                t.source.volume = 0.0f;
            }
            */
            //Debug.Log("MUSIC INTRO: " + i.clip.name);
            //Debug.Log("MUSIC LOOP: " + l.clip.name);
            //Debug.Log("MUSIC CHILL: " + c.clip.name);
            //Debug.Log("MUSIC TRAN: " + t.clip.name);
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }

    public void FadeInSong(string fadeInTrack, float timeToFade)
    {
        StartCoroutine(CrossfadeTracks(fadeInTrack, timeToFade));
    }

    private IEnumerator CrossfadeTracks(string fadeInTrack, float timeToFade)
    {
        Sound g = Array.Find(sounds, sound => sound.name == fadeInTrack);

        Debug.Log("FADE IN: " + g.name);

        float timeElapsed = 0;

        while(timeElapsed < timeToFade)
        {
            if (g.source != null)
            {
                g.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            }
            nowPlaying.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    //            AudioManager._Instance.PlaySong("Boss 1");

}
