using System;
public class InterstitialHelper
{
    public static InterstitialHelper instance = new InterstitialHelper();

    private int m_winCount = 0;
    private int m_loseCount = 0;

    public InterstitialHelper()
    {

    }

    // ret: should show ad
    public void AddWinCount()
    {
        m_winCount += 1;
    }

    public void AddLoseCount()
    {
        m_loseCount += 1;
    }

    public bool CheckShouldShowAd()
    {
        if (PlayerData.hasRemovedAds)
        {
            return false;
        }

        if (m_winCount >= Const.showAdsWinCount || m_loseCount >= Const.showAdsLoseCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetCount()
    {
        m_winCount = 0;
        m_loseCount = 0;
    }
}
