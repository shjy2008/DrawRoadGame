using System;
using UnityEngine;

public class BattleCloud : MonoBehaviour
{
    public float speed;

    public BattleCloud()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!BattleScene.instance.IsBattleEnded())
        {
            Vector2 prevPos = gameObject.transform.position;
            float canvasSpeed = BattleScene.instance.GetCanvas().GetMoveSpeed();
            gameObject.transform.position = new Vector2(prevPos.x - speed * canvasSpeed * Time.deltaTime, prevPos.y);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}


public class BattleCloudManager
{
    private float m_genTimer;
    private GameObject m_bg;

    public BattleCloudManager()
    {
        m_genTimer = 0;
        m_bg = GameObject.Find("BattleScene/Bg");

        for (int i = 0; i < 5; ++i)
        {
            CreateCloud(true);
        }
    }

    public void Update()
    {
        m_genTimer -= Time.deltaTime;
        if (m_genTimer < 0)
        {
            CreateCloud(false);
            m_genTimer = UnityEngine.Random.Range(2.0f, 5.0f);
        }
    }

    private void CreateCloud(bool isInit)
    {
        GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Cloud"), m_bg.transform);
        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        obj.transform.localScale = new Vector2(scale, scale);
        obj.AddComponent<BattleCloud>();
        obj.GetComponent<BattleCloud>().speed = UnityEngine.Random.Range(0.1f, 0.3f);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = m_bg.GetComponent<SpriteRenderer>().sortingOrder + 1;
        if (isInit)
        {
            obj.transform.position = new Vector2(UnityEngine.Random.Range(Const.leftX, Const.rightX), UnityEngine.Random.Range(Const.bottomY, Const.topY));
        }
        else
        {
            obj.transform.position = new Vector2(Const.rightX + sr.bounds.size.x / 2, UnityEngine.Random.Range(Const.bottomY, Const.topY));
        }
    }
}