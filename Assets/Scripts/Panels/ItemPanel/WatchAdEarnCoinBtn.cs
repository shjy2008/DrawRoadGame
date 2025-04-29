using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WatchAdEarnCoinBtn : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.transform.Find("Price").GetComponent<Text>().text = Table_constant.data["watch_ad_earn_coin"].param1.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnWatchAdBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded);
    }

    private void OnRewardVideoEnded()
    {
        PlayerData.ChangeCoin(Table_constant.data["watch_ad_earn_coin"].param1);
    }
}
