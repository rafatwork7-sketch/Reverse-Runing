using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUiScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button effectButton;
    [SerializeField] private Button muteSoundButton;
    [SerializeField] private Button muteEffectButton;

    private void OnEnable()
    {
        CheckAudio();
    }

    private void Start()
    {
        ButtonsActive();
    }

    public void CheckAudio()
    {
        // Update music buttons
        soundButton.gameObject.SetActive(AudioManager.Instance.Music);
        muteSoundButton.gameObject.SetActive(!AudioManager.Instance.Music);

        // Update SFX buttons
        effectButton.gameObject.SetActive(AudioManager.Instance.SoundSfx);
        muteEffectButton.gameObject.SetActive(!AudioManager.Instance.SoundSfx);
    }

    public void ButtonsActive()
    {
        closeBtn.onClick.AddListener(CloseMenuPanel);
        soundButton.onClick.AddListener(MuteMusic);
        muteSoundButton.onClick.AddListener(EnableMusic);

        effectButton.onClick.AddListener(MuteSoundSFX);
        muteEffectButton.onClick.AddListener(EnableSoundSFX);
    }

    public void MuteSoundSFX()
    {
        AudioManager.Instance.btnSfxVol = 0;
        PlayerPrefs.SetInt(GameStrings.SoundSFX, AudioManager.Instance.btnSfxVol);

        AudioManager.Instance.MuteEffectAudio();

        effectButton.gameObject.SetActive(false);
        muteEffectButton.gameObject.SetActive(true);
    }

    public void EnableSoundSFX()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        AudioManager.Instance.btnSfxVol = 1;
        PlayerPrefs.SetInt(GameStrings.SoundSFX, AudioManager.Instance.btnSfxVol);

        AudioManager.Instance.NormalEffectAudio();

        effectButton.gameObject.SetActive(true);
        muteEffectButton.gameObject.SetActive(false);
    }

    public void MuteMusic()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        AudioManager.Instance.btnMusicVol = 0;
        PlayerPrefs.SetInt(GameStrings.Music, AudioManager.Instance.btnMusicVol);

        AudioManager.Instance.MuteMusicAudio();

        soundButton.gameObject.SetActive(false);
        muteSoundButton.gameObject.SetActive(true);
    }

    public void EnableMusic()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        AudioManager.Instance.btnMusicVol = 1;
        PlayerPrefs.SetInt(GameStrings.Music, AudioManager.Instance.btnMusicVol);

        AudioManager.Instance.NormalMusicAudio();

        soundButton.gameObject.SetActive(true);
        muteSoundButton.gameObject.SetActive(false);
    }

    private void CloseMenuPanel()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        Time.timeScale = 1f;
        GameUi.Instance.HideBlackPanel();

        transform.DOLocalMoveY(1000f, 0.5f)
            .SetEase(Ease.OutSine)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void ShowMenuPanel()
    {
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);

        // Show an interstitial ad randomly before opening the menu
        int random = Random.Range(1, 3);

        if (random == 2)
        {
            AdsInitializer.Instance.interstitial.ShowAd();
        }

        gameObject.SetActive(true);

        GameUi.Instance.ShowBlackPanel();

        transform.DOLocalMoveY(0f, 0.5f)
            .SetEase(Ease.OutSine)
            .OnComplete(() => Time.timeScale = 0f);
    }
}