using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ManagerButtonUIScript : MonoBehaviour
{
    [SerializeField] Button buyCoinButton;
    [SerializeField] protected Button selectButton;
    [SerializeField] TMP_Text priceText;
    [SerializeField] int price;
    [SerializeField] string name;
    
    public virtual void Start()
    {
        priceText.text = price.ToString();
        buyCoinButton.onClick.AddListener(BuyCoinButtonActived);
        if (PlayerPrefs.HasKey(name))
        {
            price = PlayerPrefs.GetInt(name);
        }

        if (price <= 0)
        {
            buyCoinButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
        }
    }
    protected void BuyCoinButtonActived()
    {
        if (price <= GameManager.Instance.Coin)
        {
            AudioManager.PlaySound(GameUi.Instance.audio, AudioManager.Instance.storeClickSfx);
            //AudioManger.Instance.ClickStoreSound(UiGane.Instance.audio);
            GameManager.Instance.Coin -= price;
            PlayerPrefs.SetInt(GameStrings.Coin, GameManager.Instance.Coin);
            PlayerPrefs.SetInt(name, 0);
            GameUi.Instance.GetCoinCount();
            buyCoinButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
        }

    }
    public virtual void SelectButtonActived(){ AudioManager.PlaySound(GameUi.Instance.audio,AudioManager.Instance.storeClickSfx); }

    
}
