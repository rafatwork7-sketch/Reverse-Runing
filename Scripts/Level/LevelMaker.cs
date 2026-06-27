using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelMaker : Singleton<LevelMaker>
{
   
    [Header("Audio")]
    public AudioSource timerAudio;
    public AudioSource audio;

    [Header("Level Settings")]
    [SerializeField] private float minDisWall;
    [SerializeField] private float maxDisWall;
    [SerializeField] private int prefabLength = 20;
    [SerializeField] private float pointZ = -50f;
    [SerializeField] private int countDownTime = 3;

    private readonly List<GameObject> wallPrefab = new List<GameObject>();
    private readonly List<GameObject> groundPrefab = new List<GameObject>();

    private GameObject player;

    private Transform levelParent;
    private Transform groundsParent;
    private Transform wallsParent;
    private Transform enemiesParent;

    private int wallCount;
    private int groundLength;
    private int groundCount;

    private float oneStep;
    private float createEnemyPosZ;
    private Vector3 wallPosition;

    private readonly int normalWall = 0;
    private readonly int moveWall = 1;
    private readonly int slowWall = 2;
    private readonly int jumpWall = 3;

    private string playerName;

    private int rightOrLeftCount;
    private int rightAndLeftCount;
    private int midCount;
    private int wallNumber;

    public bool IsGamePlaying { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        levelParent = CreateNewObject("Level");
    }

    private void Start()
    {
        SetPlayerData();
        SetEnemyData();
        SetEnvironmentData();

        groundsParent = CreateNewObject(GameStrings.Ground);
        wallsParent = CreateNewObject(GameStrings.Wall);
        enemiesParent = CreateNewObject(GameStrings.Enemy);

        oneStep = LevelManager.Instance.oneStep;
        createEnemyPosZ = (pointZ / 11f) * 10f;

        GetGroundCount();
        CalculateWallCount();
        CreateLevel();

        StartCoroutine(Timer());
    }

    private void SetPlayerData()
    {
        // Select the saved player character
        playerName = GameStrings.MaleDummy;

        if (PlayerPrefs.HasKey(GameStrings.PlayerData))
        {
            playerName = PlayerPrefs.GetString(GameStrings.PlayerData);
        }

        if (playerName == GameStrings.MaleDummy)
        {
            player = GameData.Instance.data.playerData.maleDummy;
        }
        else if (playerName == GameStrings.FemaleDummy)
        {
            player = GameData.Instance.data.playerData.femaleDummy;
        }
        else if (playerName == GameStrings.Cowboy)
        {
            player = GameData.Instance.data.playerData.cowboy;
        }
        else if (playerName == GameStrings.Ninja)
        {
            player = GameData.Instance.data.playerData.ninja;
        }
    }

    private void SetEnemyData()
    {
        // Select the saved enemy type
        string enemyName = GameStrings.Skeleton;

        if (PlayerPrefs.HasKey(GameStrings.EnemyData))
        {
            enemyName = PlayerPrefs.GetString(GameStrings.EnemyData);
        }

        if (enemyName == GameStrings.Skeleton)
        {
            EnemyManager.Instance.enemy = GameData.Instance.data.enemyData.skeleton;
            EnemyManager.Instance.enemy.enemyName = GameStrings.Skeleton;
        }
        else if (enemyName == GameStrings.Zombie)
        {
            EnemyManager.Instance.enemy = GameData.Instance.data.enemyData.zombie;
            EnemyManager.Instance.enemy.enemyName = GameStrings.Zombie;
        }
    }

    private void SetEnvironmentData()
    {
        // Load the selected environment data
        int environmentNumber = 0;

        if (PlayerPrefs.HasKey(GameStrings.EnvironmentData))
        {
            environmentNumber = PlayerPrefs.GetInt(GameStrings.EnvironmentData);
        }

        GetEnvironmentData(environmentNumber);
    }

    public void GetEnvironmentData(int environmentNumber)
    {
        foreach (GameObject wall in GameData.Instance.data.levelData[environmentNumber].walls)
        {
            wallPrefab.Add(wall);
        }

        foreach (GameObject ground in GameData.Instance.data.levelData[environmentNumber].grounds)
        {
            groundPrefab.Add(ground);
        }
    }

    private void GetGroundCount()
    {
        groundCount = LevelManager.Instance.GroundCount;
    }

    private Transform CreateNewObject(string newObjectName)
    {
        GameObject obj = new GameObject(newObjectName);
        obj.transform.position = Vector3.zero;
        obj.transform.SetParent(levelParent);

        return obj.transform;
    }

    private void CalculateWallCount()
    {
        // Estimate how many walls should be created based on the ground length
        if (groundCount < 30)
        {
            groundLength = (groundCount - 2) * prefabLength;
        }
        else
        {
            groundLength = (groundCount - 1) * prefabLength;
        }

        wallCount = (int)(groundLength / maxDisWall) - 2;
    }

    private void CreateGround()
    {
        Vector3 spawnPoint = new Vector3(0f, -0.1f, pointZ);

        for (int i = 0; i < groundCount; i++)
        {
            GameObject ground;

            if (i < groundCount - 1)
            {
                int randomIndex = Random.Range(0, 3);
                ground = RandomGround(randomIndex, spawnPoint);
            }
            else
            {
                ground = RandomGround(groundPrefab.Count, spawnPoint);
            }

            ground.name = GameStrings.Ground;
            spawnPoint.z += prefabLength;
        }
    }

    private GameObject RandomGround(int randomIndex, Vector3 spawnPoint)
    {
        GameObject selectedGround;

        if (randomIndex < groundPrefab.Count)
        {
            selectedGround = groundPrefab[randomIndex];
        }
        else
        {
            selectedGround = groundPrefab[groundPrefab.Count - 1];
        }

        GameObject ground = Instantiate(selectedGround, spawnPoint, Quaternion.identity, groundsParent);

        GameData.Instance.currentLevelData.currentLevelData.grounds.Add(selectedGround);
        GameData.Instance.currentLevelData.currentLevelData.groundsPosition.Add(spawnPoint);

        return ground;
    }

    private void InstantiateWall(float lane, int wallIndex)
    {
        wallPosition = new Vector3(lane, 1f, pointZ);

        GameObject wall = Instantiate(wallPrefab[wallIndex], wallPosition, Quaternion.identity, wallsParent);
        wall.name = GameStrings.Wall;

        GameData.Instance.currentLevelData.currentLevelData.walls.Add(wallPrefab[wallIndex]);
        GameData.Instance.currentLevelData.currentLevelData.wallsPositions.Add(wallPosition);
    }

    private void CreateWall()
    {
        pointZ += prefabLength;

        for (int i = 0; i < wallCount; i++)
        {
            wallNumber = Random.Range(0, 5);

            // Avoid repeating the same wall pattern too many times
            if (midCount >= 3)
            {
                midCount = 0;
                wallNumber = Random.Range(1, 3);
            }

            if (rightOrLeftCount >= 2)
            {
                rightOrLeftCount = 0;
                wallNumber = Random.Range(2, 4);
            }

            if (rightAndLeftCount >= 2)
            {
                rightAndLeftCount = 0;
                int random = Random.Range(0, 2);
                wallNumber = random == 0 ? 1 : 3;
            }

            pointZ += Random.Range(minDisWall, maxDisWall);

            if (wallNumber == 1)
            {
                rightOrLeftCount++;

                int random = Random.Range(0, 2);
                float posX = random == 1 ? oneStep : -oneStep;

                InstantiateWall(posX, normalWall);
            }
            else if (wallNumber == 2)
            {
                rightAndLeftCount++;

                if (LevelManager.Instance.LevelCount < 10)
                {
                    InstantiateWall(oneStep, normalWall);
                    InstantiateWall(-oneStep, normalWall);
                }
                else if (LevelManager.Instance.LevelCount >= 15)
                {
                    int random = Random.Range(0, 2);
                    int random2 = Random.Range(0, 2);

                    random = random == 0 ? normalWall : slowWall;
                    random2 = random2 == 0 ? normalWall : slowWall;

                    InstantiateWall(oneStep, random);
                    InstantiateWall(-oneStep, random2);
                }
            }
            else if (wallNumber == 3)
            {
                midCount++;
                CreateMiddleWall();
            }
        }
    }

    private void CreateMiddleWall()
    {
        // Middle wall difficulty changes based on level progress
        if (LevelManager.Instance.LevelCount < 4)
        {
            InstantiateWall(0f, normalWall);
        }
        else if (LevelManager.Instance.LevelCount < 8)
        {
            int random = Random.Range(0, 2);
            InstantiateWall(0f, random == 0 ? normalWall : moveWall);
        }
        else if (LevelManager.Instance.LevelCount < 12)
        {
            int random = Random.Range(0, 2);
            InstantiateWall(0f, random == 0 ? normalWall : slowWall);
        }
        else if (LevelManager.Instance.LevelCount < 20)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
                InstantiateWall(0f, normalWall);
            else if (random == 1)
                InstantiateWall(0f, moveWall);
            else
                InstantiateWall(0f, slowWall);
        }
        else
        {
            minDisWall = 6.5f;
            maxDisWall = 7f;

            int random = Random.Range(0, 4);

            if (random == 0)
                InstantiateWall(0f, slowWall);
            else if (random == 1)
                InstantiateWall(0f, normalWall);
            else if (random == 2)
                InstantiateWall(0f, moveWall);
            else
                InstantiateWall(0f, jumpWall);
        }
    }

    private void CreatePlayer()
    {
        Vector3 playerPoint = Vector3.zero;
        playerPoint.y = 1f;
        playerPoint.z = pointZ + (prefabLength / 2f);

        GameObject playerObj = Instantiate(player, playerPoint, Quaternion.identity, levelParent);
        playerObj.name = playerName;
    }

    public void CreateLevel()
    {
        CreatePlayer();

        if (!GameData.Instance.currentLevelData.CanRespawn)
        {
            // Create a new random level
            CreateGround();
            CreateWall();
        }
        else
        {
            // Recreate the same level after losing
            GameData.Instance.currentLevelData.CreateLevel();
        }
    }

    private IEnumerator Timer()
    {
        // Start countdown before enabling gameplay
        while (countDownTime > 0 && !GameUi.Instance.storeUiScript.IsSelected)
        {
            GameUi.Instance.countTimeText.text = countDownTime.ToString();

            AudioManager.PlaySound(timerAudio, AudioManager.Instance.tickSfx);

            yield return new WaitForSeconds(1f);
            countDownTime--;
        }

        GameUi.Instance.countTimeText.text = "GO!";
        AudioManager.PlaySound(timerAudio, AudioManager.Instance.goSfx);

        yield return new WaitForSeconds(1f);

        GameUi.Instance.countTimeText.text = "";

        AudioManager.PlaySound(audio, AudioManager.Instance.background);

        IsGamePlaying = true;

        StartCoroutine(EnemyManager.Instance.SpawnEnemies(
            LevelManager.Instance.EnemyCount,
            enemiesParent,
            createEnemyPosZ
        ));
    }
}