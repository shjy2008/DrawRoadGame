using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GameCenterHelper : MonoBehaviour
{
    public static GameCenterHelper instance;

    private const string m_leaderBoardId = "grp.endless_mode_score";

    public GameCenterHelper()
    {
        instance = this;
    }

    public void Start()
    {
#if UNITY_IOS
        Social.localUser.Authenticate(HandleAuthenticated);
#endif
    }

    private void HandleAuthenticated(bool success)
    {
        Debug.Log("GameCenterManager *** HandleAuthenticated: success = " + success);
        if (success)
        {
            //下面三行看个人需要，需要什么信息就取什么信息，这里注释掉是因为担心有的朋友没有在iTunesConnect里设置排行、成就之类的东西，运行起来可能会报错  
            //Social.localUser.LoadFriends(HandleFriendsLoaded);  
            //Social.LoadAchievements(HandleAchievementsLoaded);  
            //Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);  
        }
    }

    //private void HandleFriendsLoaded(bool success)
    //{
    //    Debug.Log("GameCenterManager *** HandleFriendsLoaded: success = " + success);
    //    foreach (IUserProfile friend in Social.localUser.friends)
    //    {
    //        Debug.Log("GameCenterManager * friend = " + friend.ToString());
    //    }
    //}

    //private void HandleAchievementsLoaded(IAchievement[] achievements)
    //{
    //    Debug.Log("GameCenterManager * HandleAchievementsLoaded");
    //    foreach (IAchievement achievement in achievements)
    //    {
    //        Debug.Log("GameCenterManager * achievement = " + achievement.ToString());
    //    }
    //}

    //private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
    //{
    //    Debug.Log("GameCenterManager *** HandleAchievementDescriptionsLoaded");
    //    foreach (IAchievementDescription achievementDescription in achievementDescriptions)
    //    {
    //        Debug.Log("GameCenterManager * achievementDescription = " + achievementDescription.ToString());
    //    }
    //}

    // achievements  

    public void ReportProgress(string achievementId, double progress)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementId, progress, HandleProgressReported);
        }
    }

    private void HandleProgressReported(bool success)
    {
        Debug.Log("GameCenterManager *** HandleProgressReported: success = " + success);
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    // leaderboard  

    public void ReportScore(long score)
    {
#if UNITY_IOS
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, m_leaderBoardId, HandleScoreReported);
        }
#endif
    }

    public void HandleScoreReported(bool success)
    {
        Debug.Log("GameCenterManager *** HandleScoreReported: success = " + success);
    }

    public void ShowLeaderboard()
    {
#if UNITY_IOS
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
#endif
    }
}