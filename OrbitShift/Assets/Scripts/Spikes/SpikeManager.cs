using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    [Header("Objects Settings")]
    [SerializeField] private Transform player;          // a játékos Transform
    [SerializeField] private GameObject spikePrefab;    // tüske prefab

    [Header("Radius Settings")]
    [SerializeField] private float radiusInner = 3f;    // belső sugár
    [SerializeField] private float radiusOuter = 4f;    // külső sugár

    [Header("Spawn Settings")]
    [SerializeField] private int segments = 34;    //mennyi részre osztjuk a kört
    [SerializeField] private int spawnAhead = 10;       // mennyivel a játékos előtt generáljon (szegmens)
    [SerializeField] private int despawnBehind = 5;     // mennyivel a játékos mögött törölje (szegmens)

    private Dictionary<int, GameObject> spikes = new Dictionary<int, GameObject>();
    private float angleStep;

    private enum Side { Inner, Outer }

    private Side? lastSpawnedSide = null;
    private Side? pendingSide = null;
    
    public static SpikeManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        angleStep = 360f / segments;
    }

    void Update()
    {
        if (player == null) return;
        if (GameManager.Instance.noGameRuning) return;

        SpikeMechanic();
        
    }

    void SpikeMechanic()
    {
        int currentSegment = Mathf.FloorToInt(GetPlayerAngle() / angleStep);

        GenerateAhead(currentSegment);
        ClearBehind(currentSegment);
    }


    // ===== GENERÁLÁS =====
    void GenerateAhead(int currentSegment)
    {
        // Számoljuk ki a szegmenst, ahová spawnolni kell (player + spawnAhead)
        int spawnSegment = (currentSegment + spawnAhead) % segments;

        if (spikes.ContainsKey(spawnSegment)) return;

        Side sideToSpawn;

        if (pendingSide.HasValue)
        {
            sideToSpawn = pendingSide.Value;
            pendingSide = null; // felhasználtuk
        }
        else
        {
            // Random oldal
            Side side = Random.value > 0.5f ? Side.Inner : Side.Outer;

            if (lastSpawnedSide.HasValue && lastSpawnedSide.Value != side)
            {
                // Váltás → ezt a szegmenst kihagyjuk, de elmentjük a következőre
                spikes[spawnSegment] = null; // üres jelzés
                pendingSide = side;          // kötelezően ezzel folytatjuk a következőben
                return;
            }

            sideToSpawn = side;
        }

        float angle = spawnSegment * angleStep;
        GameObject spike = SpawnSpikeAt(angle, sideToSpawn);
        spikes[spawnSegment] = spike;

        lastSpawnedSide = sideToSpawn;
    }

    // ===== TÖRLÉS =====
    void ClearBehind(int currentSegment)
    {
        List<int> toRemove = new List<int>();

        foreach (var kvp in spikes)
        {
            int segIndex = kvp.Key;

            // távolság a játékoshoz képest (szegmensben)
            int distance = (segIndex - currentSegment + segments) % segments;

            // ha diff > spawnAhead -> jóval mögötte van
            if (distance > spawnAhead && distance <= (segments - despawnBehind))
            {
                //Destroy(kvp.Value);
                //toRemove.Add(segIndex);

                if (kvp.Value != null && kvp.Value.TryGetComponent<Spike>(out Spike spikeScript))
                {
                    spikeScript.Despawn();
                }
                else
                {
                    Destroy(kvp.Value);
                }

                toRemove.Add(segIndex);
            }
        }

        foreach (int index in toRemove)
        {
            spikes.Remove(index);
        }
    }

    // ===== SEGÉDFÜGGVÉNYEK =====
    GameObject SpawnSpikeAt(float angle, Side side)
    {
        float rad = angle * Mathf.Deg2Rad;
        float radius = side == Side.Inner ? radiusInner : radiusOuter;
        Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

        GameObject spike = Instantiate(spikePrefab, transform);
        spike.transform.localPosition = pos;

        /*Spike spikeScript = spike.GetComponent<Spike>();
        if (spikeScript != null)
        {
            spikeScript.PlayAppear();
        }*/
        

        Vector2 dir;
        if (side == Side.Inner)
            dir = -new Vector2(pos.x, pos.y); // középpont felé
        else
            dir = new Vector2(pos.x, pos.y);  // kifelé

        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; 

        spike.transform.rotation = Quaternion.Euler(0, 0, rotZ);

        return spike;
    }

    float GetPlayerAngle()
    {
        Vector3 pos = player.localPosition.normalized;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        if (angle <= 0) angle += 360f;
        return angle;
    }
}
