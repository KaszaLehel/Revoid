using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] private Transform trackCenter;
    [SerializeField] private GameObject deathParticlePrefab;

    [Header("Radius Settings")]
    [SerializeField, Range(0f, 2f)] private float outerRadius = 1.6f; //1.6f
    [SerializeField, Range(0f, 2f)] private float innerRadius = 1.2f; //1.2f

    [Header("Movement Settings")]
    [SerializeField, Range(0f, 360f)] private float speed = 360f; // 50
    [SerializeField, Range(0f, 20f)] private float switchSpeed = 10f;

    [Header("Speeding Settings")]
    [SerializeField] private float maxSpeed = 75f;
    [SerializeField] private float speedIncreaseInterval = 10f;
    [SerializeField] private float speedIncreaseStep = 5f;
    [SerializeField] private float smoothDuration = 2f;

    private float angle = 270f;
    private float currentRadius;
    private float targetRadius;
    private bool onInner = false;
    private bool speedUpStarted = false;

    void Start()
    {
        currentRadius = outerRadius;
        targetRadius = outerRadius;
    }

    void Update()
    {
        if (GameManager.Instance.noGameRuning) return;

        if (!speedUpStarted)
        {
            speedUpStarted = true;
            StartCoroutine(IncreaseSpeedOverTime());
        }

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
        // Koppintás váltás
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            onInner = !onInner;
            targetRadius = onInner ? innerRadius : outerRadius;
        }

        ChangeRadius();
    }

    void HandleDevInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            onInner = !onInner;
            targetRadius = onInner ? innerRadius : outerRadius;
        }

        ChangeRadius();
    }

    void ChangeRadius()
    {
        //currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * switchSpeed);
        currentRadius = Mathf.MoveTowards(currentRadius, targetRadius, switchSpeed * Time.unscaledDeltaTime);

        angle += speed * Time.unscaledDeltaTime;
        angle %= 360f;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * currentRadius;
        transform.position = trackCenter.position + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            if (deathParticlePrefab != null)
            {
                Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            }

            Debug.Log("Ending Sequence");
            GameManager.Instance.EndingSequence();

            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator IncreaseSpeedOverTime()
    {
        while (speed < maxSpeed)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);

            float startSpeed = speed;
            float targetSpeed = Mathf.Min(speed + speedIncreaseStep, maxSpeed);

            float elapsed = 0f;
            while (elapsed < smoothDuration)
            {
                speed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / smoothDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            speed = targetSpeed;
        }
    }
}
