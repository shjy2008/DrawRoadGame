using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelMode
{
    public static string Normal = "normal";
    public static string Medium = "medium";
    public static string Hard = "hard";
}

public class LevelData
{
    public static LevelData instance = new LevelData();

    public static Dictionary<string, int> passedLevelDict;
    public static string curMode; // maybe useless
    public static bool canPlayBonusLevel;
    // for ads
    public static int continuousLoseCount;
    // endless
    public static bool isEndlessMode;

    public LevelData()
    {
        // passedLevelDict
        string s = PlayerPrefs.GetString("passedLevel", "");
        if (s == "")
        {
            Dictionary<string, int> defaultDict = new Dictionary<string, int> {
                { LevelMode.Normal, 0 },
                { LevelMode.Medium, 0 },
                { LevelMode.Hard, 0 },
            };
            s = JsonConvert.SerializeObject(defaultDict);
        }
        passedLevelDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(s);

        string defaultMode = LevelMode.Normal;
        curMode = PlayerPrefs.GetString("curMode", defaultMode);

        canPlayBonusLevel = false;

        continuousLoseCount = 0;

        isEndlessMode = PlayerPrefs.GetInt("isEndlessMode", 0) != 0;
    }

    public static void SetPassedLevel(string mode, int level)
    {
        passedLevelDict[mode] = level;
        PlayerPrefs.SetString("passedLevel", JsonConvert.SerializeObject(passedLevelDict));
        Event.PostEvent(Event.Type.OnLevelPassed);
    }

    public static void SetPassedLevel(int level)
    {
        SetPassedLevel(curMode, level);

        if (level % Const.bonusLevelGap == 0)
        {
            canPlayBonusLevel = true;
        }
    }

    public static void SetCurMode(string mode)
    {
        curMode = mode;
        PlayerPrefs.SetString("curMode", curMode);
    }

    public static int GetCurLevel(string mode)
    {
        int maxLevel = Table_constant.data["normal_level_total_count"].param1;
        if (passedLevelDict[mode] < maxLevel)
        {
            return passedLevelDict[mode] + 1;
        }
        return maxLevel;
    }

    public static int GetCurLevel()
    {
        return GetCurLevel(curMode);
    }

    // ret -1: passed all the levels, no next level
    public static int GetNextLevel(string mode)
    {
        int maxLevel = Table_constant.data["normal_level_total_count"].param1;
        if (passedLevelDict[mode] < maxLevel - 1)
        {
            return passedLevelDict[mode] + 2;
        }
        return -1;
    }

    public static int GetNextLevel()
    {
        return GetNextLevel(curMode);
    }

    public static void SkipCurLevel()
    {
        SetPassedLevel(GetCurLevel());
        continuousLoseCount = 0;

        BattleScene.instance.ResetLevel();
        BattlePanel.instance.ResetPanel();
        MainPanel.instance.UpdateSkipLevelBtn();
        LosePanel.instance.UpdateSkipLevelBtn();

        Event.PostEvent(Event.Type.OnLevelPassed);
    }

    // endless mode
    public static void SetIsEndlessMode(bool flag)
    {
        if (isEndlessMode != flag)
        {
            isEndlessMode = flag;
            PlayerPrefs.SetInt("isEndlessMode", isEndlessMode ? 1 : 0);
            Event.PostEvent(Event.Type.OnEndlessModeChanged);
        }
    }

}
