using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public enum FadeState
{
    none,
    fadingOut,
    fadingIn
}


public class Sound
{
    public AudioSource source { get; private set;}
    public string name { get; private set; }
    public float baseVolume { get; private set; }
    public float basePitch { get; private set; }
    public FadeState fadeState { get; private set; }

    public Sound(AudioSource newSource)
    {
        AssignSource(newSource);
    }

    public void AssignSource(AudioSource newSource)
    {
        source = newSource;
        name = newSource.gameObject.name;
        baseVolume = source.volume;
        basePitch = source.pitch;
    }

    public IEnumerator FadeOutCoroutine(float FadeTime)
    {

        if (fadeState != FadeState.none) yield break;
        fadeState = FadeState.fadingOut;

        while (source.volume > 0)
        {
            source.volume -= baseVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        source.Stop();
        source.volume = baseVolume;
        fadeState = FadeState.none;
    }



    public IEnumerator FadeInCoroutine(float FadeTime)
    {

        if (fadeState != FadeState.none) yield break;
        fadeState = FadeState.fadingIn;

        source.volume = 0.2f;
        source.Play();

        while (source.volume < baseVolume)
        {
            source.volume += baseVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        source.volume = baseVolume;
        fadeState = FadeState.none;
    }

    public void ChangeVolume(float amount)
    {
        source.volume = baseVolume + amount;
    }
}


public class Audiomanager : Singleton<Audiomanager>
{

    List<Sound> sounds;

    private void Awake()
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        sounds = new List<Sound>();
        foreach(AudioSource source in sources)
        {
            sounds.Add(new Sound(source));
        }
    }

    public static void Play(string _name)
    {
        Sound sound = Instance.sounds.First(x => x.name == _name);
        if(sound != null)
        {
            sound.source.volume = sound.baseVolume;
            sound.source.pitch = sound.basePitch;
            sound.source.Play();
        }
    }

    public static void Play(string _name, float volumeFactor)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            sound.source.volume = sound.baseVolume * volumeFactor;
            sound.source.pitch = sound.basePitch;
            sound.source.Play();
        }
    }

    public static void Play(string _name, float volumeFactor, float pitchFactor)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            sound.source.volume = sound.baseVolume * volumeFactor;
            sound.source.pitch = sound.basePitch * pitchFactor;
            sound.source.Play();
        }
    }

    public static void Stop(string _name)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }


    public static void FadeOut(string _name, float fadeTime)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            Instance.StartCoroutine(sound.FadeOutCoroutine(fadeTime));
        }

    }

    public static void FadeIn(string _name, float fadeTime)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            sound.source.volume = sound.baseVolume;
            sound.source.pitch = sound.basePitch;
            Instance.StartCoroutine(sound.FadeInCoroutine(fadeTime));
        }
    }

    public static void ChangeVolume(string _name, float volumeChange)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            sound.ChangeVolume(volumeChange);
        }
    }

    public static bool IsPlaying(string _name)
    {
        Sound sound = Instance.sounds.FirstOrDefault(x => x.name == _name);
        if (sound != null)
        {
            return sound.source.isPlaying;
        }

        else return false;
    }

    public void _Play(string name)
    {
        Play(name);
    }

}
