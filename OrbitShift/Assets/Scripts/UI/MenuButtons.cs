using UnityEngine;

public class MenuButtons : MonoBehaviour
{   
    [Header("Menus")]
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject MarketMenu;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clickAudio;

    void Start()
    {
        SettingsMenu.SetActive(false);
        MarketMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
        if (SettingsMenu != null)
        {
             SettingsMenu.SetActive(true);
        }
        Debug.Log("Settings Opened");
    }

    public void OpenMarket()
    {
        SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
        if (MarketMenu != null)
        {
            MarketMenu.SetActive(true);
        }
        Debug.Log("Market Opened");
    }
}
