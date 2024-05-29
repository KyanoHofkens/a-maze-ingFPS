using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager Instance;

    [SerializeField] private AudioSource _soundFxObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayRandomSoundClip(AudioClip[] clips, Transform transform, float volume)
    {
        int _randInt = Random.Range(0, clips.Length - 1);

        // spawn GameObject
        AudioSource _audioSource = Instantiate(_soundFxObject, transform.position, Quaternion.identity);

        // assign AudioClip
        _audioSource.clip = clips[_randInt];

        // assign Volume
        _audioSource.volume = volume;

        // play sound
        _audioSource.Play();

        // get length of soundFX clip
        float _clipLength = _audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(_audioSource.gameObject, _clipLength);
    }

    public void PlaySoundClip(AudioClip clip, Transform transform, float volume)
    {
        // spawn GameObject
        AudioSource _audioSource = Instantiate(_soundFxObject, transform.position, Quaternion.identity);

        // assign AudioClip
        _audioSource.clip = clip;

        // assign Volume
        _audioSource.volume = volume;

        // play sound
        _audioSource.Play();

        // get length of soundFX clip
        float _clipLength = _audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(_audioSource.gameObject, _clipLength);
    }
}
