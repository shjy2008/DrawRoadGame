using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RevivePanel : BasePanel
{
    public static RevivePanel instance;

    private bool m_isCountingDown;
    private float m_reviveTimer;
    private const float m_reviveTotalTime = 9.0f;

    RevivePanel()
    {
        instance = this;

        m_isCountingDown = false;
        m_reviveTimer = 0.0f;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_isCountingDown)
        {
            m_reviveTimer += Time.deltaTime;
            if (m_reviveTimer > m_reviveTotalTime)
            {
                m_isCountingDown = false;
                GotoLosePanel();
            }
            else
            {
                GameObject radianProgressObj = gameObject.transform.Find("Bg/RadianProgress").gameObject;
                radianProgressObj.GetComponent<Image>().fillAmount = 1 - m_reviveTimer / m_reviveTotalTime;
                gameObject.transform.Find("Bg/CountDownText").GetComponent<Text>().text = ((int)(m_reviveTotalTime - m_reviveTimer + 1)).ToString();
            }
        }
    }

    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();
        m_isCountingDown = true;
        m_reviveTimer = 0.0f;

        // Close btn should appear later
        gameObject.transform.Find("Bg/CloseBtn").GetComponent<Animator>().Play("BtnTransparent");
        IEnumerator _()
        {
            yield return new WaitForSeconds(1.8f);
            gameObject.transform.Find("Bg/CloseBtn").GetComponent<Animator>().Play("BtnFadeIn");
        }
        StartCoroutine(_());
    }

    protected override void OnClosePanel()
    {
        m_isCountingDown = false;
        base.OnClosePanel();
    }

    public void OnReviveBtnClk()
    {
        m_isCountingDown = false;
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded);
    }

    private void OnRewardVideoEnded()
    {
        BattleScene.instance.Revive();
        ClosePanel();
    }

    public void OnNoThanksBtnClk()
    {
        GotoLosePanel();
    }

    public void GotoLosePanel()
    {
        ClosePanel();

        IEnumerator _()
        {
            yield return new WaitForSeconds(0.3f);
            LosePanel.instance.OpenPanel();
            LosePanel.instance.SetRewardCoinNum(BattleScene.instance.GetCanvas().GetCoin());
        }
        StartCoroutine(_());
    }

}
