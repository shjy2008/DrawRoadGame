using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GMPanel : MonoBehaviour
{
    private GameObject m_bgObj;

    // Use this for initialization
    void Start()
    {
        m_bgObj = gameObject.transform.Find("Bg").gameObject;

        if (!Const_test.showGM)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGMBtnClk()
    {
        m_bgObj.SetActive(!m_bgObj.activeSelf);
    }

    public void OnAddCoinBtnClk()
    {
        PlayerData.ChangeCoin(1000);
    }

    public void OnChestBtnClk()
    {
        ChestPanel.instance.OpenPanel();
    }

    public void OnHighScoreBtnClk()
    {
        InputField field = m_bgObj.transform.Find("HighScoreBtn/InputField").GetComponent<InputField>();
        int score = int.Parse(field.text);
        GameCenterHelper.instance.ReportScore(score);
        PlayerData.SetBestScore(score);
    }

    public void OnRemoveAdsBtnClk()
    {
        PlayerData.BuyRemoveAds();
    }

    public void OnResetBtnClk()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnLevelBtnClk()
    {
        InputField field = m_bgObj.transform.Find("Level/InputField").GetComponent<InputField>();
        int level = int.Parse(field.text);
        LevelData.SetPassedLevel(level - 1);
        BattleScene.instance.ResetLevel();
        BattlePanel.instance.ResetPanel();
    }

    public void OnHideGmClk()
    {
        gameObject.SetActive(false);
    }

    // test
    public void OnTest1Clk()
    {
        GainNewRoadPanel.instance.OpenPanel();
        GainNewRoadPanel.instance.SetRoadId(1003);
        //MainPanel.instance.ClosePanel();
    }

    public void OnTest2Clk()
    {
        MainPanel.instance.OpenPanel();
    }
}
