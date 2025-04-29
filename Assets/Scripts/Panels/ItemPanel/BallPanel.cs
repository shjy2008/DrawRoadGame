using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BallPanel : MonoBehaviour
{
    private GameObject m_pageViewObj;
    private Dictionary<int, GameObject> m_ballId2Obj;
    private Dictionary<int, GameObject> m_index2Panel;

    private bool m_isRandoming;
    private List<GameObject> m_randomingObjList;
    private int m_randomCurIndex;
    private int m_randomResultIndex;
    private int m_randomResultBallId;
    private float m_randomTimer;
    private int m_randomCounter;

    // Use this for initialization
    void Start()
    {
        Event.AddEventListener_string(Event.Type.OnBallItemClk, OnBallItemClk);

        m_pageViewObj = gameObject.transform.Find("PageView").gameObject;
        m_ballId2Obj = new Dictionary<int, GameObject>();
        m_index2Panel = new Dictionary<int, GameObject>();
        List<string> panelNames = new List<string> { "NormalPanel", "AdvancedPanel", "EpicPanel", "LegendaryPanel" };
        for (int i = 0; i < panelNames.Count; ++i)
        {
            m_index2Panel.Add(i, gameObject.transform.Find(panelNames[i]).gameObject);
        }

        // Buy with coins price
        gameObject.transform.Find("NormalPanel/BuyBtn/Price").gameObject.GetComponent<Text>().text = Table_constant.data["normal_ball_cost"].param1.ToString();
        gameObject.transform.Find("AdvancedPanel/BuyBtn/Price").gameObject.GetComponent<Text>().text = Table_constant.data["advanced_ball_cost"].param1.ToString();

        int pageCount = m_index2Panel.Count;
        int columnCount = 3;

        // init all ball items
        PageView pageView = m_pageViewObj.GetComponent<PageView>();
        pageView.OnPageChangedList.Add(OnPageChanged);

        GameObject contentObj = m_pageViewObj.transform.Find("Viewport/Content").gameObject;
        Vector2 contentObjSize = contentObj.GetComponent<RectTransform>().sizeDelta;
        contentObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Const.gameWidth * pageCount, contentObjSize.y);

        pageView.SetPageCount(pageCount);

        for (int i = 0; i < pageCount; ++i)
        {
            GameObject pageObj = Instantiate(Resources.Load<GameObject>("Prefabs/BallPage"), contentObj.transform);
            pageObj.GetComponent<RectTransform>().localPosition = new Vector2((i + 0.5f) * Const.gameWidth, 0);

            GameObject itemPrefab = pageObj.transform.Find("Item").gameObject;
            itemPrefab.SetActive(false);
            Vector2 itemOffset = pageObj.transform.Find("ItemOffset").gameObject.GetComponent<RectTransform>().rect.size;

            int type = i;
            List<Table_shop_ball.Data> datas = PlayerData.GetBallDatasWithType(type);
            for (int j = 0; j < datas.Count; ++j)
            {
                Table_shop_ball.Data data = datas[j];
                GameObject itemObj = Instantiate(itemPrefab, itemPrefab.transform.parent);
                itemObj.SetActive(true);
                BallItem ballItem = itemObj.GetComponent<BallItem>();

                // Pos
                float x = itemPrefab.transform.localPosition.x + (j % columnCount) * itemOffset.x;
                float y = itemPrefab.transform.localPosition.y - (j / columnCount) * itemOffset.y;
                itemObj.transform.localPosition = new Vector2(x, y);
                itemObj.name = data.key.ToString();

                m_ballId2Obj[data.key] = itemObj;

                // selected
                ballItem.SetSelected(data.key == PlayerData.curBallId);

                // Image
                bool have = PlayerData.ballList.Contains(data.key);
                ballItem.SetImage(string.Format("Balls/{0}", data.path));

                // Locked
                ballItem.SetLocked(!have);
            }
        }
        SetVisiblePanelIndex(0);
    }

    private void OnDestroy()
    {
        Event.RemoveEventListener_string(Event.Type.OnBallItemClk, OnBallItemClk);
    }

    private void OnBallItemClk(string name)
    {
        int.TryParse(name, out int key);
        if (PlayerData.ballList.Contains(key))
        {
            PlayerData.SetCurBallId(key);
            UpdateSelectedBall();
        }
    }

    private void UpdateSelectedBall()
    {
        foreach (GameObject obj in m_ballId2Obj.Values)
        {
            int.TryParse(obj.name, out int ballId);
            obj.GetComponent<BallItem>().SetSelected(ballId == PlayerData.curBallId);
        }
    }

    private void SetVisiblePanelIndex(int index)
    {
        for (int i = 0; i < m_index2Panel.Count; ++i)
        {
            m_index2Panel[i].SetActive(i == index);
        }
        UpdateButtonVisible(index);
    }

    private void OnPageChanged(int index)
    {
        SetVisiblePanelIndex(index);
        gameObject.transform.Find("LeftBtn").gameObject.SetActive(index != 0);
        gameObject.transform.Find("RightBtn").gameObject.SetActive(index != m_pageViewObj.GetComponent<PageView>().GetPageCount() - 1);
    }

    public void OnBuyNormalBtnClk()
    {
        int cost = Table_constant.data["normal_ball_cost"].param1;
        if (PlayerData.coin < cost)
        {
            GainCoinPanel.instance.OpenPanel();
        }
        else
        {
            PlayerData.ChangeCoin(-cost);
            GetRandomItem(Const.BallBuyType.Coin);
        }
    }

    public void OnBuyAdvancedBtnClk()
    {
        int cost = Table_constant.data["advanced_ball_cost"].param1;
        if (PlayerData.coin < cost)
        {
            GainCoinPanel.instance.OpenPanel();
        }
        else
        {
            PlayerData.ChangeCoin(-cost);
            GetRandomItem(Const.BallBuyType.ExpensiveCoin);
        }
    }

    public void OnWatchAdUnlockBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded);
    }

    private void OnRewardVideoEnded()
    {
        GetRandomItem(Const.BallBuyType.Video);
    }

    private void UpdateButtonVisible(int type)
    {
        List<Table_shop_ball.Data> notHaveList = PlayerData.GetNotHaveBallIdList(type);
        bool hasAll = (notHaveList.Count == 0);
        if (type == Const.BallBuyType.Coin)
        {
            gameObject.transform.Find("NormalPanel/BuyBtn").gameObject.SetActive(!hasAll);
        }
        else if (type == Const.BallBuyType.ExpensiveCoin)
        {
            gameObject.transform.Find("AdvancedPanel/BuyBtn").gameObject.SetActive(!hasAll);
        }
        else if (type == Const.BallBuyType.Video)
        {
            gameObject.transform.Find("EpicPanel/WatchAdUnlockBtn").gameObject.SetActive(!hasAll);
        }
    }

    private void GetRandomItem(int type)
    {
        List<Table_shop_ball.Data> notHaveList = PlayerData.GetNotHaveBallIdList(type);
        if (notHaveList.Count == 0)
        {
            // have got all
            Debug.Log("[error] BallPanel GetRandomItem: has got all");
        }
        else
        {
            Table_shop_ball.Data choice = RandomUtils.ListChoiceOne(notHaveList);
            //PlayerData.GainBallId(choice.key);

            m_isRandoming = true;
            m_randomingObjList = new List<GameObject>();
            foreach (Table_shop_ball.Data data in notHaveList)
            {
                m_randomingObjList.Add(m_ballId2Obj[data.key]);
            }
            m_randomCurIndex = -1;
            m_randomResultIndex = notHaveList.IndexOf(choice);
            m_randomResultBallId = choice.key;
            m_randomTimer = 0.0f;
            m_randomCounter = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isRandoming)
        {
            int maxCount = 15;
            float changeTime = 0.1f;
            float lastWaitTime = 0.5f;
            m_randomTimer += Time.deltaTime;
            if (m_randomCounter == maxCount - 1)
            {
                changeTime = lastWaitTime;
            }
            if (m_randomTimer > changeTime)
            {
                m_randomCounter += 1;
                if (m_randomCounter < maxCount)
                {
                    int index;
                    if (m_randomCounter == maxCount - 1)
                    {
                        index = m_randomResultIndex;
                    }
                    else
                    {
                        List<int> indexList = new List<int>();
                        for (int i = 0; i < m_randomingObjList.Count; ++i)
                        {
                            indexList.Add(i);
                        }
                        if (m_randomingObjList.Count > 1)
                        {
                            indexList.Remove(m_randomCurIndex);
                        }
                        index = RandomUtils.ListChoiceOne(indexList);
                    }
                    for (int i = 0; i < m_randomingObjList.Count; ++i)
                    {
                        m_randomingObjList[i].GetComponent<BallItem>().SetRandoming(i == index);
                    }
                    m_randomCurIndex = index;
                }
                else
                {
                    m_isRandoming = false;
                    GameObject resultObj = m_randomingObjList[m_randomResultIndex];
                    BallItem ballItem = resultObj.GetComponent<BallItem>();
                    ballItem.SetRandoming(false);
                    ballItem.SetLocked(false);
                    PlayerData.GainBallId(m_randomResultBallId);
                    UpdateSelectedBall();
                }
                m_randomTimer = 0.0f;
            }
        }
    }
}
