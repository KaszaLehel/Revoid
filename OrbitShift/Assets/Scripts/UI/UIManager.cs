using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI touchText;
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject backgroundCoin;
    [SerializeField] private GameObject endingScreen;

    [Header("Animators Settings")]
    [SerializeField] private Animator UpperAnimator;
    [SerializeField] private Animator UnderAnimator;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        endingScreen.SetActive(false);
        backgroundCoin.SetActive(false);
    }

    public void EndingScreenActivate()
    {
        endingScreen.SetActive(true);
    }

    public void DisappearMenu()
    {

        StartCoroutine(DissapearMainMenu());
        
    }

    IEnumerator DissapearMainMenu()
    {
        backgroundCoin.SetActive(true);

        //ITT KELL BEALLUTANI AZ ANIMALT ELTUNEST
        UpperAnimator.SetTrigger("SlideOut");
        UnderAnimator.SetTrigger("SlideUnder");
        yield return new WaitForSeconds(1f);
        
        menuObject.SetActive(false);
    }
}
