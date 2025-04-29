using UnityEngine;

public class ItemPanel : BasePanel
{
    public static ItemPanel instance;

    ItemPanel()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        SetActivePanelIndex(0);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnBallTabBtnClk()
    {
        SetActivePanelIndex(0);
    }

    public void OnRoadTabBtnClk()
    {
        SetActivePanelIndex(1);
    }

    public void SetActivePanelIndex(int index)
    {
        gameObject.transform.Find("Bg/BallPanel").gameObject.SetActive(index == 0);
        gameObject.transform.Find("Bg/RoadPanel").gameObject.SetActive(index == 1);
        gameObject.transform.Find("Bg/BallTabBtnParent").localScale = (index == 0 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1));
        gameObject.transform.Find("Bg/RoadTabBtnParent").localScale = (index == 1 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1));
    }

    // close MainPanel and move the ball and road to an upper pos when ItemPanel opened
    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();
        MainPanel.instance.ClosePanel();

        float y = Const.gameHeight / 2 / 100.0f / 2; // 1/4 to the ceil
        //BattleScene.instance.GetCanvas().GetBattleRoad().SetCurPosY(y, 0.2f);
        GameObject.Find("BattleScene/RoadParent").transform.localPosition = new Vector2(0, y);


        //GameObject roadParent = GameObject.Find("BattleScene/RoadParent").gameObject;
        //roadParent.transform.position = new Vector2(0, 4);
        //BattleScene.instance.GetCanvas().GetBattleRoad().SetCurPosY(0);
    }

    protected override void OnClosePanel()
    {
        base.OnClosePanel();
        MainPanel.instance.OpenPanel();

        //BattleScene.instance.GetCanvas().GetBattleRoad().SetCurPosY(0, 0.2f);
        GameObject.Find("BattleScene/RoadParent").transform.localPosition = new Vector2(0, 0);


        //GameObject roadParent = GameObject.Find("BattleScene/RoadParent").gameObject;
        //roadParent.transform.position = new Vector2(0, 0);
        //BattleScene.instance.GetCanvas().GetBattleRoad().inShopMode = true;
        //BattleScene.instance.GetCanvas().GetBattleRoad().SetCurPosY(0);
    }
}
