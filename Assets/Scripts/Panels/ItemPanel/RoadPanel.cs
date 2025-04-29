using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoadPanel : MonoBehaviour
{
    private GameObject m_pageViewObj;
    private Dictionary<int, GameObject> m_roadId2Obj = new Dictionary<int, GameObject>();

    // Use this for initialization
    void Start()
    {
        Event.AddEventListener_string(Event.Type.OnRoadItemClk, OnRoadItemClk);
        Event.AddEventListener_int(Event.Type.OnNewRoadUnlocked, OnNewRoadUnlocked);
        Event.AddEventListener(Event.Type.OnCurRoadChanged, OnCurRoadChanged);

        m_pageViewObj = gameObject.transform.Find("PageView").gameObject;

        int pageCount = 3;
        int columnCount = 3;

        PageView pageView = m_pageViewObj.GetComponent<PageView>();
        pageView.OnPageChangedList.Add(OnPageChanged);

        GameObject contentObj = m_pageViewObj.transform.Find("Viewport/Content").gameObject;
        Vector2 contentObjSize = contentObj.GetComponent<RectTransform>().sizeDelta;
        contentObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Const.gameWidth * pageCount, contentObjSize.y);

        pageView.SetPageCount(pageCount);

        for (int i = 0; i < pageCount; ++i)
        {
            GameObject pageObj = Instantiate(Resources.Load<GameObject>("Prefabs/RoadPage"), contentObj.transform);
            pageObj.GetComponent<RectTransform>().localPosition = new Vector2((i + 0.5f) * Const.gameWidth, 0);

            GameObject itemPrefab = pageObj.transform.Find("Item").gameObject;
            itemPrefab.SetActive(false);
            Vector2 itemOffset = pageObj.transform.Find("ItemOffset").gameObject.GetComponent<RectTransform>().rect.size;

            int type = i;
            List<Table_shop_road.Data> datas = PlayerData.GetRoadDatasWithAchieveType(type);
            for (int j = 0; j < datas.Count; ++j)
            {
                Table_shop_road.Data data = datas[j];
                GameObject itemObj = Instantiate(itemPrefab, itemPrefab.transform.parent);
                itemObj.SetActive(true);
                RoadItem roadItem = itemObj.GetComponent<RoadItem>();

                // Pos
                float x = itemPrefab.transform.localPosition.x + (j % columnCount) * itemOffset.x;
                float y = itemPrefab.transform.localPosition.y - (j / columnCount) * itemOffset.y;
                itemObj.transform.localPosition = new Vector2(x, y);
                itemObj.name = data.key.ToString();

                m_roadId2Obj[data.key] = itemObj;

                // selected
                roadItem.SetSelected(data.key == PlayerData.curRoadId);

                // Image
                bool have = PlayerData.roadList.Contains(data.key);
                roadItem.SetImage(string.Format("Roads/{0}", data.path));

                // Locked
                roadItem.SetLocked(!have);

            }
        }
        SetVisiblePanelIndex(0);
        UpdateDescText(0);
    }

    private void OnDestroy()
    {
        Event.RemoveEventListener_string(Event.Type.OnRoadItemClk, OnRoadItemClk);
        Event.RemoveEventListener_int(Event.Type.OnNewRoadUnlocked, OnNewRoadUnlocked);
        Event.RemoveEventListener(Event.Type.OnCurRoadChanged, OnCurRoadChanged);
    }

    private void OnNewRoadUnlocked(int roadId)
    {
        m_roadId2Obj[roadId].GetComponent<RoadItem>().SetLocked(false);
    }

    private void OnCurRoadChanged()
    {
        foreach (KeyValuePair<int, GameObject> data in m_roadId2Obj)
        {
            data.Value.GetComponent<RoadItem>().SetSelected(data.Key == PlayerData.curRoadId);
        }
    }

    private void OnRoadItemClk(string name)
    {
        int.TryParse(name, out int key);
        if (PlayerData.roadList.Contains(key))
        {
            PlayerData.SetCurRoadId(key);
            UpdateSelectedRoad();
        }
        UpdateDescText(key);
    }

    private void OnPageChanged(int index)
    {
        SetVisiblePanelIndex(index);
        gameObject.transform.Find("LeftBtn").gameObject.SetActive(index != 0);
        gameObject.transform.Find("RightBtn").gameObject.SetActive(index != m_pageViewObj.GetComponent<PageView>().GetPageCount() - 1);
        UpdateDescText(0);
    }

    private void UpdateSelectedRoad()
    {
        foreach (GameObject obj in m_roadId2Obj.Values)
        {
            int.TryParse(obj.name, out int roadId);
            obj.GetComponent<RoadItem>().SetSelected(roadId == PlayerData.curRoadId);
        }
    }

    private void UpdateDescText(int roadId)
    {
        if (roadId == 0 || PlayerData.roadList.Contains(roadId))
        {
            gameObject.transform.Find("Text").gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.Find("Text").gameObject.SetActive(true);
            Table_shop_road.Data roadData = Table_shop_road.data[roadId];
            int curParam = 0;
            if (roadData.achieveType == Const.AchieveType.Level)
            {
                curParam = LevelData.GetCurLevel();
            }
            else if (roadData.achieveType == Const.AchieveType.Endless_score)
            {
                curParam = PlayerData.bestScore;
            }
            else if (roadData.achieveType == Const.AchieveType.Login)
            {
                curParam = PlayerData.loginDays;
            }
            string progressText = string.Format(" ({0}/{1})", curParam, roadData.param);
            gameObject.transform.Find("Text").GetComponent<Text>().text = string.Format(roadData.desc, roadData.param) + progressText;
        }
    }

    private void SetVisiblePanelIndex(int index)
    {
        Text titleText = gameObject.transform.Find("Title").gameObject.GetComponent<Text>();
        if (index == 0)
        {
            titleText.text = "NORMAL GAME";
        }
        else if (index == 1)
        {
            titleText.text = "ENDLESS GAME";
        }
        else if (index == 2)
        {
            titleText.text = "LOGIN REWARDS";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
