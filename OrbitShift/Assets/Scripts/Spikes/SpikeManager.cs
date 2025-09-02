using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    [Header("Objects Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject crystalPrefab;

    [Header("Radius Settings")]
    [SerializeField] private float radiusInner = 3f;
    [SerializeField] private float radiusOuter = 4f;

    [Header("Spawn Settings")]
    [SerializeField] private int segments = 34;
    [SerializeField] private int spawnAhead = 10;
    [SerializeField] private int despawnBehind = 5;

    [Header("Crystal Settings")]
    [SerializeField, Range(0f, 1f)] private float crystalSpawnChance = 0.3f;
    [SerializeField] private int crystalAhead = 4;
    [SerializeField, Range(0f, 1f)] private float crystalOffset = 0.1f;

    private Dictionary<int, GameObject> spikes = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> crystals = new Dictionary<int, GameObject>();

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

        GenerateCrystalAhead(currentSegment);

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
            pendingSide = null;
        }
        else
        {
            Side side = Random.value > 0.5f ? Side.Inner : Side.Outer;

            if (lastSpawnedSide.HasValue && lastSpawnedSide.Value != side)
            {
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


    void GenerateCrystalAhead(int currentSegment)
    {
        int crystalSegment = (currentSegment + crystalAhead) % segments;

        if (crystals.ContainsKey(crystalSegment)) return;

        Side? spikeSide = GetSpikeSideAtSegment(crystalSegment);

        Side sideForCrystal = spikeSide.HasValue
            ? Opposite(spikeSide.Value)
            : (Random.value > 0.5f ? Side.Inner : Side.Outer);

        TrySpawnCrystal(crystalSegment, sideForCrystal);

    }

    // ===== TÖRLÉS =====
    void ClearBehind(int currentSegment)
    {
        List<int> toRemoveSpikes = new List<int>();
        List<int> toRemoveCrystals = new List<int>();

        foreach (var kvp in spikes)
        {
            int segIndex = kvp.Key;
            int distance = (segIndex - currentSegment + segments) % segments;

            if (distance > spawnAhead && distance <= (segments - despawnBehind))
            {
                if (kvp.Value != null && kvp.Value.TryGetComponent(out Spike spikeScript))
                {
                    spikeScript.Despawn();
                }
                else
                {
                    Destroy(kvp.Value);
                }

                toRemoveSpikes.Add(segIndex);
            }
        }

        foreach (var kvp in crystals)
        {
            int segIndex = kvp.Key;
            int distance = (segIndex - currentSegment + segments) % segments;

            if (distance > spawnAhead && distance <= (segments - despawnBehind))
            {
                Destroy(kvp.Value);
                toRemoveCrystals.Add(segIndex);
            }
        }

        foreach (int index in toRemoveSpikes) { spikes.Remove(index); }
        foreach (int index in toRemoveCrystals) { crystals.Remove(index); }
    }

    // ===== SEGÉDFÜGGVÉNYEK =====
    GameObject SpawnSpikeAt(float angle, Side side)
    {
        float rad = angle * Mathf.Deg2Rad;
        float radius = side == Side.Inner ? radiusInner : radiusOuter;
        Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

        GameObject spike = Instantiate(spikePrefab, transform);
        spike.transform.localPosition = pos;

        Vector2 dir;
        if (side == Side.Inner)
            dir = -new Vector2(pos.x, pos.y); // középpont felé
        else
            dir = new Vector2(pos.x, pos.y);  // kifelé

        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        spike.transform.rotation = Quaternion.Euler(0, 0, rotZ);

        return spike;
    }

    GameObject SpawnCrystalAt(float angle, Side side)
    {
        float rad = angle * Mathf.Deg2Rad;

        float radius = side == Side.Inner ? radiusInner - crystalOffset : radiusOuter + crystalOffset;

        Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

        GameObject spike = Instantiate(crystalPrefab, transform);
        spike.transform.localPosition = pos;

        /*Vector2 dir;
        if (side == Side.Inner)
            dir = -new Vector2(pos.x, pos.y); // középpont felé
        else
            dir = new Vector2(pos.x, pos.y);  // kifelé

        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        spike.transform.rotation = Quaternion.Euler(0, 0, rotZ);*/

        return spike;
    }

    float GetPlayerAngle()
    {
        Vector3 pos = player.localPosition.normalized;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        if (angle <= 0) angle += 360f;
        return angle;
    }



    void TrySpawnCrystal(int segment, Side side)
    {
        if (Random.value > crystalSpawnChance)
        {
            crystals[segment] = null; // jelöld, hogy erre NEM spawnoltunk kristályt
            return;
        }

        float angle = segment * angleStep;
        GameObject crystal = SpawnCrystalAt(angle, side);
        crystals[segment] = crystal;
    }


    Side? GetSpikeSideAtSegment(int segment)
    {
        if (spikes.TryGetValue(segment, out var obj) && obj != null)
            return SideOfLocalPosition(obj.transform.localPosition);
        return null;
    }

    Side SideOfLocalPosition(Vector3 localPos)
    {
        float d = localPos.magnitude;
        float diffInner = Mathf.Abs(d - radiusInner);
        float diffOuter = Mathf.Abs(d - radiusOuter);
        return (diffInner <= diffOuter) ? Side.Inner : Side.Outer;
    }
    
    Side Opposite(Side s) => (s == Side.Inner) ? Side.Outer : Side.Inner;


}
