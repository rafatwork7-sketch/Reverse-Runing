using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class StoreUiScript : MonoBehaviour
{
    private const string Coin5000ProductId = "coin_5000";
    private const string Coin10000ProductId = "coin_10000";
    private const string Coin20000ProductId = "coin_20000";
    private const string RemoveAdsProductId = "remove_ads";

    [Header("UI")]
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private Button closeBtn;

    private const int Purchase5000Coins = 5000;
    private const int Purchase10000Coins = 10000;
    private const int Purchase20000Coins = 20000;

    public bool IsSelected { get; set; }

    private void Start()
    {
        closeBtn.onClick.AddListener(HideStorePanel);
    }

    public void ShowStorePanel()
    {
        AudioManager.Instance.LowSound();
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        int random = Random.Range(1, 3);

        //if (random == 2)
        //{
        //    AdsInitializer.Instance.interstitial.ShowAd();
        //}

        rootPanel.SetActive(true);
        StartCoroutine(WaitToPause());
    }

    private IEnumerator WaitToPause()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0f;
    }

    private void HideStorePanel()
    {
        AudioManager.Instance.NormalSound();
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        rootPanel.SetActive(false);
        Time.timeScale = 1f;

        if (IsSelected)
        {
            GameSceneManager.Instance.ReloadCurrentScene();
        }
    }

    public void OnPurchasingComplete(Product product)
    {
        if (product == null)
            return;

        switch (product.definition.id)
        {
            case Coin5000ProductId:
                AddPurchasedCoins(Purchase5000Coins);
                break;

            case Coin10000ProductId:
                AddPurchasedCoins(Purchase10000Coins);
                break;

            case Coin20000ProductId:
                AddPurchasedCoins(Purchase20000Coins);
                break;

            case RemoveAdsProductId:
                RemoveAds();
                break;

            default:
                Debug.LogWarning("Unknown product id: " + product.definition.id);
                break;
        }
    }

    private void RemoveAds()
    {
        // Save no-ads purchase state
        AdsInitializer.Instance.isNoAds = true;
        PlayerPrefs.SetInt(GameStrings.Ads, 1);

        AdsInitializer.Instance.bannerAd.HideBannerAd();
    }

    private void AddPurchasedCoins(int amount)
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.coinSfx);

        GameManager.Instance.Coin += amount;

        PlayerPrefs.SetInt(GameStrings.Coin, GameManager.Instance.Coin);
        GameUi.Instance.coinText.text = GameManager.Instance.Coin.ToString();
    }
}