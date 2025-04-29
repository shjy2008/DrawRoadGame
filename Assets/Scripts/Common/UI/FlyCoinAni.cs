using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyCoinAni_coin
{
    private enum State
    {
        None = -1,
        Drop = 0,
        Wait = 1,
        Fly = 2,
    }

    private FlyCoinAni m_ani;

    private Vector2 m_fromPos;
    private Vector2 m_toPos;

    private State m_curState = State.None;
    private GameObject m_obj;
    private float m_timer;
    private Vector2 m_dropPos;
    private Vector2 m_curveMidPos;

    private float m_waitTime;

    public FlyCoinAni_coin(FlyCoinAni ani)
    {
        m_ani = ani;

        m_obj = UnityEngine.Object.Instantiate(m_ani.coinObjPrefab, m_ani.parent.transform);
        m_obj.SetActive(true);

        m_fromPos = m_ani.fromPosObj.transform.position;
        m_toPos = m_ani.toPosObj.transform.position;

        //m_obj.transform.position = m_fromPos;

        float dropMaxRange = m_ani.dropMaxRange;
        m_dropPos = m_fromPos + new Vector2(UnityEngine.Random.Range(-dropMaxRange, dropMaxRange), UnityEngine.Random.Range(-dropMaxRange, dropMaxRange));

        float radian = Mathf.Atan2(-(m_toPos.y - m_dropPos.y), (m_toPos.x - m_dropPos.x));
        float diffX = 200 * Mathf.Cos(radian);
        float diffY = 200 * Mathf.Sin(radian);
        m_curveMidPos = new Vector2((m_dropPos.x + m_toPos.x) / 2 - diffX, (m_dropPos.y + m_toPos.y) / 2 - diffY);

        m_waitTime = m_ani.waitTime + UnityEngine.Random.Range(-0.1f, 0.1f);

        m_curState = State.Drop;
    }

    public void Update()
    {
        if (m_curState == State.Drop)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_ani.dropTime)
            {
                m_timer = 0;
                m_curState = State.Wait;
                m_obj.transform.position = m_dropPos;
            }
            else
            {
                m_obj.transform.position = m_fromPos + m_timer / m_ani.dropTime * (m_dropPos - m_fromPos);
            }
        }

        else if (m_curState == State.Wait)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_waitTime)
            {
                m_timer = 0;
                m_curState = State.Fly;
            }
        }

        else if (m_curState == State.Fly)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_ani.flyTime)
            {
                m_timer = 0;
                m_curState = State.None;

                //m_coinObjList[i].transform.position = m_toPos;
                UnityEngine.Object.Destroy(m_obj);

                m_ani.RemoveCoin(this);
            }
            else
            {
                //m_coinObjList[i].transform.position = m_dropPosList[i] + m_timer / m_flyTime * (m_toPos - m_dropPosList[i]);
                Vector2 pos = UiUtil.QuadraticBezierCurve(m_dropPos, m_curveMidPos, m_toPos, m_timer / m_ani.flyTime);
                m_obj.transform.position = pos;
            }
        }
    }
}

public class FlyCoinAni : MonoBehaviour
{
    public GameObject coinObjPrefab;
    public GameObject fromPosObj;
    public GameObject toPosObj;
    public GameObject parent;

    private const int maxCount = 80;

    public float dropMaxRange = 120.0f;
    public int count = 10;
    public float dropTime = 0.2f;
    public float waitTime = 0.2f;
    public float flyTime = 0.3f;

    private List<FlyCoinAni_coin> m_coinList = new List<FlyCoinAni_coin>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Coin may be removed in Update(), so we need to create a new temp list
        List<FlyCoinAni_coin> tmpCoinList = new List<FlyCoinAni_coin>(m_coinList);
        foreach (FlyCoinAni_coin coin in tmpCoinList)
        {
            coin.Update();
        }
    }

    public void RunAni()
    {
        if (count > maxCount)
        {
            count = maxCount;
        }

        for (int i = 0; i < count; ++i)
        {
            FlyCoinAni_coin coin = new FlyCoinAni_coin(this);
            m_coinList.Add(coin);
        }
    }

    public void RemoveCoin(FlyCoinAni_coin coin)
    {
        m_coinList.Remove(coin);
    }
}
