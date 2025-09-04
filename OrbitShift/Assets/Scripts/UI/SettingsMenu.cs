using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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

    [Header("Contact Settings")]
    [SerializeField] private Button contactButton;
    [SerializeField] private string emailAddress = "kaszalehel@gmail.com";
    [SerializeField] private string subject = "Revoid - Contact";

    [Header("Explore Settings")]
    [SerializeField] private string url = "https://shadowscythe-games.itch.io/";

    [Header("Privacy Settings")]
    [SerializeField] private string privacyUrl = "https://sites.google.com/view/revoid-privacy-policy";

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


    public void ContactOnClick()
    {
        if (contactButton != null)
        {
            Debug.Log("MAILTO");
            string mailto = $"mailto:{emailAddress}?subject={UnityWebRequest.EscapeURL(subject)}";
            Application.OpenURL(mailto);
        }
    }


    public void ExploreOnClick()
    {
        Debug.Log("Explore Games");
        Application.OpenURL(url);
    }

    public void PrivacyOnClick()
    {
        Debug.Log("Privacy");
        Application.OpenURL(privacyUrl);
    }
}
