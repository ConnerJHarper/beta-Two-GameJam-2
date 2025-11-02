using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text scoreText;
    public Image rankImage; // Assign a UI Image for combo rank
    public Sprite[] rankSprites; // 5 sprites for combo ranks (0–4)

    [Header("Score & Combo")]
    public int score = 0;
    private int comboCount = 0;
    public float comboDuration = 5f; // how long before combo resets
    private float comboTimer;
    public int comboRank = 0;
    public int maxComboRank = 5;

    [Header("Fish Settings")]
    [Tooltip("Add up to 5 fish prefabs here (different fish types).")]
    public GameObject[] fishPrefabs;
    public Transform[] spawnPoints;

    public float fishSpeedBoostPerRank = 0.1f; // +10% per rank
    public int startingFish = 4;               // start with 4 fish
    public int extraFishPerRank = 2;           // always +2 on rank up
    public float spawnInterval = 10f;          // optional timed spawn

    private List<GameObject> activeFish = new List<GameObject>();
    private float spawnTimer;
    private int lastSpawnIndex = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        SpawnStartingFish();
        UpdateRankImage();
    }

    void Update()
    {
        // Combo countdown
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }

        // Optional timed fish respawning
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnFish();
        }
    }

    // 🧮 Add score + combo logic
    public void AddScore(int amount)
    {
        amount = 10;
        score += amount;
        comboCount++;
        comboTimer = comboDuration;
        UpdateScoreUI();
        CheckComboRankUp();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // 🎯 Rank progression logic
    private void CheckComboRankUp()
    {
        int newRank = Mathf.Clamp(comboCount / 3, 0, maxComboRank);
        if (newRank > comboRank)
        {
            comboRank = newRank;
            RankUp();
        }
    }

    private void RankUp()
    {
        Debug.Log("Rank Up! Now Rank " + comboRank);

        // 1️⃣ Boost existing fish speed
        foreach (GameObject fish in activeFish)
        {
            if (fish != null)
            {
                FishMovement fm = fish.GetComponent<FishMovement>();
                if (fm != null)
                    fm.BoostSpeed(1f + fishSpeedBoostPerRank * comboRank);
            }
        }

        // 2️⃣ Always spawn 2 new fish each rank
        for (int i = 0; i < extraFishPerRank; i++)
        {
            TrySpawnFish();
        }

        // 3️⃣ Update rank UI
        UpdateRankImage();
    }

    private void ResetCombo()
    {
        comboCount = 0;
        comboRank = 0;
        comboTimer = 0f;

        // Reset all fish speeds
        foreach (GameObject fish in activeFish)
        {
            if (fish != null)
            {
                FishMovement fm = fish.GetComponent<FishMovement>();
                if (fm != null)
                    fm.ResetSpeed();
            }
        }

        UpdateRankImage();
    }

    private void UpdateRankImage()
    {
        if (rankImage != null && rankSprites.Length > 0)
        {
            int index = Mathf.Clamp(comboRank, 0, rankSprites.Length - 1);
            rankImage.sprite = rankSprites[index];
        }
    }

    // 🐟 Initial spawn
    private void SpawnStartingFish()
    {
        for (int i = 0; i < startingFish; i++)
        {
            TrySpawnFish();
        }
    }

    // 🐠 Spawn a single fish
    private void TrySpawnFish()
    {
        // Clean up destroyed fish
        activeFish.RemoveAll(f => f == null || f.Equals(null));

        if (spawnPoints.Length == 0 || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("Missing spawn points or fish prefabs!");
            return;
        }

        // Pick random spawn point (avoid same twice)
        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnPoints.Length);
        } while (newIndex == lastSpawnIndex && spawnPoints.Length > 1);

        lastSpawnIndex = newIndex;
        Transform point = spawnPoints[newIndex];

        // Small random offset
        Vector3 randomOffset = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0f);
        // Random fish prefab
        GameObject chosenFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        GameObject fish = Instantiate(chosenFish, point.position + randomOffset, Quaternion.identity);
        activeFish.Add(fish);
    }
}
