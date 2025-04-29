using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    public static WinPanel instance;

    private GameObject m_noAdsBtnObj;
    private GameObject m_continueBtnObj;
    private GameObject m_claimBtnObj;
    private GameObject m_adsClaimBtnObj;
    private GameObject m_coinTextObj;

    private bool m_shouldShowAd;
    private int m_rewardCoinNum;
    private int m_adsCoinRate;

    WinPanel()
    {
        instance = this;
    }

    private new void Awake()
    {
        m_shouldShowAd = false;
        m_rewardCoinNum = 0;
        m_adsCoinRate = 1;

        Event.AddEventListener(Event.Type.OnNoAdsBought, OnNoAdsBought);

        m_noAdsBtnObj = gameObject.transform.Find("Bg/NoAdsBtn").gameObject;
        m_continueBtnObj = gameObject.transform.Find("Bg/ContinueBtn").gameObject;
        m_claimBtnObj = gameObject.transform.Find("Bg/Reward/ClaimBtn").gameObject;
        m_adsClaimBtnObj = gameObject.transform.Find("Bg/Reward/AdsClaimBtn").gameObject;
        m_coinTextObj = gameObject.transform.Find("Bg/Reward/Text").gameObject;

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();

        // clear continuous lose
        LevelData.continuousLoseCount = 0;

        // Gain new road skin
        IEnumerator _()
        {
            yield return new WaitForSeconds(1.0f);
            PlayerData.TryGainNewRoad_LevelPassed();
        }
        StartCoroutine(_());

        // Title
        string title;
        if (BattleScene.instance.GetCanvas().GetBattleLevel().IsBonusLevel())
        {
            title = "BONUS LEVEL\nCOMPLETED!";
        }
        else
        {
            title = string.Format("LEVEL {0}\nCOMPLETED!", LevelData.GetCurLevel() - 1);
        }
        gameObject.transform.Find("Bg/Title").GetComponent<Text>().text = title;

        // Open action
        gameObject.transform.Find("Bg").GetComponent<Animator>().Play("WinPanel_Open", 0, 0);

        // Particle System
        gameObject.transform.Find("Bg/ConfettiDirectionalMagic").GetComponent<ParticleSystem>().Play();
    }

    private void OnNoAdsBought()
    {
        UpdateNoAdsBtn();
    }

    private void UpdateNoAdsBtn()
    {
        if (PlayerData.hasRemovedAds || Const_test.noAds)
        {
            m_noAdsBtnObj.SetActive(false);
        }
        else
        {
            // Ads
            InterstitialHelper.instance.AddWinCount();
            m_shouldShowAd = InterstitialHelper.instance.CheckShouldShowAd();
            m_noAdsBtnObj.SetActive(m_shouldShowAd);
        }
    }

    // Reward coin
    public void SetRewardCoinNum(int num)
    {
        m_rewardCoinNum = num;

        if (m_rewardCoinNum > 0)
        {
            gameObject.transform.Find("Bg/Reward").gameObject.SetActive(true);

            // Claim btn
            m_claimBtnObj.SetActive(true);
            m_adsClaimBtnObj.SetActive(true);

            // Normal claim button should appear later
            m_claimBtnObj.GetComponent<Animator>().Play("BtnTransparent");
            IEnumerator _()
            {
                yield return new WaitForSeconds(0.8f);
                m_claimBtnObj.GetComponent<Animator>().Play("BtnFadeIn");
            }
            StartCoroutine(_());

            // Ads Coin rate
            if (BattleScene.instance.GetCanvas().GetBattleLevel().IsBonusLevel())
            {
                m_adsCoinRate = RandomUtils.ListChoiceOne<int>(Table_constant.data["bonus_level_ads_coin_rate"].param2);
            }
            else
            {
                m_adsCoinRate = Table_constant.data["normal_level_ads_coin_rate"].param1;
            }
            m_adsClaimBtnObj.transform.Find("Text").GetComponent<Text>().text = string.Format("CLAIM x{0}", m_adsCoinRate);

            // Continue btn
            m_continueBtnObj.SetActive(false);
            m_noAdsBtnObj.SetActive(false);
        }
        else
        {
            m_claimBtnObj.SetActive(false);
            m_adsClaimBtnObj.SetActive(false);

            m_continueBtnObj.SetActive(true);
            UpdateNoAdsBtn();
        }

        // Coin text
        m_coinTextObj.GetComponent<Text>().text = m_rewardCoinNum.ToString();
    }

    private void ClaimCoins(int times)
    {
        if (m_rewardCoinNum > 0)
        {
            IEnumerator _(int _coinNum)
            {
                yield return new WaitForSeconds(0.5f);
                PlayerData.ChangeCoin(_coinNum);
            }
            StartCoroutine(_(m_rewardCoinNum * times));

            // Gain coin ani
            FlyCoinAni flyComp = gameObject.GetComponent<FlyCoinAni>();
            flyComp.dropMaxRange = times <= 1 ? 150 : 250;
            flyComp.count = m_rewardCoinNum * times;
            flyComp.RunAni();

            m_rewardCoinNum = 0;
            m_claimBtnObj.SetActive(false);
            m_adsClaimBtnObj.SetActive(false);
            m_coinTextObj.GetComponent<Text>().text = "0";

            m_continueBtnObj.SetActive(true);
            UpdateNoAdsBtn();
        }
        else
        {
            m_continueBtnObj.SetActive(true);
        }
    }

    public void OnClaimBtnClk()
    {
        ClaimCoins(1);
    }

    public void OnAdsClaimBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded);
    }

    private void OnRewardVideoEnded()
    {
        ClaimCoins(m_adsCoinRate);
    }

    public void OnNoAdsBtnClk()
    {
        NoAdsPanel.instance.OpenPanel();
    }

    public void OnContitnueBtnClk()
    {
        ClosePanel();
        MainPanel.instance.BackToMainPanel();
        BattlePanel.instance.ResetPanel();

        if (m_shouldShowAd)
        {
            //UnityAdsHelper.instance.TryShowInterstitial();
            InterstitialHelper.instance.ResetCount();
        }
    }
}
