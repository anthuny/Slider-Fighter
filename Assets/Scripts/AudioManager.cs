using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup musicMixer;

    public Sound[] sounds;

    public Sound combatMusic;
    public Sound mapMusic;
    public Sound attackBarMusic;
    public Sound shopMusic;

    public bool sfxMuted = false;
    public bool musicMuted = false;

    float musicOldVol;
    float sfxOldVol;



    void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            if (s.music)
                s.source.outputAudioMixerGroup = musicMixer;
            else
                s.source.outputAudioMixerGroup = sfxMixer;
        }        
    }

    private void Start()
    {
        AdjustSFXVolume(.75f, true);
        AdjustMusicTrackVolume(.75f, true);

        //ToggleShopMusic(false);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s.music)
        {
            s.source.Play();
            s.source.loop = true;
        }

        if (name == "Combat")
            combatMusic = s;
        else if (name == "MapLoop")
            mapMusic = s;
        else if (name == "AttackBarLoop")
            attackBarMusic = s;
        else if (name == "M_ShopBG")
            shopMusic = s;
        else
            s.source.PlayOneShot(s.clip);
    }

    public void StopCombatMusic()
    {
        combatMusic.source.Stop();
    }

    public void StopMapMusic()
    {
        mapMusic.source.Stop();
    }

    public void StopAttackBarMusic()
    {
        attackBarMusic.source.Stop();
    }

    public void PauseCombatMusic(bool toggle)
    {
        if (toggle)
            combatMusic.source.Pause();
        else
            combatMusic.source.Play();
    }

    public void PauseAttackBarMusic(bool toggle)
    {
        if (toggle)
            attackBarMusic.source.Pause();
        else
            attackBarMusic.source.Play();
    }

    public void ToggleShopMusic(bool toggle)
    {
        if (shopMusic.name != "")
        {
            if (toggle)
                shopMusic.source.Play();
            else
            {
                if (shopMusic.source.isPlaying)
                    shopMusic.source.Pause();
            }
        }
    }

    public void IncAttackBarMusicSpeed()
    {
        //attackBarMusic.source.temp
    }

    public void PauseMapMusic(bool toggle)
    {
        if (toggle)
            mapMusic.source.Pause();
        else
            mapMusic.source.Play();
    }

    public void AdjustMusicTrackVolume(float val, bool avoidSFX = false)
    {
        if (!avoidSFX)
        {
            // Button Click SFX
            //Play("Button_Click");
        }

        if (!musicMuted)
            musicMixer.audioMixer.SetFloat("musicVol", Mathf.Log(val) * 20);

        musicOldVol = Mathf.Log(val) * 20;
    }

    public void AdjustSFXVolume(float val, bool avoidSFX = false)
    {
        if (!avoidSFX)
        {
            // Button Click SFX
            Play("Button_Click");
        }

        if (!sfxMuted)
            sfxMixer.audioMixer.SetFloat("sfxVol", Mathf.Log(val) * 20);

        sfxOldVol = Mathf.Log(val) * 20;
    }

    public void ToggleMuteSFXVolume()
    {
        if (!sfxMuted)
        {
            sfxMuted = true;

            //sfxOldVol = GetSFXVolume();
            sfxMixer.audioMixer.SetFloat("sfxVol", -80);
        }
        else
        {
            sfxMixer.audioMixer.SetFloat("sfxVol", sfxOldVol);          
            sfxMuted = false;
        }
    }

    private float GetSFXVolume()
    {
        bool result = sfxMixer.audioMixer.GetFloat("sfxVol", out sfxOldVol);
        if (result)
            return sfxOldVol;
        else
            return 0;
    }


    public void ToggleMuteMusicVolume()
    {
        if (!musicMuted)
        {
            musicMuted = true;

            //musicOldVol = GetMusicVolume();
            musicMixer.audioMixer.SetFloat("musicVol", -80);
        }
        else
        {
            sfxMixer.audioMixer.SetFloat("musicVol", musicOldVol);
            musicMuted = false;
        }
    }
    private float GetMusicVolume()
    {
        bool result = musicMixer.audioMixer.GetFloat("musicVol", out musicOldVol);
        if (result)
            return musicOldVol;
        else
            return 0;
    }
}   

