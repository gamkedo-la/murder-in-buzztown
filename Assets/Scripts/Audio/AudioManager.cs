using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _musicSource, _effectsSource;

    #region cached audio clips
    public AudioClip buzztownThemeAudioClip;
    public AudioClip dialogueBlipAudioClip;
    public AudioClip jumpAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip deathAudioClip;
    #endregion

    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(AudioClip clip){
        // PlayOneShot wont stop simultaneous audio clips.
        _effectsSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip clip){
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void ChangeMasterVolume(float value) {
        AudioListener.volume = value;
    }

}
