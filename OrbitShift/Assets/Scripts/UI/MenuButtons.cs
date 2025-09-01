using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject SettingsMenu;

    void Start()
    {
        SettingsMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        Debug.Log("Settings Opened");
    }
}
