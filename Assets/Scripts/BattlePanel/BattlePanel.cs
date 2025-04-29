using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    public static BattlePanel instance;

    // for level progress show
    private GameObject m_levelScrollBar;
    private GameObject m_levelCurText;
    private GameObject m_levelCurImage;
    private GameObject m_levelNextText;
    private GameObject m_levelNextImage;
    private GameObject m_levelTitleText;

    BattlePanel()
    {
        instance = this;
    }

    private new void Awake()
    {
        Event.AddEventListener(Event.Type.OnBattleStarted, OnBattleStarted);
        Event.AddEventListener_int(Event.Type.OnScoreUpdated, OnScoreUpdated);
        Event.AddEventListener(Event.Type.OnEndlessModeChanged, OnEndlessModeChanged);
        Event.AddEventListener(Event.Type.OnBestScoreChanged, OnBestScoreChanged);

        Transform normalModeTranform = gameObject.transform.Find("NormalMode");
        m_levelScrollBar = normalModeTranform.Find("Scrollbar").gameObject;
        m_levelCurText = normalModeTranform.Find("CurImage/Text").gameObject;
        m_levelCurImage = normalModeTranform.Find("CurImage/Image").gameObject;
        m_levelNextText = normalModeTranform.Find("NextImage/Text").gameObject;
        m_levelNextImage = normalModeTranform.Find("NextImage/Image").gameObject;
        m_levelTitleText = normalModeTranform.Find("Title").gameObject;

        OnEndlessModeChanged();
        OnBestScoreChanged();

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float percent = BattleScene.instance.GetCanvas().GetBattleLevel().GetPercent();
        m_levelScrollBar.GetComponent<Scrollbar>().size = percent;
    }

    public override void ResetPanel()
    {
        base.ResetPanel();

        int curLevel = LevelData.GetCurLevel();
        int nextLevel = LevelData.GetNextLevel();

        bool isCurBonusLevel = BattleScene.instance.GetCanvas().GetBattleLevel().IsBonusLevel();
        bool isNextBonusLevel = nextLevel % Const.bonusLevelGap == 1;

        // process cur level
        m_levelCurImage.SetActive(isCurBonusLevel);
        m_levelCurText.SetActive(!isCurBonusLevel);
        if (!isCurBonusLevel)
        {
            m_levelCurText.GetComponent<Text>().text = curLevel.ToString();
        }

        // process next level
        m_levelNextImage.SetActive(isNextBonusLevel);
        m_levelNextText.SetActive(!isNextBonusLevel);
        if (!isNextBonusLevel)
        {
            if (nextLevel != -1)
            {
                m_levelNextText.SetActive(true);
                if (isCurBonusLevel)
                {
                    nextLevel -= 1;
                }
                m_levelNextText.GetComponent<Text>().text = nextLevel.ToString();
            }
            else // pass all the levels
            {
                m_levelNextText.SetActive(false);
            }
        }

        // process title
        m_levelTitleText.SetActive(isCurBonusLevel);

        // endless mode text
        if (LevelData.isEndlessMode)
        {
            SetEndlessShowCurScore(false);
        }
    }

    private void OnBattleStarted()
    {
        if (LevelData.isEndlessMode)
        {
            SetEndlessShowCurScore(true);
        }
    }

    private void SetEndlessShowCurScore(bool isShow)
    {
        if (isShow)
        {
            gameObject.transform.Find("EndlessMode/ScoreText").gameObject.SetActive(true);
            gameObject.transform.Find("EndlessMode/BestScoreText").localPosition = gameObject.transform.Find("EndlessMode/BestScoreNormalPos").localPosition;
            gameObject.transform.Find("EndlessMode/BestScoreText").localScale = new Vector2(1, 1);
        }
        else
        {
            gameObject.transform.Find("EndlessMode/ScoreText").gameObject.SetActive(false);
            gameObject.transform.Find("EndlessMode/BestScoreText").localPosition = gameObject.transform.Find("EndlessMode/BestScoreCenterPos").localPosition;
            gameObject.transform.Find("EndlessMode/BestScoreText").localScale = new Vector2(2, 2);
        }
    }

    private void OnScoreUpdated(int score)
    {
        GameObject scoreText = gameObject.transform.Find("EndlessMode/ScoreText").gameObject;
        Text text = scoreText.GetComponent<Text>();
        text.text = string.Format("Score: {0}", score);
    }

    private void OnEndlessModeChanged()
    {
        bool isEndlessMode = LevelData.isEndlessMode;
        gameObject.transform.Find("NormalMode").gameObject.SetActive(!isEndlessMode);
        gameObject.transform.Find("EndlessMode").gameObject.SetActive(isEndlessMode);
        if (isEndlessMode)
        {
            SetEndlessShowCurScore(false);
        }
    }

    private void OnBestScoreChanged()
    {
        int bestScore = PlayerData.bestScore;
        gameObject.transform.Find("EndlessMode/BestScoreText").GetComponent<Text>().text = string.Format("BEST: {0}", bestScore);
    }

    public void OnPauseBtnClk()
    {
        PausePanel.instance.OpenPanel();
    }
}
