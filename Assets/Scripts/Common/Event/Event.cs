using System;
using System.Collections.Generic;
using DelegateTools;

public class Event
{
    public struct Type
    {
        // battle
        public static string OnBattleStarted = "OnBattleStarted";
        public static string OnEndlessModeChanged = "OnEndlessModeChanged";
        public static string OnLevelPassed = "OnLevelPassed";
        public static string OnBestScoreChanged = "OnBestScoreChanged";
        // UI
        public static string OnBackToMainPanel = "OnBackToMainPanel";
        // score
        public static string OnScoreUpdated = "OnScoreUpdated";
        // coin
        public static string OnCoinUpdated = "OnCoinUpdated";
        // chest key
        public static string OnGainChestKey = "OnGainChestKey";
        public static string OnUseAllChestKey = "OnUseAllChestKey";
        // shop
        public static string OnBallItemClk = "OnBallItemClk";
        public static string OnRoadItemClk = "OnRoadItemClk";
        public static string OnCurBallChanged = "OnCurBallChanged";
        public static string OnCurRoadChanged = "OnCurRoadChanged";
        public static string OnNewRoadUnlocked = "OnNewRoadUnlocked";
        // ads
        public static string OnNoAdsBought = "OnNoAdsBought";
    };

    private static Dictionary<string, List<VoidDelegate>> m_dict = new Dictionary<string, List<VoidDelegate>>();
    private static Dictionary<string, List<IntDelegate>> m_dict_int = new Dictionary<string, List<IntDelegate>>();
    private static Dictionary<string, List<StringDelegate>> m_dict_string = new Dictionary<string, List<StringDelegate>>();
    private static Dictionary<string, List<IntStringDelegate>> m_dict_int_string = new Dictionary<string, List<IntStringDelegate>>();

    // No arg
    public static void AddEventListener(string key, VoidDelegate func)
    {
        if (m_dict.TryGetValue(key, out List<VoidDelegate> value))
        {
            value.Add(func);
        }
        else
        {
            value = new List<VoidDelegate>();
            value.Add(func);
            m_dict.Add(key, value);
        }
    }

    public static void RemoveEventListener(string key, VoidDelegate func)
    {
        if (m_dict.TryGetValue(key, out List<VoidDelegate> value))
        {
            value.Remove(func);
        }
    }

    // Arg: int
    public static void AddEventListener_int(string key, IntDelegate func)
    {
        if (m_dict_int.TryGetValue(key, out List<IntDelegate> value))
        {
            value.Add(func);
        }
        else
        {
            value = new List<IntDelegate>();
            value.Add(func);
            m_dict_int.Add(key, value);
        }
    }

    public static void RemoveEventListener_int(string key, IntDelegate func)
    {
        if (m_dict_int.TryGetValue(key, out List<IntDelegate> value))
        {
            value.Remove(func);
        }
    }

    // Arg: string
    public static void AddEventListener_string(string key, StringDelegate func)
    {
        if (m_dict_string.TryGetValue(key, out List<StringDelegate> value))
        {
            value.Add(func);
        }
        else
        {
            value = new List<StringDelegate>();
            value.Add(func);
            m_dict_string.Add(key, value);
        }
    }

    public static void RemoveEventListener_string(string key, StringDelegate func)
    {
        if (m_dict_string.TryGetValue(key, out List<StringDelegate> value))
        {
            value.Remove(func);
        }
    }

    // Arg: int, string
    public static void AddEventListener_int_string(string key, IntStringDelegate func)
    {
        if (m_dict_int_string.TryGetValue(key, out List<IntStringDelegate> value))
        {
            value.Add(func);
        }
        else
        {
            value = new List<IntStringDelegate>();
            value.Add(func);
            m_dict_int_string.Add(key, value);
        }
    }

    public static void RemoveEventListener_int_string(string key, IntStringDelegate func)
    {
        if (m_dict_int_string.TryGetValue(key, out List<IntStringDelegate> value))
        {
            value.Remove(func);
        }
    }

    public static void PostEvent(string key)
    {
        if (m_dict.TryGetValue(key, out List<VoidDelegate> value))
        {
            foreach (var func in value)
            {
                func();
            }
        }
    }

    public static void PostEvent(string key, int param1)
    {
        if (m_dict_int.TryGetValue(key, out List<IntDelegate> value))
        {
            foreach (var func in value)
            {
                func(param1);
            }
        }
    }

    public static void PostEvent(string key, string param1)
    {
        if (m_dict_string.TryGetValue(key, out List<StringDelegate> value))
        {
            foreach (var func in value)
            {
                func(param1);
            }
        }
    }

    public static void PostEvent(string key, int param1, string param2)
    {
        if (m_dict_int_string.TryGetValue(key, out List<IntStringDelegate> value))
        {
            foreach (var func in value)
            {
                func(param1, param2);
            }
        }
    }
}
