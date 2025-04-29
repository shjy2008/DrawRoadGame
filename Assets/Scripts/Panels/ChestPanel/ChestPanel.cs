using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChestPanel : BasePanel
{
    public static ChestPanel instance;

    private const int chestCount = 9;

    // reset in ResetPanelObject
    private List<GameObject> m_rewardObjList = new List<GameObject>();
    private List<GameObject> m_keyObjList = new List<GameObject>();

    // reset in ResetPanelData
    private List<int> m_coinNumList; // 9 items
    private bool m_hasBestReward; // Best reward can only be coins if you have owned all the chest rewards
    private Table_shop_ball.Data m_bestRewardData;
    private int m_bestRewardIndex;
    private int m_curOpenedRewardCount; // +1 when a reward has been opened
    private int m_curKeyCount; // -1 when a reward has been opened. Can be reset to 3 after watching an ad

    // objs
    private GameObject m_chestKeyParent;
    private GameObject m_watchAdBtn;
    private GameObject m_closeBtn;
    private GameObject m_particleObj;

    ChestPanel()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when a reward has been opened
    // ret: true if open successfully
    public bool SelectRewardIndex(int index)
    {
        if (PlayerData.chestKeyNum > 0)
        {
            PlayerData.UseAllChestKey();
        }

        if (m_curKeyCount <= 0)
            return false;

        GameObject rewardObj = m_rewardObjList[index];
        rewardObj.transform.Find("Chest").gameObject.SetActive(false);
        if (m_hasBestReward && m_curOpenedRewardCount == m_bestRewardIndex)
        {
            PlayerData.GainBallId(m_bestRewardData.key);

            GameObject imageObj = rewardObj.transform.Find("Image").gameObject;
            imageObj.SetActive(true);
            imageObj.GetComponent<Image>().sprite = GetBestRewardSprite();

            m_particleObj.GetComponent<ParticleSystem>().Play();
            Vector2 imagePos = imageObj.transform.position;
            m_particleObj.transform.position = new Vector3(imagePos.x, imagePos.y, m_particleObj.transform.position.z);
        }
        else
        {
            int coinNum = m_coinNumList[m_curOpenedRewardCount];
            IEnumerator _()
            {
                yield return new WaitForSeconds(0.5f);
                PlayerData.ChangeCoin(coinNum);
            }
            StartCoroutine(_());

            GameObject coinObj = rewardObj.transform.Find("Coin").gameObject;
            coinObj.SetActive(true);
            coinObj.transform.Find("Text").GetComponent<Text>().text = coinNum.ToString();

            // Gain coin ani
            rewardObj.GetComponent<FlyCoinAni>().RunAni();
        }

        m_curKeyCount -= 1;
        m_curOpenedRewardCount += 1;

        UpdateKeyObjs();

        return true;
    }

    private void UpdateKeyObjs()
    {
        for (int i = 0; i < Const.maxChestKey; ++i)
        {
            GameObject keyObj = m_keyObjList[i];
            bool have = i < m_curKeyCount;
            keyObj.transform.Find("Have").gameObject.SetActive(have);
            keyObj.transform.Find("NotHave").gameObject.SetActive(!have);
        }

        // use up the keys
        if (m_curKeyCount <= 0)
        {
            if (m_curOpenedRewardCount < chestCount) // can watch video to add keys
            {
                m_watchAdBtn.SetActive(true);
                m_chestKeyParent.SetActive(false);

                // Close btn should appear later
                m_closeBtn.SetActive(true);
                m_closeBtn.transform.Find("Text").GetComponent<Text>().text = "NO THANKS";
                m_closeBtn.GetComponent<Animator>().Play("BtnTransparent");
                IEnumerator _()
                {
                    yield return new WaitForSeconds(0.8f);
                    m_closeBtn.GetComponent<Animator>().Play("BtnFadeIn");
                }
                StartCoroutine(_());
            }
            else
            {
                m_watchAdBtn.SetActive(false);
                m_closeBtn.SetActive(true);
                m_closeBtn.transform.Find("Text").GetComponent<Text>().text = "NEXT";
                m_chestKeyParent.SetActive(false);
            }
        }
    }

    public void OnWatchAdBtnClk()
    {
        //UnityAdsHelper.instance.TryShowRewardedVideo(OnRewardVideoEnded);
    }

    private void OnRewardVideoEnded()
    {
        m_curKeyCount = Const.maxChestKey;
        UpdateKeyObjs();
        m_watchAdBtn.SetActive(false);
        m_closeBtn.SetActive(false);
        m_chestKeyParent.SetActive(true);
    }

    private Sprite GetBestRewardSprite()
    {
        Texture2D tex = Resources.Load<Texture2D>(string.Format("Balls/{0}", m_bestRewardData.path));
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    private void ResetPanelObject()
    {
        ClearObjs();

        // reward
        GameObject rewardPrefab = gameObject.transform.Find("Bg/RewardBg/RewardPrefab").gameObject;
        rewardPrefab.SetActive(false);
        Vector2 originPos = rewardPrefab.transform.localPosition;
        Vector2 offset = gameObject.transform.Find("Bg/RewardBg/Offset").gameObject.GetComponent<RectTransform>().rect.size;
        for (int i = 0; i < chestCount; ++i)
        {
            GameObject reward = Instantiate(rewardPrefab, rewardPrefab.transform.parent);
            reward.name = i.ToString();
            reward.SetActive(true);
            int column = i % 3;
            int row = i / 3;
            reward.transform.localPosition = new Vector2(originPos.x + offset.x * column, originPos.y - offset.y * row);
            m_rewardObjList.Add(reward);
        }

        // key
        m_chestKeyParent.SetActive(true);
        GameObject keyPrefab = m_chestKeyParent.transform.Find("KeyObjPrefab").gameObject;
        keyPrefab.SetActive(false);
        originPos = keyPrefab.transform.localPosition;
        offset = m_chestKeyParent.transform.Find("Offset").gameObject.GetComponent<RectTransform>().rect.size;
        for (int i = 0; i < Const.maxChestKey; ++i)
        {
            GameObject key = Instantiate(keyPrefab, keyPrefab.transform.parent);
            key.SetActive(true);
            key.transform.localPosition = new Vector2(originPos.x + offset.x * i, originPos.y);
            m_keyObjList.Add(key);
        }
        UpdateKeyObjs();

        // btn
        m_watchAdBtn.SetActive(false);
        m_closeBtn.SetActive(false);
    }

    private void ResetPanelData()
    {
        // reward list
        m_coinNumList = RandomUtils.GetShuffledList(Table_constant.data["chest_room_coin"].param2);
        List<Table_shop_ball.Data> bestRewardRandomList = new List<Table_shop_ball.Data>();
        foreach (Table_shop_ball.Data data in Table_shop_ball.data.Values)
        {
            if (data.type == Const.BallBuyType.Chest && !PlayerData.ballList.Contains(data.key))
            {
                bestRewardRandomList.Add(data);
            }
        }
        if (bestRewardRandomList.Count > 0)
        {
            m_hasBestReward = true;
            m_bestRewardData = RandomUtils.ListChoiceOne(bestRewardRandomList);
            //m_bestRewardIndex = RandomUtils.ListChoiceOne(new List<int> { 0, 1, 2 }); // best reward must be in the first three places
            m_bestRewardIndex = Random.Range(0, chestCount);
        }
        else
        {
            m_hasBestReward = false;
        }
        m_curOpenedRewardCount = 0;
        m_curKeyCount = Const.maxChestKey;

        // best reward
        GameObject bestRewardImg = gameObject.transform.Find("Bg/BestReward/Image").gameObject;
        GameObject bestRewardCoin = gameObject.transform.Find("Bg/BestReward/Coin").gameObject;
        if (m_hasBestReward)
        {
            bestRewardImg.SetActive(true);
            bestRewardImg.GetComponent<Image>().sprite = GetBestRewardSprite();
            bestRewardCoin.SetActive(false);
        }
        else
        {
            bestRewardImg.SetActive(false);
            bestRewardCoin.SetActive(true);
            int bestCoinNum = 0;
            foreach (int num in Table_constant.data["chest_room_coin"].param2)
            {
                if (num > bestCoinNum)
                {
                    bestCoinNum = num;
                }
            }
            bestRewardCoin.transform.Find("Text").gameObject.GetComponent<Text>().text = bestCoinNum.ToString();
        }
    }

    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();

        m_chestKeyParent = gameObject.transform.Find("Bg/ChestKey").gameObject;
        m_watchAdBtn = gameObject.transform.Find("Bg/WatchAdBtn").gameObject;
        m_closeBtn = gameObject.transform.Find("Bg/CloseBtn").gameObject;
        m_particleObj = gameObject.transform.Find("Bg/ConfettiExplosionPurple").gameObject;

        ResetPanelData();
        ResetPanelObject();
    }

    protected override void OnClosePanel()
    {
        ClearObjs();
        base.OnClosePanel();
    }

    private void ClearObjs()
    {
        foreach (GameObject obj in m_rewardObjList)
        {
            Destroy(obj);
        }
        m_rewardObjList.Clear();

        foreach (GameObject obj in m_keyObjList)
        {
            Destroy(obj);
        }
        m_keyObjList.Clear();
    }
}
