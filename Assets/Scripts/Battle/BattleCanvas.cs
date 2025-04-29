using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manage the ball, road, blocks, items and level
public class BattleCanvas
{
    private readonly GameObject m_ballObj;
    public readonly GameObject blockParent;
    public readonly GameObject roadParent;
    public readonly GameObject blockRectPrefab;
    public readonly GameObject blockCirclePrefab;
    public readonly GameObject coinPrefab;

    // Speed
    private const float m_originMoveSpeed = 4.0f;
    private float m_moveSpeedScale = 1.0f;

    public float ballRadius = 0.25f;
    //private float m_ballMaxDropSpeed;

    private List<BattleBlock> m_blockList;
    private List<GameObject> m_itemList;
    private List<BlockGenerator> m_generatorList;
    private BattleRoad3D m_road;
    private BattleLevel m_level;
    private bool m_isTouching;

    private int m_score;
    private int m_coin;
    private int m_levelId;

    public BattleCanvas(int levelId)
    {
        Event.AddEventListener(Event.Type.OnCurBallChanged, OnCurBallChanged);
        Event.AddEventListener(Event.Type.OnCurRoadChanged, OnCurRoadChanged);

        m_ballObj = GameObject.Find("BattleScene/Ball").gameObject;
        blockParent = GameObject.Find("BattleScene/BlockParent").gameObject;
        roadParent = GameObject.Find("BattleScene/RoadParent").gameObject;
        if (Const_test.battle3D)
        {
            blockRectPrefab = Resources.Load<GameObject>("Blocks3D/Cube");
            blockCirclePrefab = Resources.Load<GameObject>("Blocks3D/Cylinder");
        }
        else
        {
            blockRectPrefab = Resources.Load<GameObject>("Prefabs/BlockRect");
            blockCirclePrefab = Resources.Load<GameObject>("Prefabs/BlockCircle");
        }
        coinPrefab = Resources.Load<GameObject>("Item3D/Coin");

        //m_ballMaxDropSpeed = 12.0f;

        m_blockList = new List<BattleBlock>();
        m_itemList = new List<GameObject>();
        m_generatorList = new List<BlockGenerator>();
        CreateRoad();
        if (levelId == Const.endless_levelId)
        {
            m_level = new Level_Endless(this);
        }
        else
        {
            //m_level = (BattleLevel)Activator.CreateInstance(Type.GetType(string.Format("Level_{0}", level)), new object[] { this });
            m_level = new Level_Normal(this, levelId);
        }

        m_score = 0;
        Event.PostEvent(Event.Type.OnScoreUpdated, m_score);

        m_coin = 0;
        m_levelId = levelId;

        GetBattleBall().SetIsInvincible(false);
        //GetBattleBall().Reset();
    }

    public void Destroy()
    {
        Event.RemoveEventListener(Event.Type.OnCurBallChanged, OnCurBallChanged);
        Event.RemoveEventListener(Event.Type.OnCurRoadChanged, OnCurRoadChanged);
        RemoveAllBlocks();
        m_road.Destroy();
        m_level.Destroy();
        RemoveAllItems();
        GetBattleBall().Reset();
    }

    public int GetLevelId()
    {
        return m_levelId;
    }

    public BattleBall GetBattleBall()
    {
        return m_ballObj.GetComponent<BattleBall>();
    }

    // Speed
    public float GetMoveSpeed()
    {
        return m_originMoveSpeed * GetSpeedScale();
    }

    public void SetSpeedScale(float scale)
    {
        m_moveSpeedScale = scale;
    }

    public float GetSpeedScale()
    {
        if (Const_test.changeSpeedScale)
        {
            return Const_test.speedScale;
        }
        return m_moveSpeedScale;
    }

    // Ball or Road changed
    private void OnCurBallChanged()
    {
        GetBattleBall().SetBallId(PlayerData.curBallId);
    }

    private void OnCurRoadChanged()
    {
        CreateRoad();
    }

    private void CreateRoad()
    {
        if (m_road != null)
        {
            m_road.Destroy();
            m_road = null;
        }
        Table_shop_road.Data roadData = Table_shop_road.data[PlayerData.curRoadId];
        if (Const_test.battle3D)
        {
            m_road = new BattleRoad3D_Continuous(this, PlayerData.curRoadId);
        }
        else
        {
            //if (roadData.imageType == 0)
            //{
            //    m_road = new BattleRoad_Continuous(this, PlayerData.curRoadId);
            //}
            //else if (roadData.imageType == 1)
            //{
            //    m_road = new BattleRoad_Image(this, PlayerData.curRoadId);
            //}
        }
    }

    public BattleRoad3D GetBattleRoad()
    {
        return m_road;
    }

    public BattleLevel GetBattleLevel()
    {
        return m_level;
    }

    public void Update()
    {
        if (!InputMgr.IsClickedUI())
        {
            if (InputMgr.IsBegan())
            {
                m_isTouching = true;
                m_road.OnTouchBegan(InputMgr.GetTouchPos());
            }
            else if (InputMgr.IsMoved())
            {
                if (m_isTouching)
                {
                    if (!BattleScene.instance.IsBattleEnded())
                        m_road.OnTouchMoved(InputMgr.GetTouchPos());
                }
            }
            else if (InputMgr.IsEnded())
            {
                m_isTouching = false;
                m_road.OnTouchEnded(InputMgr.GetTouchPos());
            }
        }

        if (BattleScene.instance.IsBallMoving())
        {
            // road update
            m_road.Update();
        }

        if (BattleScene.instance.IsBlocksMoving())
        {
            // update blocks
            foreach (BlockGenerator generator in m_generatorList)
            {
                generator.Update();
            }

            List<BattleBlock> copiedBlocks = new List<BattleBlock>(m_blockList);
            foreach (BattleBlock block in copiedBlocks)
            {
                block.Update();
            }

            foreach (GameObject item in m_itemList)
            {
                Vector3 prevPos = item.transform.localPosition;
                item.transform.localPosition = new Vector3(prevPos.x - Time.deltaTime * GetMoveSpeed(), prevPos.y, item.transform.localPosition.z);
            }

            // level update
            m_level.Update();
        }
    }

    // block
    public void AddBlock(BattleBlock block)
    {
        m_blockList.Add(block);
    }

    public void RemoveBlock(BattleBlock block)
    {
        block.Destroy();
        m_blockList.Remove(block);
    }

    public void RemoveAllBlocks()
    {
        foreach (BattleBlock block in m_blockList)
        {
            block.Destroy();
        }
        m_blockList.Clear();
    }

    public void AddBlockGenerator(BlockGenerator generator)
    {
        m_generatorList.Add(generator);
    }

    public void RemoveBlockGenerator(BlockGenerator generator)
    {
        m_generatorList.Remove(generator);
    }

    public void RemoveAllBlockGenerator()
    {
        m_generatorList.Clear();
    }

    // score
    public void AddScore(int score)
    {
        m_score += score;
        Event.PostEvent(Event.Type.OnScoreUpdated, m_score);
    }

    public void SetScore(int score)
    {
        m_score = score;
        Event.PostEvent(Event.Type.OnScoreUpdated, m_score);
    }

    public int GetScore()
    {
        return m_score;
    }

    // coin
    public void AddCoin(int num = 1)
    {
        m_coin += num;
    }

    public int GetCoin()
    {
        return m_coin;
    }

    // items
    public void AddItem(GameObject item)
    {
        m_itemList.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        UnityEngine.Object.Destroy(item);
        m_itemList.Remove(item);
    }

    public void RemoveAllItems()
    {
        foreach (GameObject item in m_itemList)
        {
            UnityEngine.Object.Destroy(item);
        }
        m_itemList.Clear();
    }

    // if not assign posX, use Const.rightX + width / 2
    public void CreateChestKey(float posY, bool assignX = false, float posX = 0.0f)
    {
        //  maybe have a key for the chest room!
        GameObject keyObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Item3D/Key"));
        keyObj.AddComponent<BattleItem>();
        keyObj.name = "ChestKey";
        float x;
        if (assignX)
        {
            x = posX;
        }
        else
        {
            SpriteRenderer sr = keyObj.GetComponent<SpriteRenderer>();
            x = Const.rightX + sr.bounds.size.x / 2;
        }
        keyObj.transform.position = new Vector3(x, posY, keyObj.transform.position.z);
        keyObj.transform.SetParent(blockParent.transform);
        AddItem(keyObj);
    }

    // if not assign posX, use Const.rightX + width / 2
    public void CreateCoin(float posY, bool assignX = false, float posX = 0.0f)
    {
        GameObject coinObj = UnityEngine.Object.Instantiate(coinPrefab);
        coinObj.AddComponent<BattleItem>();
        coinObj.name = "Coin";
        float x;
        if (assignX)
        {
            x = posX;
        }
        else
        {
            //SpriteRenderer sr = coinObj.transform.Find("Icon").GetComponent<SpriteRenderer>();
            //x = Const.rightX + sr.bounds.size.x / 2;
            x = Const.rightX + coinObj.transform.localScale.x / 2;
        }
        coinObj.transform.localPosition = new Vector3(x, posY, coinObj.transform.localPosition.z);
        coinObj.transform.SetParent(blockParent.transform);
        AddItem(coinObj);
    }

    // Deprecated
    public void CreateCheckpoint()
    {
        float width = 0.5f;
        GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/FinishLine"));
        obj.AddComponent<BattleItem>();
        obj.name = "Checkpoint";
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.color = Color.green;
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(width, Const.topY - Const.bottomY);
        BoxCollider2D bc2d = obj.GetComponent<BoxCollider2D>();
        bc2d.size = sr.bounds.size;
        obj.transform.position = new Vector3(Const.rightX + width / 2, 0, obj.transform.position.z);
        obj.transform.SetParent(blockParent.transform);
        AddItem(obj);
    }

    public void CreateDebugLine(bool isStart)
    {
        float width = 0.5f;
        GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/FinishLine"));
        obj.AddComponent<BattleItem>();
        obj.name = "Debug";
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.color = isStart ? Color.green : Color.red;
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(width, Const.topY - Const.bottomY);
        BoxCollider2D bc2d = obj.GetComponent<BoxCollider2D>();
        bc2d.size = sr.bounds.size;
        obj.transform.position = new Vector3(Const.rightX + width / 2, 0, obj.transform.position.z);
        obj.transform.SetParent(blockParent.transform);
        AddItem(obj);
    }
}
