using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdPanelUiScript : MonoBehaviour
{
    [SerializeField] Button closeBtn;

  
    public void ButtonsClick()
    {
        closeBtn.onClick.AddListener(HideAdsPanel);
    }
    public void ShowAdsPanel()
    {
        GameUi.Instance.ShowBlackPanel();
        AudioManager.Instance.LowSound();
        AudioManager.PlaySound(GameUi.Instance.audio,AudioManager.Instance.clickSfx);
        gameObject.SetActive(true);
        StartCoroutine(WaitToPause());
    }
    IEnumerator WaitToPause()
    {
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0;
    }
    void HideAdsPanel()
    {
        GameUi.Instance.HideBlackPanel();
        AudioManager.Instance.NormalSound();
        AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.clickSfx);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
