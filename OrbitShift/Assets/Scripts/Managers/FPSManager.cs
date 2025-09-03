using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [Tooltip("Maximum FPS limit (0 = nincs korlát, csak a kijelző frissítés számít)")]
    public int maxFPS = 0;

    void Start()
    {
        // Lekérjük a jelenlegi kijelző frissítési rátáját a refreshRateRatio segítségével
        Resolution currentResolution = Screen.currentResolution;
        int screenRefreshRate = Mathf.RoundToInt(
            (float)currentResolution.refreshRateRatio.numerator / currentResolution.refreshRateRatio.denominator
        );

        Debug.Log("Kijelző frissítési ráta: " + screenRefreshRate + " Hz");

        // Ha van maximum FPS megadva és kisebb, mint a kijelző rátája, azt használjuk
        if (maxFPS > 0 && maxFPS < screenRefreshRate)
        {
            Application.targetFrameRate = maxFPS;
            Debug.Log("FPS limit beállítva maxFPS alapján: " + maxFPS);
        }
        else
        {
            Application.targetFrameRate = screenRefreshRate;
            Debug.Log("FPS limit beállítva a kijelző frissítéshez: " + screenRefreshRate);
        }

        // VSync kikapcsolása, hogy az FPS limit működjön
        QualitySettings.vSyncCount = 0;
    }
}
