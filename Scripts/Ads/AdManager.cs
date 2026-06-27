//using UnityEngine;
//using GoogleMobileAds.Api;

//public class AdManager : MonoBehaviour
//{
//   // public static AdManager Instance;

//    public string bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

//    public string interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";

//    public string rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";


//    private bool SkipQuestion = false;
//    public bool skipQuestion { get { return SkipQuestion; } set { SkipQuestion = value; } }

//    private bool CountinuePlaying = false;
//    public bool countinuePlaying { get { return CountinuePlaying; } set { CountinuePlaying = value; } }

//    private bool NoAds = false;
//    public bool isNoAds { get { return NoAds; } set { NoAds = value; } }

    
//    public string CurrentShowAdName { get; set; }

//    private BannerView bannerView;

//    private GoogleMobileAds.Api.InterstitialAd interstitialAd;

//    private GoogleMobileAds.Api.RewardedAd rewardedAd;
//    int noAdsValue = 0;
//    private void Awake()
//    {
//        //Instance = this;


//    }
//    void Start()
//    {
//        MobileAds.Initialize(initStatus => { });
//        if (PlayerPrefs.HasKey(GameStrings.Ads))
//        {
//            noAdsValue = PlayerPrefs.GetInt(GameStrings.Ads);
//        }
//        isNoAds = noAdsValue == 1 ? true : false;
//        CreatRewardedAd();
//        CreatInterstitialAd();
//    }
//    public void ShowBannerAd()
//    {
//        if (!NoAds)
//        {
//            AdSize adSize = new AdSize(320, 50);
//            this.bannerView = new BannerView(bannerUnitId, adSize, AdPosition.Bottom);

//            AdRequest bannerRequest = new AdRequest.Builder().Build();

//            this.bannerView.LoadAd(bannerRequest);
//        }
//    }

//    void CreatInterstitialAd()
//    {
//        this.interstitialAd = new GoogleMobileAds.Api.InterstitialAd(interstitialUnitId);

//        AdRequest interstitialRequest = new AdRequest.Builder().Build();

//        this.interstitialAd.LoadAd(interstitialRequest);

        
//    }
//    public void UpdateInterstitialAd()
//    {
//        if (!NoAds)
//        {
//            if (!this.interstitialAd.IsLoaded())
//            {
//                CreatInterstitialAd();
//            }
//        }
//    }

//    public void ShowInterstitialAd()
//    {
//        if (!NoAds)
//        {
//            if (this.interstitialAd.IsLoaded())
//            {
//                this.interstitialAd.Show();
//            }
//        }
//    }
//    public void CreatRewardedAd()
//    {
//        this.rewardedAd = new GoogleMobileAds.Api.RewardedAd(rewardedUnitId);

//        AdRequest request = new AdRequest.Builder().Build();

//        this.rewardedAd.LoadAd(request);

//        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
//    }
//    void UserChoseToWatchAd()
//    {
//        AudioManger.PlaySound(UiGane.Instance.audio, AudioManger.Instance.clickSfx);
//        //AudioManger.Instance.ClickSound(UiGane.Instance.audio);
//        if (this.rewardedAd.IsLoaded())
//        {
//            this.rewardedAd.Show();
//        }
//    }
//    public void HandleUserEarnedReward(object sender, Reward args)
//    {
//        //string type = args.Type;
//        //double amount = args.Amount;
//        //MonoBehaviour.print(
//        //     "HandleRewardedAdRewarded event received for "
//        //         + amount.ToString() + " " + type);

//        if (CurrentShowAdName == GameStrings.SkipLevel)
//        {
//            LevelManger.Instance._levelCount++;
//            PlayerPrefs.SetInt(GameStrings.LevelCount, LevelManger.Instance._levelCount);
//            ScenceManger.Instance.LoadScence();
//        }
//        else if (CurrentShowAdName == GameStrings.DoubleCoin)
//        {
//           StartCoroutine( UiGane.Instance.winPanel.DoubleCoin());
//        }
//        CreatRewardedAd();
//    }

//    public void ShowRewardAds(string adName)
//    {
//        CurrentShowAdName = adName;
//        UserChoseToWatchAd();
//    }
//    public void DestroyBanner()
//    {
//        if (this.bannerView != null)
//        {
//            this.bannerView.Hide();
//            this.bannerView.Destroy();

//        }
//    }
//}
