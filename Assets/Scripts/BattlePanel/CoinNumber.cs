using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinNumber : MonoBehaviour
{
    private GameObject m_imageObj;
    private GameObject m_textObj;

    private const float SCROLL_TIME = 0.5f;
    private int m_startNum;
    private int m_targetNum;
    private bool m_isScrolling;
    private float m_scrollTimer;

    // Use this for initialization
    void Start()
    {
        Event.AddEventListener(Event.Type.OnCoinUpdated, OnCoinUpdated);

        m_imageObj = gameObject.transform.Find("Image").gameObject;
        m_textObj = gameObject.transform.Find("Text").gameObject;

        m_startNum = PlayerData.coin;
        m_targetNum = 0;
        m_isScrolling = false;
        m_scrollTimer = 0;

        m_textObj.GetComponent<Text>().text = PlayerData.coin.ToString();
    }

    private void OnDestroy()
    {
        Event.RemoveEventListener(Event.Type.OnCoinUpdated, OnCoinUpdated);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isScrolling)
        {
            Text text = m_textObj.GetComponent<Text>();
            m_scrollTimer += Time.deltaTime;
            if (m_scrollTimer > SCROLL_TIME)
            {
                text.text = ((int)m_targetNum).ToString();
                m_isScrolling = false;
            }
            else
            {
                text.text = ((int)(m_startNum + (m_targetNum - m_startNum) * (m_scrollTimer / SCROLL_TIME))).ToString();
            }
        }
    }

    private void OnCoinUpdated()
    {
        int.TryParse(m_textObj.GetComponent<Text>().text, out int num);
        m_startNum = num;
        m_targetNum = PlayerData.coin;
        m_isScrolling = true;
        m_scrollTimer = 0;

        m_textObj.GetComponent<Animator>().Play("CoinAnim", 0, 0);
        m_imageObj.GetComponent<Animator>().Play("CoinAnim", 0, 0);
    }

}
