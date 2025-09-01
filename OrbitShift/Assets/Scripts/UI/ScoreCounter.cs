using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject particlePrefab;

    [SerializeField, Range(0f, 1f)] private float colorStrength = 0.5f;

    private int nextThreshold = 10;
    private Vector3 spawnpoint = Vector3.zero;

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.SetText("{0}", GameManager.Instance.score);

        if (GameManager.Instance.score == nextThreshold)
        {
            SpawnParticle();
            if (nextThreshold == 10) nextThreshold = 40;
            else
            {
                nextThreshold += 20;
            }
            
        }
    }

    void SpawnParticle()
    {
        GameObject particle = Instantiate(particlePrefab, spawnpoint, Quaternion.identity);

        //ParticleSystem ps = particle.TryGetComponent<ParticleSystem>(out ParticleSystem);

        if (particle.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
        {
            var main = ps.main;
            main.startColor = new Color(Random.value, Random.value, Random.value, colorStrength);

            Destroy(particle, 1f);
        }
    }
}
