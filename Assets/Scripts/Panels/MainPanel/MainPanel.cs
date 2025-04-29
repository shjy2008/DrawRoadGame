using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public static MainPanel instance;

    private bool m_isStartedBattle;

    private bool m_isTouching = false;
    private Vector2 m_beganPos;

    MainPanel()
    {
        instance = this;
    }

    private new void Awake()
    {
        Event.AddEventListener(Event.Type.OnLevelPassed, OnLevelPassed);
        Event.AddEventListener(Event.Type.OnBestScoreChanged, OnBestScoreChanged);
        Event.AddEventListener(Event.Type.OnNoAdsBought, OnNoAdsBought);

        m_isStartedBattle = false;
        UpdateRemoveAds();
        SetIsEndlessMode(LevelData.isEndlessMode);
        UpdateNormalLevelBtn();
        UpdateEndlessLevelBtn();
        UpdateSkipLevelBtn();
        UpdateShopRedTips();

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Swipe to start
        if (InputMgr.IsBegan() && !InputMgr.IsClickedUI())
        {
            m_isTouching = true;
            m_beganPos = InputMgr.GetTouchPos();
        }
        else if (m_isTouching && InputMgr.IsMoved())
        {
            Vector2 touchPos = InputMgr.GetTouchPos();
            if (Mathf.Abs(touchPos.x - m_beganPos.x) > 50.0f ||
                Mathf.Abs(touchPos.y - m_beganPos.y) > 50.0f)
            {
                if (!m_isStartedBattle)
                {
                    _StartBattle();
                }
            }
        }
        else if (InputMgr.IsEnded())
        {
            m_isTouching = false;
        }
    }

    public void OnLevelBtnClick()
    {
        if (LevelPanel.LevelPanel.instance.IsOpened() == false)
        {
            LevelPanel.LevelPanel.instance.OpenPanel();
        }
        else
        {
            LevelPanel.LevelPanel.instance.ClosePanel();
        }
	}

    public void OnSkipLevelBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded_SkipLevel);
    }

    private void OnRewardVideoEnded_SkipLevel()
    {
        LevelData.SkipCurLevel();
    }

    private void _StartBattle()
    {
        m_isStartedBattle = true;
        ClosePanel();
        BattleScene.instance.StartBattle();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        gameObject.transform.Find("Finger").gameObject.SetActive(false);
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        gameObject.transform.Find("Finger").gameObject.SetActive(true);
    }

    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();
        UpdateSkipLevelBtn();
        UpdateShopRedTips();
    }

    public void BackToMainPanel()
    {
        OpenPanel();

        m_isStartedBattle = false;

        Event.PostEvent(Event.Type.OnBackToMainPanel);
    }

    public void OnSettingBtnClick()
    {
        if (SettingPanel.instance.IsOpened() == false)
        {
            SettingPanel.instance.OpenPanel();
        }
        else
        {
            SettingPanel.instance.ClosePanel();
        }
    }

    public void OnBallShopPanelClick()
    {
        ItemPanel.instance.OpenPanel();
    }

    public void OnNoAdsBtnClick()
    {
        NoAdsPanel.instance.OpenPanel();
    }

    public void OnGameCenterBtnClick()
    {
        // The button size should be reset
        IEnumerator _()
        {
            yield return new WaitForSeconds(0.2f);
            GameCenterHelper.instance.ShowLeaderboard();
        }
        StartCoroutine(_());
    }

    public void UpdateRemoveAds()
    {
        if (PlayerData.hasRemovedAds || Const_test.noAds)
        {
            gameObject.transform.Find("NoAdsBtn").gameObject.SetActive(false);
        }
    }

    public void OnNormalBtnClick()
    {
        SetIsEndlessMode(false);
    }

    public void OnEndlessBtnClick()
    {
        SetIsEndlessMode(true);
        if (!PlayerData.hasClickedEndless)
        {
            PlayerData.SetHasClickedEndless(true);
            UpdateEndlessRedTips();
        }
    }

    private void UpdateEndlessRedTips()
    {
        int unlockLevel = Table_constant.data["endless_unlock_level"].param1;
        bool isEndlessUnlocked = LevelData.GetCurLevel() >= unlockLevel;
        bool showTips = isEndlessUnlocked && !PlayerData.hasClickedEndless;
        gameObject.transform.Find("EndlessBtnParent/EndlessBtn/RedTips").gameObject.SetActive(showTips);
    }

    private void UpdateShopRedTips()
    {
        bool showTips = false;
        if (PlayerData.coin >= Table_constant.data["normal_ball_cost"].param1 &&
            PlayerData.GetNotHaveBallIdList(Const.BallBuyType.Coin).Count > 0)
        {
            showTips = true;
        }
        if (PlayerData.coin >= Table_constant.data["advanced_ball_cost"].param1 &&
            PlayerData.GetNotHaveBallIdList(Const.BallBuyType.ExpensiveCoin).Count > 0)
        {
            showTips = true;
        }
        gameObject.transform.Find("BallShopBtn/RedTips").gameObject.SetActive(showTips);
    }

    private void SetIsEndlessMode(bool flag)
    {
        LevelData.SetIsEndlessMode(flag);

        Vector2 selectedScale = new Vector2(1, 1);
        Vector2 unselectedScale = new Vector2(0.85f, 0.85f);
        gameObject.transform.Find("NormalBtnParent").localScale = (flag == false ? selectedScale : unselectedScale);
        gameObject.transform.Find("EndlessBtnParent").localScale = (flag == true ? selectedScale : unselectedScale);
    }

    private void UpdateNormalLevelBtn()
    {
        gameObject.transform.Find("NormalBtnParent/NormalBtn/Desc").GetComponent<Text>().text = "LEVEL: " + LevelData.GetCurLevel();
    }

    private void UpdateEndlessLevelBtn()
    {
        Transform endlessBtnTranform = gameObject.transform.Find("EndlessBtnParent/EndlessBtn");
        Text endlessDesc = endlessBtnTranform.Find("Desc").GetComponent<Text>();
        int unlockLevel = Table_constant.data["endless_unlock_level"].param1;
        bool isEndlessUnlocked = LevelData.GetCurLevel() >= unlockLevel;
        if (!isEndlessUnlocked)
        {
            endlessDesc.text = "Unlock at\nlevel " + unlockLevel;
        }
        else
        {
            endlessDesc.text = "BEST: " + PlayerData.bestScore;
        }
        endlessBtnTranform.GetComponent<Button>().enabled = isEndlessUnlocked;
        endlessBtnTranform.Find("UnlockMask").gameObject.SetActive(!isEndlessUnlocked);
        UpdateEndlessRedTips();
    }

    public void UpdateSkipLevelBtn()
    {
        bool showSkipBtn = LevelData.continuousLoseCount >= 3;
        gameObject.transform.Find("SkipLevelBtn").gameObject.SetActive(showSkipBtn);
    }

    private void OnLevelPassed()
    {
        UpdateNormalLevelBtn();
        UpdateEndlessLevelBtn();
    }

    private void OnBestScoreChanged()
    {
        UpdateEndlessLevelBtn();
    }

    private void OnNoAdsBought()
    {
        UpdateRemoveAds();
    }
}
