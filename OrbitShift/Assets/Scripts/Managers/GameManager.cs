using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip clickAudio;

    public bool noGameRuning = true;
    public bool gameEnded = false;
    private bool clicked = false;


    public int score = 0;
    public int bestScore { get; private set; }

    public int crystalsPoint = 0;
    public int allCrystalsPoint { get; private set; }

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        allCrystalsPoint = PlayerPrefs.GetInt("CrystalPoints", 0);

    }

    void Update()
    {
        if (!noGameRuning) return;
        if (clicked) return;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:

                HandleMobileInput();
                break;
            case RuntimePlatform.WindowsEditor:

                HandleDevInput();
                break;
        }
    }

    void HandleMobileInput()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            int fingerId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;

            if (gameEnded)
            {
                //Debug.Log("RestartGame");
                SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
                //Debug.Log("StartGame");
                SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
                clicked = true;
                StartCoroutine(StartGame());
                //noGameRuning = false;
            }

        }
    }

    void HandleDevInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (gameEnded)
            {
                SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
                SoundEfectsManager.Instance.PlaySoundFX(clickAudio, transform, 1f);
                clicked = true;
                StartCoroutine(StartGame());
            }
        }
    }

    private IEnumerator StartGame()
    {
        UIManager.Instance.DisappearMenu();
        yield return new WaitForSeconds(0.7f);
        noGameRuning = false;
        clicked = false;
    }

    public void EndingSequence()
    {
        noGameRuning = true;
        gameEnded = true;
        StartCoroutine(EndingPanel());

        allCrystalsPoint += crystalsPoint;
        PlayerPrefs.SetInt("CrystalPoints", allCrystalsPoint);


        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            Debug.Log("Új Best Score: " + bestScore);
        }

        PlayerPrefs.Save();
    }

    private IEnumerator EndingPanel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.EndingScreenActivate();
    }


    public void MinusCrystal(int amounth)
    {
        if (allCrystalsPoint >= amounth)
        {
            allCrystalsPoint -= amounth;
            Debug.Log(allCrystalsPoint);
        }
        return;
    }
}
