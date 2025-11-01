using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text scoreText;
    public Image rankImage; // 👈 Assign a UI Image (for combo rank)
    public Sprite[] rankSprites; // 👈 5 sprites for ranks 0–4

    [Header("Score & Combo")]
    public int score = 0;
    private int comboCount = 0;
    public float comboDuration = 5f; // how long before combo resets
    private float comboTimer;
    public int comboRank = 0; // 0–5 ranks
    public int maxComboRank = 5;

    [Header("Fish Settings")]
    public GameObject fishPrefab;
    public Transform[] spawnPoints;
    public int baseFishSpawn = 4;      // initial fish count
    public int extraFishPerRank = 1;   // fish added each rank
    public int maxFish = 8;            // total limit
    public float fishSpeedBoostPerRank = 0.1f; // +10% per rank
    private List<GameObject> activeFish = new List<GameObject>();

    private float spawnTimer;
    public float spawnInterval = 10f;

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

        // Periodic spawn
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnFish();
        }
    }

    // 🧮 Add score + combo tracking
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

    // 🎯 Rank progression
    private void CheckComboRankUp()
    {
        int newRank = Mathf.Clamp(comboCount / 3, 0, maxComboRank); // every 3 catches = next rank

        if (newRank > comboRank)
        {
            comboRank = newRank;
            RankUp();
        }
    }

    private void RankUp()
    {
        Debug.Log("Rank Up! Now Rank " + comboRank);

        // 1️⃣ Boost fish speed
        foreach (GameObject fish in activeFish)
        {
            if (fish != null)
            {
                FishMovement fm = fish.GetComponent<FishMovement>();
                if (fm != null)
                    fm.BoostSpeed(1f + fishSpeedBoostPerRank * comboRank);
            }
        }

        // 2️⃣ Spawn extra fish based on rank
        int extraToSpawn = extraFishPerRank * comboRank;
        for (int i = 0; i < extraToSpawn; i++)
        {
            TrySpawnFish();
        }

        // 3️⃣ Update rank image
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

    // 🎨 Update rank image in UI
    private void UpdateRankImage()
    {
        if (rankImage != null && rankSprites.Length > 0)
        {
            int index = Mathf.Clamp(comboRank, 0, rankSprites.Length - 1);
            rankImage.sprite = rankSprites[index];
        }
    }

    // 🐟 Spawn Logic
    private void SpawnStartingFish()
    {
        for (int i = 0; i < baseFishSpawn; i++)
        {
            TrySpawnFish();
        }
    }

    private void TrySpawnFish()
    {
        activeFish.RemoveAll(f => f == null);

        if (activeFish.Count >= maxFish)
            return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject fish = Instantiate(fishPrefab, point.position, Quaternion.identity);
        activeFish.Add(fish);
    }
}
