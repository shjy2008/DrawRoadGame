using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GainNewRoadPanel : BasePanel
{
    public static GainNewRoadPanel instance;
    private int m_roadId;

    GainNewRoadPanel()
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

    public void SetRoadId(int roadId)
    {
        m_roadId = roadId;
        Table_shop_road.Data roadData = Table_shop_road.data[roadId];
        string path = string.Format("Roads/{0}", roadData.path);
        Texture2D tex = Resources.Load<Texture2D>(path);
        gameObject.transform.Find("Bg/RoadItem/Image").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        gameObject.transform.Find("Bg/Desc").GetComponent<Text>().text = string.Format(roadData.desc, roadData.param);
    }

    public void OnOkBtnClk()
    {
        PlayerData.SetCurRoadId(m_roadId);
        ClosePanel();
    }
}
