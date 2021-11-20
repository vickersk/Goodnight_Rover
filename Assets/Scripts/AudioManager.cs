using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using Boo.Lang;

//This code is mostly from the Brackeys AudioManager youtube video https://www.youtube.com/watch?v=6OT43pvUyfY&t=0s
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private AppManager appManager;

    public static AudioManager instance;


    private bool playingMusic = false;


    void Awake()
    {   
        //singleton implementation 
        if (instance == null)
        {
            instance = this;

            //make sure that this object is not destroyed when switching between scenes
            DontDestroyOnLoad(this.gameObject);


            appManager = GetComponent<AppManager>();

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                if (s.isMusic)
                {
                    s.source.volume = s.volume * appManager.musicVolume;
                }
                else
                {
                    s.source.volume = s.volume * appManager.fxVolume;
                }
                s.source.loop = s.loop;
            }

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        

    }

    public void Play(string soundName)
    {
        //find the sound in the sound array and play it
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) { return; }     
        s.source.Play();
    }


    private void Start()
    {
        UpdateVolume();
        //play music for the scene
        if (!playingMusic)
        {
            StartCoroutine(MusicRoutine());
        }
        

    }

    private IEnumerator MusicRoutine()
    {
        playingMusic = true;
        while (true)
        {
            Play("Theme1");
            yield return new WaitForSeconds(sounds[0].clip.length);
            Play("Theme2");
            yield return new WaitForSeconds(sounds[1].clip.length);
            Play("Theme3");
            yield return new WaitForSeconds(sounds[2].clip.length);
        }
        
    }

    public void UpdateVolume()
    {
        foreach (Sound s in sounds)
        {
            if (s.isMusic)
            {
                s.source.volume = s.volume * (AppManager.instance.musicVolume / 10f);
            }
            else
            {
                s.source.volume = s.volume * (AppManager.instance.fxVolume / 10f);
            }
        }
    }


}
