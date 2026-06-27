using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int coin;
    public int Coin
    {
        get => coin;
        set => coin = value;
    }

    private const int GoodCoin = 50;
    private const int VeryGoodCoin = 100;
    private const int PerfectCoinAmount = 150;

    public int WonCoinCount { get; private set; }

    [SerializeField] private float playerSpeed = 250f;
    public float PlayerSpeed
    {
        get => playerSpeed;
        set => playerSpeed = value;
    }

    [SerializeField] private float enemySpeed = 265f;
    public float EnemySpeed
    {
        get => enemySpeed;
        set => enemySpeed = value;
    }

    private void Start()
    {
        // Lock the game to 60 FPS
        Application.targetFrameRate = 60;
    }

    public void CheckCoinCount()
    {
        // Reward coins based on player performance
        if (PlayerController.Instance.PlayerHitWall <= 0)
        {
            WonCoinCount = PerfectCoinAmount;
        }
        else if (PlayerController.Instance.PlayerHitWall == 1)
        {
            WonCoinCount = VeryGoodCoin;
        }
        else
        {
            WonCoinCount = GoodCoin;
        }

        coin += WonCoinCount;
        PlayerPrefs.SetInt(GameStrings.Coin, coin);
    }
}