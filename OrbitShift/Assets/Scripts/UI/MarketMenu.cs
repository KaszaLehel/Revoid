using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MarketMenu : MonoBehaviour
{

    [SerializeField] private Animator marketAnimator;
    [SerializeField] private Button backButton;
    [SerializeField] private AudioClip clickAudio;

    public void CloseMarketMenu()
    {
        SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);

        if (marketAnimator != null)
        {
            //Debug.Log("Clicked");
            StartCoroutine(Close());
        }

        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Close()
    {
        backButton.interactable = false;
        marketAnimator.SetTrigger("Disolve");
        yield return new WaitForSeconds(1f);
        backButton.interactable = true;
        gameObject.SetActive(false);
    }
}
