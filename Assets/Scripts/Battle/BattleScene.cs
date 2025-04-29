using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    public static BattleScene instance;

    private bool m_battleStarted;
    private bool m_battleEnded;
    private bool m_isWin;
    private bool m_hasRevived;
    private float m_battleStartTime;
    private BattleCanvas m_canvas;
    private BattleCanvas m_backupCanvas;
    private BattleCloudManager m_cloudMgr;

    BattleScene()
    {
        instance = this;
    }

    private void Awake()
    {
        m_battleStarted = false;
        m_battleEnded = false;
        m_isWin = false;
        m_hasRevived = false;
        m_battleStartTime = 0.0f;
        m_canvas = null;
        m_backupCanvas = null;
        m_cloudMgr = null;

        // Calc game width & height
        float screenRatio = (float)Screen.height / Screen.width;
        //float designRatio = (float)Const.designHeight / Const.designWidth;
        //if (screenRatio > designRatio) // Screen is too thin
        //{
        //    Const.gameWidth = Const.designWidth;
        //    Const.gameHeight = Const.designWidth * screenRatio;
        //}
        //else // Screen is too fat
        {
            Const.gameHeight = Const.designHeight;
            Const.gameWidth = Const.designHeight / screenRatio;
        }
        Const.rightX = Const.gameWidth / 2 / 100 + 3;
        Const.leftX = -Const.rightX - 5;
        Const.topY = Const.gameHeight / 2 / 100 + 5;
        Const.bottomY = -Const.topY;

        Application.targetFrameRate = Const.frameRate;

        Event.AddEventListener(Event.Type.OnBackToMainPanel, OnBackToMainPanel);
        Event.AddEventListener(Event.Type.OnEndlessModeChanged, OnEndlessModeChanged);

        ResetLevel();
        BattlePanel.instance.ResetPanel();

        // bg
        GameObject bgObj = gameObject.transform.Find("Bg").gameObject;
        Vector2 bgSize = bgObj.GetComponent<SpriteRenderer>().size;
        float scale = 1.0f;
        float gameRatio = Const.gameHeight / Const.gameWidth;
        float bgRatio = bgSize.y / bgSize.x;
        if (gameRatio > bgRatio) // Screen is too thin
        {
            scale = Const.gameHeight / bgSize.y / 100.0f;
        }
        else // Screen is too fat
        {
            scale = Const.gameWidth / bgSize.x / 100.0f;
        }
        bgObj.transform.localScale = new Vector2(scale, scale);

        // cloud
        m_cloudMgr = new BattleCloudManager();

        //gameObject.transform.Find("Sphere").GetComponent<MeshRenderer>().material.color = Color.yellow;

        // TODO
        Const.blockColor = new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255));

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_canvas != null)
        {
            m_canvas.Update();
        }

        if (m_cloudMgr != null)
        {
            m_cloudMgr.Update();
        }

        //Debug.Log(1.0f / Time.deltaTime);
    }

    private void OnBackToMainPanel()
    {
        ResetLevel();
    }

    private void OnEndlessModeChanged()
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        m_battleStarted = false;
        m_battleEnded = false;
        m_isWin = false;
        m_hasRevived = false;

        if (m_canvas != null)
        {
            m_canvas.Destroy();
            m_canvas = null;
        }

        int levelId;
        bool isEndlessMode = LevelData.isEndlessMode;
        if (isEndlessMode)
        {
            levelId = Const.endless_levelId;
        }
        else
        {
            if (LevelData.canPlayBonusLevel)
            {
                levelId = Const.bonus_levelId;
                LevelData.canPlayBonusLevel = false;
            }
            else
            {
                levelId = LevelData.GetCurLevel();
            }
        }
        m_canvas = new BattleCanvas(levelId);
    }

    public void StartBattle()
    {
        m_battleStarted = true;
        m_battleStartTime = Time.time;
        Event.PostEvent(Event.Type.OnBattleStarted);
    }

    public void SetBattleEnded(bool isWin)
    {
        m_battleEnded = true;
        if (LevelData.isEndlessMode)
        {
            IEnumerator _()
            {
                yield return new WaitForSeconds(0.2f);
                if (CanRevive())
                {
                    RevivePanel.instance.OpenPanel();
                    m_hasRevived = true;
                }
                else
                {
                    EndlessEndPanel.instance.SetScore(m_canvas.GetScore());
                    EndlessEndPanel.instance.OpenPanel();
                    EndlessEndPanel.instance.SetRewardCoinNum(m_canvas.GetCoin());
                }
            }
            StartCoroutine(_());
        }
        else
        {
            if (isWin)
            {
                Invoke("OpenNormalWinPanel", 0.2f);
                m_isWin = true;
            }
            else
            {
                IEnumerator _()
                {
                    yield return new WaitForSeconds(0.2f);
                    if (CanRevive())
                    {
                        RevivePanel.instance.OpenPanel();
                        m_hasRevived = true;
                    }
                    else
                    {
                        LosePanel.instance.OpenPanel();
                        LosePanel.instance.SetRewardCoinNum(BattleScene.instance.GetCanvas().GetCoin());
                    }
                }
                StartCoroutine(_());
            }
        }
        m_canvas.GetBattleLevel().OnBattleEnded(isWin);
    }

    private void OpenNormalWinPanel()
    {
        WinPanel.instance.OpenPanel();
        WinPanel.instance.SetRewardCoinNum(BattleScene.instance.GetCanvas().GetCoin());
        if (PlayerData.chestKeyNum >= Const.maxChestKey)
        {
            IEnumerator _()
            {
                yield return new WaitForSeconds(0.5f);
                ChestPanel.instance.OpenPanel();
            }
            StartCoroutine(_());
        }
    }

    public bool IsBattleEnded()
    {
        return m_battleEnded;
    }

    public bool IsBattleStarted()
    {
        return m_battleStarted;
    }

    public bool IsBlocksMoving()
    {
        return IsWin() || (m_battleStarted && !m_battleEnded);
    }

    public bool IsBallMoving()
    {
        return !IsLose();
    }

    public bool IsWin()
    {
        return m_battleEnded && m_isWin;
    }

    public bool IsLose()
    {
        return m_battleEnded && !m_isWin;
    }

    public BattleCanvas GetCanvas()
    {
        return m_canvas;
    }

    // Revive
    public void Revive()
    {
        m_battleEnded = false;
        //m_canvas.Destroy();
        //m_canvas = m_backupCanvas;
        //m_canvas.GetBattleRoad().SetCurPosY(0.0f);
        m_canvas.GetBattleBall().SetIsInvincible(true, 3.0f);
        m_canvas.GetBattleBall().Reset();
    }

    private bool CanRevive()
    {
        return !m_hasRevived && (Time.time - m_battleStartTime) > Const.canReviveTime; // && m_backupCanvas != null;
    }

    public void MakeBackupCanvas()
    {
        //m_backupCanvas = (BattleCanvas)DeepCopy(m_canvas);
        m_backupCanvas = new BattleCanvas(m_canvas.GetLevelId());
        PropertyInfo[] PI = m_canvas.GetType().GetProperties();
        for (int i = 0; i < PI.Length; i++)
        {
            PropertyInfo P = PI[i];
            P.SetValue(m_backupCanvas, P.GetValue(m_canvas));
        }
    }
}
