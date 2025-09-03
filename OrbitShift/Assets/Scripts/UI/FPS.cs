using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        // átlagolt deltaTime simításra
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        if (fpsText != null)
            fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";
    }
}
