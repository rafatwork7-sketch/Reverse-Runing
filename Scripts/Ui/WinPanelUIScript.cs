using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WinPanelUIScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button doubleCoinBtn;
    [SerializeField] private Button nextLevelBtn;

    [Header("Texts")]
    public TMP_Text coinTextInWinPanel;
    [SerializeField] private TMP_Text nextLevelText;

    [Header("Images")]
    [SerializeField] private Image nextLevelImage;

    [Header("Coin Counter")]
    public int CountFPS = 30;
    public float Duration = 1f;
    public string NumberFormat = "N0";

    private int previousValue;
    private int newValue;

    private int previousWinValue;
    private int newWinValue;

    public void ButtonsClick()
    {
        nextLevelBtn.onClick.AddListener(NextLevel);
        doubleCoinBtn.onClick.AddListener(WatchVideoForWinCoin);
    }

    private void WatchVideoForWinCoin()
    {
        AdsInitializer.Instance.videoAds.ShowAd(GameStrings.DoubleCoin);
    }

    public IEnumerator DoubleCoin()
    {
        GameManager.Instance.CheckCoinCount();

        WaitForSeconds wait = new WaitForSeconds(1f / CountFPS);

        previousValue = GameManager.Instance.Coin;
        previousWinValue = GameManager.Instance.WonCoinCount;

        newValue = GameManager.Instance.Coin + GameManager.Instance.WonCoinCount;
        newWinValue = GameManager.Instance.WonCoinCount * 2;

        int stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * Duration));
        int winStepAmount = Mathf.FloorToInt((newWinValue - previousWinValue) / (CountFPS * Duration));

        // Prevent zero step values from stopping the counter
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
                AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.coinSfx);
            }

            coinTextInWinPanel.SetText(previousWinValue.ToString(NumberFormat));

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
                AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.coinSfx);
            }

            GameUi.Instance.coinText.SetText(previousValue.ToString(NumberFormat));

            yield return wait;
        }
    }

    private void NextLevel()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        //if (LevelManager.Instance.LevelCount % 2 == 0)
        //{
        //    AdsInitializer.Instance.interstitial.ShowAd();
        //}

        GameSceneManager.Instance.ReloadCurrentScene();
    }

    public void ShowWinPanel()
    {
        gameObject.SetActive(true);

        transform.DOLocalMoveY(0f, 1.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                nextLevelBtn.image.DOFade(1f, 4f);
                nextLevelImage.DOFade(1f, 4f);
                nextLevelText.DOFade(1f, 4f);
            });

        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.winFx);
    }
}