using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LosePanelUIScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button restartLevelBtn;
    [SerializeField] private Button skipLevelBtn;

    [Header("Texts")]
    [SerializeField] private TMP_Text restartLevelText;

    public void ButtonsClick()
    {
        restartLevelBtn.onClick.AddListener(RestartGame);
        skipLevelBtn.onClick.AddListener(WatchVideoSkipLevel);
    }

    private void WatchVideoSkipLevel()
    {
        AdsInitializer.Instance.videoAds.ShowAd(GameStrings.SkipLevel);
    }

    private void RestartGame()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        int random = Random.Range(1, 3);

        if (LevelManager.Instance.LevelCount % 2 == 0 && random == 2)
        {
            AdsInitializer.Instance.interstitial.ShowAd();
        }

        GameSceneManager.Instance.ReloadCurrentScene();
    }

    public void ShowLosePanel()
    {
        gameObject.SetActive(true);

        transform.DOLocalMoveY(0f, 1.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                restartLevelBtn.image.DOFade(1f, 4f);
                restartLevelText.DOFade(1f, 4f);
            });

        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.loseFx);
    }
}