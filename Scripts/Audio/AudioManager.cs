using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private const float NormalVolume = 0f;
    private const float MutedVolume = -80f;
    private const float SnapshotTransitionTime = 0.1f;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMix;
    [SerializeField] private AudioMixerSnapshot normalSound;
    [SerializeField] private AudioMixerSnapshot lowSound;

    [Header("Game Sounds")]
    public AudioClip winFx;
    public AudioClip loseFx;
    public AudioClip background;
    public AudioClip wallExplosion;
    public AudioClip slashPlayer;
    public AudioClip jumpPlayer;
    public AudioClip losePlayer;
    public AudioClip losePlayerGirl;
    public AudioClip hitPlayer;
    public AudioClip hitPlayerGirl;

    [Header("UI Sounds")]
    public AudioClip logoFx;
    public AudioClip clickSfx;
    public AudioClip storeClickSfx;

    [Header("Weapon Sounds")]
    public AudioClip pistol;

    [Header("Enemy Sounds")]
    public AudioClip skeletonDead;
    public AudioClip zombieDead;

    [Header("Countdown Sounds")]
    public AudioClip tickSfx;
    public AudioClip goSfx;
    public AudioClip coinSfx;

    [HideInInspector] public int btnMusicVol = 1;
    [HideInInspector] public int btnSfxVol = 1;

    public bool SoundSfx { get; private set; }
    public bool Music { get; private set; }

    private void Start()
    {
        LoadSfxSettings();
        LoadMusicSettings();
    }

    public void LowSound()
    {
        if (btnMusicVol == 1)
        {
            lowSound.TransitionTo(SnapshotTransitionTime);
        }
    }

    public void NormalSound()
    {
        if (btnMusicVol == 1)
        {
            normalSound.TransitionTo(SnapshotTransitionTime);
        }
    }

    public static void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    private void LoadSfxSettings()
    {
        // Load saved SFX setting
        btnSfxVol = PlayerPrefs.HasKey(GameStrings.SoundSFX)
            ? PlayerPrefs.GetInt(GameStrings.SoundSFX)
            : 1;

        if (btnSfxVol == 1)
        {
            NormalEffectAudio();
        }
        else
        {
            MuteEffectAudio();
        }
    }

    private void LoadMusicSettings()
    {
        // Load saved music setting
        btnMusicVol = PlayerPrefs.HasKey(GameStrings.Music)
            ? PlayerPrefs.GetInt(GameStrings.Music)
            : 1;

        if (btnMusicVol == 1)
        {
            NormalMusicAudio();
        }
        else
        {
            MuteMusicAudio();
        }
    }

    public void NormalMusicAudio()
    {
        audioMix.SetFloat(GameStrings.MusicAudio, NormalVolume);
        Music = true;
    }

    public void MuteMusicAudio()
    {
        audioMix.SetFloat(GameStrings.MusicAudio, MutedVolume);
        Music = false;
    }

    public void NormalEffectAudio()
    {
        audioMix.SetFloat(GameStrings.EffectAudio, NormalVolume);
        SoundSfx = true;
    }

    public void MuteEffectAudio()
    {
        audioMix.SetFloat(GameStrings.EffectAudio, MutedVolume);
        SoundSfx = false;
    }
}