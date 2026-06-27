using System.Collections;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Audio")]
    public AudioSource audio;

    [Header("Level Settings")]
    public float oneStep = 1.2f;
    public bool isWin = false;
    public bool isLose = false;

    [SerializeField] private int enemyCount = 10;
    [SerializeField] private int groundCount = 20;

    private int levelCount = 1;
    private int maxAmmo = 7;

    private float playerSpeed;
    private float enemySpeed;

    public int LevelCount { get { return levelCount; } set { levelCount = value; } }
    public int EnemyCount { get { return enemyCount; } set { enemyCount = value; } }
    public int GroundCount { get { return groundCount; } set { groundCount = value; } }
    public int MaxAmmo { get { return maxAmmo; } set { maxAmmo = value; } }

    private void Start()
    {
        playerSpeed = GameManager.Instance.PlayerSpeed;
        enemySpeed = GameManager.Instance.EnemySpeed;

        GetLevelCount();
        GameDifficultyLevel();

        // Banner ads can be enabled from here when needed
        // AdManager.Instance.ShowBannerAd();
    }

    private void GetLevelCount()
    {
        // Load saved level progress
        if (PlayerPrefs.HasKey(GameStrings.LevelCount))
        {
            levelCount = PlayerPrefs.GetInt(GameStrings.LevelCount);
        }
        else
        {
            levelCount = 1;
        }

        GameUi.Instance.LevelCount(levelCount);
    }

    private void GameDifficultyLevel()
    {
        AddMoreEnemy();
        AddMoreAmmo();
        AddMoreFloor();
        CharacterSpeedIncrease();
    }

    private void AddMoreEnemy()
    {
        // Increase enemies gradually until reaching the limit
        if (enemyCount < 31)
        {
            for (int i = 1; i < levelCount; i += 3)
            {
                enemyCount++;
            }
        }
    }

    private void AddMoreAmmo()
    {
        // Ammo is based on the current enemy count
        maxAmmo = enemyCount - 2;
    }

    private void AddMoreFloor()
    {
        // Add more ground tiles as the level gets harder
        if (levelCount <= 10)
        {
            for (int i = 1; i < levelCount; i += 2)
            {
                groundCount++;
            }
        }
        else if (levelCount <= 30)
        {
            for (int i = 1; i < levelCount; i += 3)
            {
                groundCount++;
            }
        }
        else if (levelCount <= 50)
        {
            for (int i = 1; i < levelCount; i += 4)
            {
                groundCount++;
            }
        }
        else
        {
            for (int i = 1; i < levelCount; i += 5)
            {
                groundCount++;
            }
        }
    }

    private void CharacterSpeedIncrease()
    {
        // Increase player and enemy speed based on level progress
        if (levelCount <= 20)
        {
            for (int i = 1; i < levelCount; i += 3)
            {
                playerSpeed++;
                enemySpeed++;
            }
        }
        else if (levelCount <= 40)
        {
            for (int i = 1; i < levelCount; i += 4)
            {
                playerSpeed++;
                enemySpeed++;
            }
        }
        else
        {
            for (int i = 1; i < levelCount; i += 5)
            {
                playerSpeed++;
                enemySpeed++;
            }
        }
    }

    public bool GameIsFinished()
    {
        // Game can continue only if there is no win or lose state 
        return !isWin && !isLose && LevelMaker.Instance.IsGamePlaying;
    }

    public void Win()
    {
        if (isLose)
            return;

        isWin = true;

        GameData.Instance.currentLevelData.CanRespawn = false;
        GameData.Instance.currentLevelData.ResetData();

        AudioManager.Instance.LowSound();
        PlayerController.Instance.Win();
        EnemyManager.Instance.LoseEnemy();

        GameUi.Instance.ShowBlackPanel();
        GameUi.Instance.winPanel.ShowWinPanel();

        levelCount++;
        PlayerPrefs.SetInt(GameStrings.LevelCount, levelCount);

        GameManager.Instance.CheckCoinCount();

        StartCoroutine(GameUi.Instance.GetCoinText(
            GameManager.Instance.Coin,
            GameManager.Instance.WonCoinCount
        ));
    }

    public void Lose()
    {
        if (isWin)
            return;

        isLose = true;

        GameData.Instance.currentLevelData.CanRespawn = true;

        AudioManager.Instance.LowSound();
        PlayerController.Instance.Lose();
        EnemyManager.Instance.WinEnemy();

        GameUi.Instance.ShowBlackPanel();
        GameUi.Instance.losePanel.ShowLosePanel();
    }
}