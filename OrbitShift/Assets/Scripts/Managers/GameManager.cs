using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool noGameRuning = true;
    public bool gameEnded = false;
    private bool clicked = false;

    public int score = 0;

    public int bestScore { get; private set; }

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
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
                //Debug.Log("StartGame");
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
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
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

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
            Debug.Log("Új Best Score: " + bestScore);
        }
    }
    
    private IEnumerator EndingPanel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.EndingScreenActivate();
    }
}
