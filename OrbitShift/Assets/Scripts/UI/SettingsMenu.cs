using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private Button backButton;

    public void CloseSettingsMenu()
    {
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
        yield return new WaitForSeconds(1);
        backButton.interactable = true;
        gameObject.SetActive(false);
    }
}
