using UnityEngine;
using ThirteenPixels.Soda;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Ses Klipleri")]
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private AudioClip playerAttackSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip levelCompleteSound;
    [SerializeField] private AudioClip themeMusic;

    [Header("Ses Ayarları")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float musicVolume = 0.3f;

    private AudioSource[] audioSources;
    private AudioSource musicSource;
    private int currentAudioSourceIndex = 0;
    private const int AUDIO_SOURCE_COUNT = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayThemeMusic();
    }

    private void InitializeAudioSources()
    {
        audioSources = new AudioSource[AUDIO_SOURCE_COUNT];
        for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlayThemeMusic()
    {
        if (themeMusic == null || musicSource == null) return;

        musicSource.clip = themeMusic;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.Play();
    }

    public void StopThemeMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = audioSources[currentAudioSourceIndex];
        source.clip = clip;
        source.volume = volume * masterVolume * sfxVolume;
        source.Play();

        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % AUDIO_SOURCE_COUNT;
    }

    // Dışarıdan çağrılacak fonksiyonlar
    public void PlayLevelUpSound()
    {
        PlaySound(levelUpSound);
    }

    public void PlayPlayerAttackSound()
    {
        PlaySound(playerAttackSound);
    }

    public void PlayPlayerDeathSound()
    {
        PlaySound(playerDeathSound);
    }

    public void PlayPlayerJumpSound()
    {
        PlaySound(playerJumpSound);
    }

    public void PlayLevelCompleteSound()
    {
        PlaySound(levelCompleteSound);
    }

    // Ses ayarları için metodlar
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
} 