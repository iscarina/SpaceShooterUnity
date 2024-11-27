using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] musicSounds, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;

    public enum SOUNDS_ENUM
    {
        MainTheme,
        Hit,
        ShotPlayer,
        ShotEnemy,
        Die,
        PowerUp,
        Torpedo
    }

    public static readonly Dictionary<SOUNDS_ENUM, string> SOUNDS = new Dictionary<SOUNDS_ENUM, string>
    {
        { SOUNDS_ENUM.MainTheme, "MainTheme" },
        { SOUNDS_ENUM.Hit, "Hit" },
        { SOUNDS_ENUM.ShotPlayer, "ShotPlayer" },
        { SOUNDS_ENUM.ShotEnemy, "ShotEnemy" },
        { SOUNDS_ENUM.Die, "Die" },
        { SOUNDS_ENUM.PowerUp, "PowerUp" },
        { SOUNDS_ENUM.Torpedo, "Torpedo" },
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayMusic(AudioManager.SOUNDS[AudioManager.SOUNDS_ENUM.MainTheme]);
    }

    public void PlayMusic(string name)
    {
        Sound s = System.Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound Not Found {name}");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
