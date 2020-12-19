using System;
using UnityEngine;

//Audio Manager
public class AudioManager : MonoBehaviour
{
    public Audio[] AudioArray;
    public static AudioManager instance; //Making it static so that it can be accessed in any class.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Traversing all the audios in array.
        foreach (var s in AudioArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.spatialBlend = 0.5f;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.Loop;
            s.source.pitch = s.pitch;
        }
    }
    private void Start()
    {
        Play("Theme");
    }

    //Function for playing an particular sound.
    public void Play(string name)
    {
        Audio s = Array.Find(AudioArray, AudioArray => AudioArray.name == name);
        s.source.Play();
    }
}