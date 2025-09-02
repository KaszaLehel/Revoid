using UnityEngine;

public class SoundEfectsManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundObjectPrefab;
    public static SoundEfectsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void PlaySoundFX(AudioClip audioClip, Transform transform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObjectPrefab, transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        //float clipLength = audioSource.clip.length;
        float clipLength = audioClip.length;

        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayRandomSoundFX(AudioClip[] audioClip, Transform transform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundObjectPrefab, transform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

    }
}
