using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public bool noGameRuning = true;
    public bool gameEnded = false;
    private bool clicked = false;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
            if (gameEnded)
            {
                Debug.Log("RestartGame");
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
                Debug.Log("StartGame");
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
            if (gameEnded)
            {
                Debug.Log("RestartGame");
                clicked = true;
                SceneController.Instance.ReloadScene();
            }
            else
            {
                Debug.Log("StartGame");
                clicked = true;
                StartCoroutine(StartGame());
                //noGameRuning = false;
            }
        }
    }

    private IEnumerator StartGame()
    {
        UIManager.Instance.DisappearMenu();
        yield return new WaitForSeconds(0.5f);
        noGameRuning = false;
        clicked = false;
    }

    public void EndingSequence()
    {
        noGameRuning = true;
        gameEnded = true;
        StartCoroutine(EndingPanel());
    }

    private IEnumerator EndingPanel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.EndingScreenActivate();
    }

}
