using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameUi : MonoBehaviour
{
    public static GameUi Instance;

    [Header("Audio")]
    public AudioSource audio;

    [Header("Front UI")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text levelCountText;
    public TMP_Text coinText;
    public TMP_Text countTimeText;

    [Header("Global Black Panel")]
    [SerializeField] private Image blackPanel;

    [Header("Menu")]
    [SerializeField] private Button menuBtn;
    [SerializeField] private MenuUiScript menuUiScript;

    [Header("Win")]
    public WinPanelUIScript winPanel;

    [Header("Lose")]
    public LosePanelUIScript losePanel;

    [Header("Store")]
    public StoreUiScript storeUiScript;
    [SerializeField] private Button storeBtn;

    [Header("Ads")]
    public AdPanelUiScript adPanelUiScript;
    [SerializeField] private Button adsBtn;

    [Header("Tutorial")]
    public HowToPlayUIScripts howToPlayUIScripts;

    [Header("Coin Counter")]
    public int CountFPS = 30;
    public float Duration = 1f;
    public string NumberFormat = "N0";

    private int previousValue;
    private int newValue;

    private int previousWinValue = 0;
    private int newWinValue;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HowToPlayGame();

        winPanel.ButtonsClick();
        losePanel.ButtonsClick();
        menuUiScript.ButtonsActive();
        adPanelUiScript.ButtonsClick();

        menuBtn.onClick.AddListener(menuUiScript.ShowMenuPanel);
        adsBtn.onClick.AddListener(adPanelUiScript.ShowAdsPanel);
        storeBtn.onClick.AddListener(storeUiScript.ShowStorePanel);

        GetCoinCount();

        AudioManager.Instance.NormalSound();

        if (menuUiScript != null)
        {
            menuUiScript.CheckAudio();
        }

        previousValue = GameManager.Instance.Coin;
    }

    private void HowToPlayGame()
    {
        // Load tutorial progress
        if (PlayerPrefs.HasKey(GameStrings.FingerWithStart))
        {
            howToPlayUIScripts.FingerMove = PlayerPrefs.GetInt(GameStrings.FingerWithStart);
        }

        if (PlayerPrefs.HasKey(GameStrings.FingerJump))
        {
            howToPlayUIScripts.FingerJump = PlayerPrefs.GetInt(GameStrings.FingerJump);
        }

        if (LevelManager.Instance.LevelCount == 1 && howToPlayUIScripts.FingerMove == 0)
        {
            howToPlayUIScripts.FingerMove = 1;
            PlayerPrefs.SetInt(GameStrings.FingerWithStart, howToPlayUIScripts.FingerMove);

            howToPlayUIScripts.HowToPlayMoveActivated();
        }
        else if (LevelManager.Instance.LevelCount == 20 && howToPlayUIScripts.FingerJump == 0)
        {
            howToPlayUIScripts.FingerJump = 1;
            PlayerPrefs.SetInt(GameStrings.FingerJump, howToPlayUIScripts.FingerJump);

            howToPlayUIScripts.HowToPlayJumpActivated();
        }
    }

    public void GetCoinCount()
    {
        // Load saved coins
        if (PlayerPrefs.HasKey(GameStrings.Coin))
        {
            GameManager.Instance.Coin = PlayerPrefs.GetInt(GameStrings.Coin);
        }

        coinText.text = GameManager.Instance.Coin.ToString();
    }

    public void LevelCount(int level)
    {
        levelCountText.text = "level " + level;
    }

    public void ShowBlackPanel()
    {
        AudioManager.Instance.LowSound();

        blackPanel.enabled = true;
        blackPanel.DOFade(0.6f, 3f);
    }

    public void HideBlackPanel()
    {
        AudioManager.Instance.NormalSound();

        blackPanel.DOFade(0f, 2f)
            .OnComplete(() => blackPanel.enabled = false);
    }

    public void GetAmmoText(int currentAmmo)
    {
        ammoText.text = currentAmmo + " / ";
    }

    public IEnumerator GetCoinText(int coin, int winCoin)
    {
        WaitForSeconds wait = new WaitForSeconds(Duration / CountFPS);

        newValue = coin;
        newWinValue = winCoin;

        int stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * Duration));
        int winStepAmount = Mathf.FloorToInt((newWinValue - previousWinValue) / (CountFPS * Duration));

        // Avoid infinite loops if the step value becomes zero
        stepAmount = Mathf.Max(1, stepAmount);
        winStepAmount = Mathf.Max(1, winStepAmount);

        yield return new WaitForSeconds(1.5f);

        while (previousWinValue < newWinValue)
        {
            previousWinValue += winStepAmount;

            if (previousWinValue > newWinValue)
            {
                previousWinValue = newWinValue;
            }

            if (previousWinValue == newWinValue)
            {
                AudioManager.PlaySound(audio, AudioManager.Instance.coinSfx);
            }

            winPanel.coinTextInWinPanel.SetText(previousWinValue.ToString(NumberFormat));

            yield return wait;
        }

        while (previousValue < newValue)
        {
            previousValue += stepAmount;

            if (previousValue > newValue)
            {
                previousValue = newValue;
            }

            if (previousValue == newValue)
            {
                AudioManager.PlaySound(audio, AudioManager.Instance.coinSfx);
            }

            coinText.SetText(previousValue.ToString(NumberFormat));

            yield return wait;
        }
    }
}