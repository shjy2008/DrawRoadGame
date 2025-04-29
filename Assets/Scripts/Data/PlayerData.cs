using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerData
{
    public static PlayerData instance = new PlayerData();

    public static int bestScore;
    public static int coin;
    // ball
    public static List<int> ballList;
    public static int curBallId;
    // road
    public static List<int> roadList;
    public static int curRoadId;
    // chest
    public static int chestKeyNum;
    // settings
    public static bool enableVibration;
    public static bool enableSound;
    // remove ads
    public static bool hasRemovedAds;
    // login
    public static int loginDays;
    public static string prevLoginTime_str;
    // endless
    public static bool hasClickedEndless;

    public PlayerData()
    {
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
        coin = PlayerPrefs.GetInt("coin", 0);

        InitBallData();
        InitRoadData();

        chestKeyNum = PlayerPrefs.GetInt("chestKeyNum", 0);

        enableVibration = PlayerPrefs.GetInt("enableVibration", 1) != 0;

        enableSound = PlayerPrefs.GetInt("enableSound", 1) != 0;

        hasRemovedAds = PlayerPrefs.GetInt("hasRemovedAds", 0) != 0;

        loginDays = PlayerPrefs.GetInt("loginDays", 0);
        prevLoginTime_str = PlayerPrefs.GetString("prevLoginTime_str", "");

        DateTime nowDateTime = DateTime.Now;
        TryAddLoginDay(prevLoginTime_str, nowDateTime);
        PlayerPrefs.SetString("prevLoginTime_str", nowDateTime.ToString());

        hasClickedEndless = PlayerPrefs.GetInt("hasClickedEndless", 0) != 0;
    }

    private void InitBallData()
    {
        // ballList
        string ballList_str = PlayerPrefs.GetString("ballList", "");
        if (ballList_str == "")
        {
            ballList = new List<int>();
        }
        else
        {
            ballList = JsonConvert.DeserializeObject<List<int>>(ballList_str);
        }
        int defaultBallId = Table_constant.data["default_ball_id"].param1;
        if (!ballList.Contains(defaultBallId))
        {
            GainBallId(defaultBallId);
        }

        // ballId
        curBallId = PlayerPrefs.GetInt("curBallId", 0);
        if (curBallId == 0)
        {
            SetCurBallId(defaultBallId);
        }
    }

    private void InitRoadData()
    {
        // roadList
        string roadList_str = PlayerPrefs.GetString("roadList", "");
        if (roadList_str == "")
        {
            roadList = new List<int>();
        }
        else
        {
            roadList = JsonConvert.DeserializeObject<List<int>>(roadList_str);
        }
        int defaultRoadId = Table_constant.data["default_road_id"].param1;
        if (!roadList.Contains(defaultRoadId))
        {
            GainRoadId(defaultRoadId);
        }

        // roadId
        curRoadId = PlayerPrefs.GetInt("curRoadId", 0);
        if (curRoadId == 0)
        {
            SetCurRoadId(defaultRoadId);
        }
    }

    // score
    public static void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("bestScore", score);
            Event.PostEvent(Event.Type.OnBestScoreChanged);
        }
    }

    // coin
    public static void ChangeCoin(int diff)
    {
        coin += diff;
        PlayerPrefs.SetInt("coin", coin);
        Event.PostEvent(Event.Type.OnCoinUpdated);
    }

    // ball
    public static void GainBallId(int ballId)
    {
        ballList.Add(ballId);
        PlayerPrefs.SetString("ballList", JsonConvert.SerializeObject(ballList));

        // Set as current ball automatically
        SetCurBallId(ballId);
    }

    public static void SetCurBallId(int ballId)
    {
        if (curBallId != ballId)
        {
            curBallId = ballId;
            PlayerPrefs.SetInt("curBallId", curBallId);
            Event.PostEvent(Event.Type.OnCurBallChanged);
        }
    }

    public static List<Table_shop_ball.Data> GetBallDatasWithType(int type)
    {
        List<Table_shop_ball.Data> ret = new List<Table_shop_ball.Data>();
        foreach (Table_shop_ball.Data data in Table_shop_ball.data.Values)
        {
            if (data.type == type)
            {
                ret.Add(data);
            }
        }
        return ret;
    }

    public static List<Table_shop_ball.Data> GetNotHaveBallIdList(int type)
    {
        List<Table_shop_ball.Data> notHaveList = new List<Table_shop_ball.Data>();
        List<Table_shop_ball.Data> typeNormalIds = PlayerData.GetBallDatasWithType(type);
        foreach (Table_shop_ball.Data data in typeNormalIds)
        {
            if (!PlayerData.ballList.Contains(data.key))
            {
                notHaveList.Add(data);
            }
        }
        return notHaveList;
    }


    // road
    public static void GainRoadId(int roadId)
    {
        roadList.Add(roadId);
        PlayerPrefs.SetString("roadList", JsonConvert.SerializeObject(roadList));

        // Set as current road automatically
        //SetCurRoadId(roadId);

        Event.PostEvent(Event.Type.OnNewRoadUnlocked, roadId);
    }

    public static void SetCurRoadId(int roadId)
    {
        if (curRoadId != roadId)
        {
            curRoadId = roadId;
            PlayerPrefs.SetInt("curRoadId", curRoadId);
            Event.PostEvent(Event.Type.OnCurRoadChanged);
        }
    }

    public static List<Table_shop_road.Data> GetRoadDatasWithAchieveType(int achieveType)
    {
        List<Table_shop_road.Data> ret = new List<Table_shop_road.Data>();
        foreach (Table_shop_road.Data data in Table_shop_road.data.Values)
        {
            if (data.achieveType == achieveType)
            {
                ret.Add(data);
            }
        }
        return ret;
    }

    public static void TryGainNewRoad_LevelPassed()
    {
        List<Table_shop_road.Data> dataList = PlayerData.GetRoadDatasWithAchieveType(Const.AchieveType.Level);
        foreach (Table_shop_road.Data data in dataList)
        {
            if (LevelData.GetCurLevel() > data.param)
            {
                if (!PlayerData.roadList.Contains(data.key))
                {
                    PlayerData.GainRoadId(data.key);

                    GainNewRoadPanel.instance.OpenPanel();
                    GainNewRoadPanel.instance.SetRoadId(data.key);
                }
            }
        }
    }

    public static void TryGainNewRoad_EndlessScore()
    {
        List<Table_shop_road.Data> dataList = PlayerData.GetRoadDatasWithAchieveType(Const.AchieveType.Endless_score);
        int gainRoadId = -1;
        foreach (Table_shop_road.Data data in dataList)
        {
            if (PlayerData.bestScore >= data.param)
            {
                if (!PlayerData.roadList.Contains(data.key))
                {
                    PlayerData.GainRoadId(data.key);
                    gainRoadId = data.key;
                }
            }
        }

        if (gainRoadId != -1)
        {
            GainNewRoadPanel.instance.OpenPanel();
            GainNewRoadPanel.instance.SetRoadId(gainRoadId);
        }
    }

    public static void TryGainNewRoad_Login()
    {
        List<Table_shop_road.Data> dataList = PlayerData.GetRoadDatasWithAchieveType(Const.AchieveType.Login);
        foreach (Table_shop_road.Data data in dataList)
        {
            if (loginDays >= data.param)
            {
                if (!PlayerData.roadList.Contains(data.key))
                {
                    PlayerData.GainRoadId(data.key);

                    GainNewRoadPanel.instance.OpenPanel();
                    GainNewRoadPanel.instance.SetRoadId(data.key);
                }
            }
        }
    }

    // chest key
    public static void GainChestKey()
    {
        chestKeyNum += 1;
        chestKeyNum = Mathf.Min(chestKeyNum, Const.maxChestKey); // can have maximun 3 keys
        PlayerPrefs.SetInt("chestKeyNum", chestKeyNum);
        Event.PostEvent(Event.Type.OnGainChestKey);
    }

    public static void UseAllChestKey()
    {
        chestKeyNum = 0;
        PlayerPrefs.SetInt("chestKeyNum", chestKeyNum);
        Event.PostEvent(Event.Type.OnUseAllChestKey);
    }

    // vibration
    public static void SetVibrationEnabled(bool enabled)
    {
        enableVibration = enabled;
        PlayerPrefs.SetInt("enableVibration", enableVibration ? 1 : 0);
        if (enableVibration)
        {
            Handheld.Vibrate();
        }
    }

    // sound
    public static void SetSoundEnabled(bool enabled)
    {
        enableSound = enabled;
        PlayerPrefs.SetInt("enableSound", enableSound ? 1 : 0);
    }

    // remove ads
    public static void BuyRemoveAds()
    {
        hasRemovedAds = true;
        PlayerPrefs.SetInt("hasRemovedAds", 1);
        Event.PostEvent(Event.Type.OnNoAdsBought);
    }

    // login days
    public static void TryAddLoginDay(string prevLoginTime_str, DateTime nowDateTime)
    {
        if (prevLoginTime_str == "")
        {
            AddLoginDay();
        }
        else
        {
            DateTime prevDateTime = Convert.ToDateTime(prevLoginTime_str);
            if (nowDateTime.Year != prevDateTime.Year || nowDateTime.Month != prevDateTime.Month || nowDateTime.Day != prevDateTime.Day)
            {
                AddLoginDay();
            }
        }
    }

    private static void AddLoginDay()
    {
        loginDays += 1;
        PlayerPrefs.SetInt("loginDays", loginDays);

        // Gain new road skin
        TryGainNewRoad_Login();
    }

    // endless
    public static void SetHasClickedEndless(bool flag)
    {
        hasClickedEndless = flag;
        PlayerPrefs.SetInt("hasClickedEndless", hasClickedEndless ? 1 : 0);
    }

}
