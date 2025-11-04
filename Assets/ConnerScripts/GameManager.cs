using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text scoreText;
    public Image rankImage;
    public Sprite[] rankSprites;
    [Space]
    [Tooltip("UI Text that shows 'Rank Up!' message")]
    public Text rankUpText;         
    public float rankUpDisplayTime = 2f; 

    [Header("Score & Combo")]
    public int score = 0;
    private int comboCount = 0;
    public float comboDuration = 5f;
    private float comboTimer;
    public int comboRank = 0;
    public int maxComboRank = 5;

    [Header("Fish Settings")]
    public GameObject[] fishPrefabs;
    public Transform[] spawnPoints;

    public float fishSpeedBoostPerRank = 0.1f;
    public int startingFish = 4;
    public int extraFishPerRank = 2;
    public float spawnInterval = 10f;

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

        if (rankUpText != null)
            rankUpText.enabled = false;
    }

    void Update()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnFish();
        }
    }

    
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

        
        foreach (GameObject fish in activeFish)
        {
            if (fish != null)
            {
                FishMovement fm = fish.GetComponent<FishMovement>();
                if (fm != null)
                    fm.BoostSpeed(1f + fishSpeedBoostPerRank * comboRank);
            }
        }

        
        for (int i = 0; i < extraFishPerRank; i++)
        {
            TrySpawnFish();
        }

        
        UpdateRankImage();

       
        if (rankUpText != null)
            StartCoroutine(ShowRankUpText());
    }

    private IEnumerator ShowRankUpText()
    {
        rankUpText.text = "Rank Up! Rank " + comboRank;
        rankUpText.enabled = true;
        rankUpText.canvasRenderer.SetAlpha(1f);

        
        yield return new WaitForSeconds(rankUpDisplayTime);

        
        rankUpText.CrossFadeAlpha(0f, 1f, false);
        yield return new WaitForSeconds(1f);

        rankUpText.enabled = false;
        rankUpText.canvasRenderer.SetAlpha(1f); 
    }

    private void ResetCombo()
    {
        comboCount = 0;
        comboRank = 0;
        comboTimer = 0f;

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

    private void SpawnStartingFish()
    {
        for (int i = 0; i < startingFish; i++)
        {
            TrySpawnFish();
        }
    }

    private void TrySpawnFish()
    {
        activeFish.RemoveAll(f => f == null || f.Equals(null));

        if (spawnPoints.Length == 0 || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("Missing spawn points or fish prefabs!");
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnPoints.Length);
        } while (newIndex == lastSpawnIndex && spawnPoints.Length > 1);

        lastSpawnIndex = newIndex;
        Transform point = spawnPoints[newIndex];

        Vector3 randomOffset = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0f);
        GameObject chosenFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        GameObject fish = Instantiate(chosenFish, point.position + randomOffset, Quaternion.identity);
        activeFish.Add(fish);
    }
}
