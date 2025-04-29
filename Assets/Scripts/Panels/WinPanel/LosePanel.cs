using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : BasePanel
{
    public static LosePanel instance;

    private GameObject m_noAdsBtnObj;
    private GameObject m_continueBtnObj;
    private GameObject m_claimBtnObj;
    private GameObject m_coinTextObj;

    private bool m_shouldShowAd;
    private int m_rewardCoinNum;

    LosePanel()
    {
        instance = this;
    }

    private new void Awake()
    {
        m_shouldShowAd = false;
        m_rewardCoinNum = 0;

        Event.AddEventListener(Event.Type.OnNoAdsBought, OnNoAdsBought);

        m_noAdsBtnObj = gameObject.transform.Find("Bg/NoAdsBtn").gameObject;
        m_continueBtnObj = gameObject.transform.Find("Bg/ContinueBtn").gameObject;
        m_claimBtnObj = gameObject.transform.Find("Bg/Reward/ClaimBtn").gameObject;
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

        // continuous lose, show skip btn
        LevelData.continuousLoseCount += 1;

        // Skip level btn
        UpdateSkipLevelBtn();
    }

    public void UpdateSkipLevelBtn()
    {
        bool showSkipBtn = LevelData.continuousLoseCount >= 3;
        gameObject.transform.Find("Bg/SkipLevelBtn").gameObject.SetActive(showSkipBtn);
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
            InterstitialHelper.instance.AddLoseCount();
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
            m_claimBtnObj.SetActive(true);
            m_continueBtnObj.SetActive(false);
            m_noAdsBtnObj.SetActive(false);
        }
        else
        {
            m_claimBtnObj.SetActive(false);
            m_continueBtnObj.SetActive(true);
            UpdateNoAdsBtn();
        }

        m_coinTextObj.GetComponent<Text>().text = m_rewardCoinNum.ToString();
    }

    public void OnClaimBtnClk()
    {
        if (m_rewardCoinNum > 0)
        {
            IEnumerator _(int _coinNum)
            {
                yield return new WaitForSeconds(0.5f);
                PlayerData.ChangeCoin(_coinNum);
            }
            StartCoroutine(_(m_rewardCoinNum));

            // Gain coin ani
            gameObject.GetComponent<FlyCoinAni>().count = m_rewardCoinNum;
            gameObject.GetComponent<FlyCoinAni>().RunAni();

            m_rewardCoinNum = 0;
            m_claimBtnObj.SetActive(false);
            m_coinTextObj.GetComponent<Text>().text = "0";

            m_continueBtnObj.SetActive(true);
            UpdateNoAdsBtn();
        }
    }

    public void OnNoAdsBtnClk()
    {
        NoAdsPanel.instance.OpenPanel();
    }

    public void OnContinueBtnClk()
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

    public void OnSkipLevelBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded_SkipLevel);
    }

    private void OnRewardVideoEnded_SkipLevel()
    {
        LevelData.SkipCurLevel();
        ClosePanel();
    }
}
