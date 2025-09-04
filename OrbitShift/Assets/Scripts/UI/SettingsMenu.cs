using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private Button backButton;
    [SerializeField] private AudioClip clickAudio;

    [Header("Audio Settings")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Sprite soundOnImage;
    [SerializeField] private Sprite soundOffImage;
    private bool isOn = true;

    void Start()
    {
        if (soundOnImage == null)
        {
            soundOnImage = soundButton.image.sprite;
        }

        isOn = PlayerPrefs.GetInt("Audio", 1) == 1;
        soundButton.image.sprite = isOn ? soundOnImage : soundOffImage;
        AudioListener.volume = isOn ? 1 : 0;
    }

    public void AudioButotnClicked()
    {
        if (isOn)
        {
            soundButton.image.sprite = soundOffImage;
            isOn = false;
            AudioListener.volume = 0;
        }
        else
        {
            soundButton.image.sprite = soundOnImage;
            isOn = true;
            AudioListener.volume = 1;
        }

        PlayerPrefs.SetInt("Audio", isOn ? 1 : 0);
    }

    public void CloseSettingsMenu()
    {
        SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
        if (settingsAnimator != null)
            StartCoroutine(Close());
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Close()
    {
        backButton.interactable = false;
        settingsAnimator.SetTrigger("Disolve");
        yield return new WaitForSeconds(1f);
        backButton.interactable = true;
        gameObject.SetActive(false);
    }
}
