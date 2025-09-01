using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundCoin;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject endingMenu;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("Animation Settings")]
    [SerializeField] private Animator MainMenuAnimator;

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
        MainMenu.SetActive(true);
        backgroundCoin.SetActive(false);
        endingMenu.SetActive(false);

        bestScoreText.SetText("Best Score: {0}", GameManager.Instance.bestScore);
    }



    #region Dissapear Main Menu

    public void DisappearMenu()
    {
        backgroundCoin.SetActive(true);
        StartCoroutine(SlideOutMenu());
    }

    IEnumerator SlideOutMenu()
    {
        MainMenuAnimator.SetTrigger("SlideOut");
        yield return new WaitForSeconds(1f);
        MainMenu.SetActive(false);
    }
    #endregion


    #region Ending screen
    public void EndingScreenActivate()
    {
        endingMenu.SetActive(true);
    }
    #endregion
}
