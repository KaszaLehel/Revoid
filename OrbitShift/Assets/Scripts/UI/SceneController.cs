using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject TransitionUI;

    public bool isTransition = false;

    public static SceneController Instance { get; private set; }
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
        TransitionUI.SetActive(true);
    }

    public void ReloadScene()
    {
        isTransition = true;
        StartCoroutine(Load());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Load()
    {
        anim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //anim.SetTrigger("Start");
        //yield return new WaitForSeconds(1);
        //UIManager.Instance.ReactivateUI();
        //TransitionUI.SetActive(false);
        //isTransition = false;
    }
}
