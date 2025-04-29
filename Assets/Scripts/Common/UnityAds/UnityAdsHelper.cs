//using System;
//using System.Collections;
//using DelegateTools;
//using UnityEngine;
//using UnityEngine.Advertisements;

//public class UnityAdsHelper : MonoBehaviour, IUnityAdsListener
//{
//    public static UnityAdsHelper instance;

//    // Unity Ads
//#if UNITY_IOS
//    private const string gameId = "3458944";
//#elif UNITY_ANDROID
//    private const string gameId = "3458945";
//#endif
//    private const bool testMode = true;
//    private const string interstitialPlacementId = "video";
//    private const string rewardedVideoPlacementId = "rewardedVideo";
//    private const string bannerPlacementId = "banner";

//    private VoidDelegate m_rewardedVideoEndedCb = null;

//    public UnityAdsHelper()
//    {
//        instance = this;
//    }

//    // Initialize the Ads listener and service:
//    void Start()
//    {
//        Event.AddEventListener(Event.Type.OnNoAdsBought, OnNoAdsBought);

//        Advertisement.AddListener(this);
//        Advertisement.Initialize(gameId, testMode);
//        if (!PlayerData.hasRemovedAds && !Const_test.noAds)
//        {
//            StartCoroutine(ShowBannerWhenReady());
//        }
//    }

//    IEnumerator ShowBannerWhenReady()
//    {
//        while (!Advertisement.IsReady(bannerPlacementId))
//        {
//            yield return new WaitForSeconds(0.5f);
//        }

//        if (!PlayerData.hasRemovedAds)
//        {
//            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
//            Advertisement.Banner.Show(bannerPlacementId);
//        }
//    }

//    private void OnNoAdsBought()
//    {
//        HideBanner();
//    }

//    public void HideBanner()
//    {
//        Advertisement.Banner.Hide();
//    }

//    public void TryShowInterstitial()
//    {
//        if (Const_test.noAds)
//        {
//            return;
//        }

//        if (PlayerData.hasRemovedAds)
//        {
//            return;
//        }

//        if (Advertisement.IsReady(interstitialPlacementId))
//        {
//            Advertisement.Show(interstitialPlacementId);
//        }
//    }

//    public void TryShowRewardedVideo(VoidDelegate rewardedVideoEndCb)
//    {
//        if (Const_test.noAds)
//        {
//            rewardedVideoEndCb();
//            return;
//        }

//        if (IsRewardedVideoReady())
//        {
//            Advertisement.Show(rewardedVideoPlacementId);
//            m_rewardedVideoEndedCb = rewardedVideoEndCb;
//        }
//        else
//        {
//            TextTipsPanel.instance.ShowText("Ads are not ready yet. Please wait for a while.");
//        }
//    }

//    // Check if the rewarded video is ready. If not ready yet, maybe the UI appearence will be different
//    public bool IsRewardedVideoReady()
//    {
//        return Advertisement.IsReady(rewardedVideoPlacementId);
//    }

//    public void OnUnityAdsReady(string placementId)
//    {

//    }

//    public void OnUnityAdsDidError(string message)
//    {
//        // Log the error.
//    }

//    public void OnUnityAdsDidStart(string placementId)
//    {
//        // Optional actions to take when the end-users triggers an ad.
//    }

//    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
//    {
//        // Define conditional logic for each ad completion status:
//        if (showResult == ShowResult.Finished)
//        {
//            // Reward the user for watching the ad to completion.
//            if (placementId == rewardedVideoPlacementId && m_rewardedVideoEndedCb != null)
//            {
//                m_rewardedVideoEndedCb();
//                m_rewardedVideoEndedCb = null;
//            }
//        }
//        else if (showResult == ShowResult.Skipped)
//        {
//            // Do not reward the user for skipping the ad.
//        }
//        else if (showResult == ShowResult.Failed)
//        {
//            Debug.LogWarning("The ad did not finish due to an error.");
//        }
//    }
//}
