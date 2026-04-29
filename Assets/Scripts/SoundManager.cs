using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // This makes the SoundManager globally accessible from ANY script
    public static SoundManager Instance { get; private set; }

    [Header("Music Settings")]
    public AudioSource musicSource;

    void Awake()
    {
        // Singleton setup: Ensure only one SoundManager ever exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the music playing between scene loads!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Plays background music on a loop.
    /// Usage: SoundManager.Instance.PlayMusic(myMusicClip);
    /// </summary>
    public void PlayMusic(AudioClip musicClip, float volume = 0.5f)
    {
        if (musicClip == null) return;

        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Plays a sound effect once. 
    /// Usage: SoundManager.Instance.PlaySFX(shootClip);
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f, bool varyPitch = false)
    {
        if (clip == null) return;

        // Create a temporary, invisible GameObject to play the sound
        GameObject soundObj = new GameObject("SFX_" + clip.name);
        AudioSource source = soundObj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;

        // VARY PITCH: This is the ultimate "Game Feel" trick. 
        // If 5 arrows hit at once, varying the pitch slightly makes them sound like 5 distinct impacts instead of one loud, robotic noise.
        if (varyPitch)
        {
            source.pitch = Random.Range(0.85f, 1.15f);
        }

        source.Play();

        // Tell Unity to destroy the temporary object the exact millisecond the audio finishes
        Destroy(soundObj, clip.length);
    }
}