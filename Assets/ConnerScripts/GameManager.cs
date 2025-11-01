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
    public GameObject[] fishPrefabs; // Multiple fish prefabs here!
    public Transform[] spawnPoints;
    public int baseFishSpawn = 4;
    public int extraFishPerRank = 1;
    public int maxFish = 8;
    public float fishSpeedBoostPerRank = 0.1f; // +10% per rank

    private List<GameObject> activeFish = new List<GameObject>();
    private float spawnTimer;
    public float spawnInterval = 10f;
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
        // Handle combo timer
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }

        // Periodic spawn check
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

        // Increase max fish allowed per rank
        maxFish = 8 + comboRank;

        // 1️⃣ Increase speed of all existing fish
        foreach (GameObject fish in activeFish)
        {
            if (fish != null)
            {
                FishMovement fm = fish.GetComponent<FishMovement>();
                if (fm != null)
                    fm.BoostSpeed(1f + fishSpeedBoostPerRank * comboRank);
            }
        }

        // 2️⃣ Spawn extra fish per rank
        int extraToSpawn = extraFishPerRank * comboRank;
        for (int i = 0; i < extraToSpawn; i++)
        {
            TrySpawnFish();
        }

        // 3️⃣ Guarantee a few more fish always spawn on rank up
        TrySpawnFish();
        TrySpawnFish();

        // 4️⃣ Update rank UI
        UpdateRankImage();
    }

    private void ResetCombo()
    {
        comboCount = 0;
        comboRank = 0;
        comboTimer = 0f;

        // Reset fish speeds
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

    // 🐟 Initial Fish Spawning
    private void SpawnStartingFish()
    {
        for (int i = 0; i < baseFishSpawn; i++)
        {
            TrySpawnFish();
        }
    }

    // 🐠 Smarter Fish Spawning Logic
    private void TrySpawnFish()
    {
        // Clean up destroyed fish
        activeFish.RemoveAll(f => f == null || f.Equals(null));

        // If no prefabs or points are set, skip
        if (spawnPoints.Length == 0 || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("Missing spawn points or fish prefabs!");
            return;
        }

        // Dynamically scale fish cap with rank
        int targetFish = maxFish + (comboRank * 2);

        // If we're under the target number, fill up quickly
        int fishToSpawn = targetFish - activeFish.Count;
        if (fishToSpawn <= 0) return;

        // Spawn all missing fish at once
        for (int i = 0; i < fishToSpawn; i++)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, spawnPoints.Length);
            } while (newIndex == lastSpawnIndex && spawnPoints.Length > 1);

            lastSpawnIndex = newIndex;
            Transform point = spawnPoints[newIndex];

            // Add small random offset
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f), 0f);

            // Pick random fish type
            GameObject chosenFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            GameObject fish = Instantiate(chosenFish, point.position + randomOffset, Quaternion.identity);
            activeFish.Add(fish);
        }
    }
}

