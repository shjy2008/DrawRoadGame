using System;
using UnityEngine;

// Some constant value that will never be changed.
// If it would be changed frequently, add to the Constant Table
public class Const
{
    // general
    public const int normalFrameRate = 30;
    public const int highFrameRate = 60;
    public const int frameRate = highFrameRate;
    public const float designWidth = 750;
    public const float designHeight = 1334;

    public static float gameWidth;
    public static float gameHeight;
    public static float leftX;// = -designWidth / 2 / 100;
    public static float rightX;// = designWidth / 2 / 100;
    public static float topY;// = Screen.height / 2 / 100; //designHeight / 2 / 100;
    public static float bottomY;// = -Screen.height / 2 / 100; //-designHeight / 2 / 100;

    public static int showAdsWinCount = 2;
    public static int showAdsLoseCount = 5;

    // battle
    public const int INVALID_KEY = -1;
    public const int endless_levelId = -1;
    public const int bonus_levelId = -2;

    public const int bonusLevelGap = 5; // Enter a bonus level every time when how many levels have been passed
    public const float ballPosX = -2.0f;
    public const float blocksGap = 2.0f;
    public const float canReviveTime = 5.0f; // After how long can choose to revive
    public const float endlessMaxSpeedScale = 3.0f;

    public const float blockRandomYMax = 2.0f;
    public const float blockRandomYMin = -2.0f;
    public const float blockChangeToRandom = -10.0f; // If less than this, change to random
    public const float roadWidth = 1.2f;

    public static Color blockColor;

    // chest key
    public const int maxChestKey = 3; // can have maximun 3 keys

    public class BallBuyType
    {
        public static int Coin = 0;
        public static int ExpensiveCoin = 1;
        public static int Video = 2;
        public static int Chest = 3;
    }

    public class AchieveType
    {
        public static int Level = 0;
        public static int Endless_score = 1;
        public static int Login = 2;
    }

    public class GeneratorAppearType
    {
        public static int Block = 1;
        public static int BlockCoin = 2;
        public static int RelaxCoin = 3;
        public static int BonusCoin = 4;
    }
}
