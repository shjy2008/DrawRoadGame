using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRoad
{
    protected BattleCanvas m_canvas;
    private float m_curPosY;
    private float m_drawPosX;
    protected const float m_lineWidth = 0.32f;
    private const float m_minPosY = -5.0f;
    private const float m_maxPosY = 4.0f;

    // Set posY gradually
    private float m_settingFromY;
    private float m_settingTargetY;
    private float m_settingTimer;
    private float m_settingTotalTime;
    private bool m_isSettingPosY;

    private float m_beganPosY;
    private Vector2 m_beganTouchPos;

    private List<Vector2> m_posList;
    protected List<GameObject> m_objList;
    public readonly GameObject m_roadRectPrefab;
    public readonly GameObject m_roadCirclePrefab;

    public BattleRoad(BattleCanvas canvas, int roadId)
    {
        m_canvas = canvas;
        m_curPosY = 0.0f;
        m_drawPosX = 0.0f;

        m_settingFromY = 0;
        m_settingTargetY = 0;
        m_settingTimer = 0;
        m_settingTotalTime = 0;
        m_isSettingPosY = false;

        m_beganPosY = 0.0f;
        m_beganTouchPos = new Vector2();

        m_posList = new List<Vector2>();
        m_objList = new List<GameObject>();

        // origin line
        m_posList.Add(new Vector2(Const.leftX, m_curPosY));
        m_posList.Add(new Vector2(m_drawPosX, m_curPosY));
        m_roadRectPrefab = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/RoadRect"));
        m_roadCirclePrefab = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/RoadCircle"));
        m_roadRectPrefab.SetActive(false);
        m_roadCirclePrefab.SetActive(false);

        Table_shop_road.Data roadData = Table_shop_road.data[PlayerData.curRoadId];
        Texture2D tex = Resources.Load<Texture2D>(string.Format("Roads/{0}", roadData.path));
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect); ;
        m_roadRectPrefab.GetComponent<SpriteRenderer>().sprite = sprite;

        if (roadData.path_circle != "")
        {
            Texture2D tex_circle = Resources.Load<Texture2D>(string.Format("Roads/{0}", roadData.path_circle));
            m_roadCirclePrefab.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex_circle, new Rect(0, 0, tex_circle.width, tex_circle.height), new Vector2(0.5f, 0.5f));
        }

        CreateRectObj(m_posList[0], m_posList[1]);
    }

    public void Destroy()
    {
        foreach (GameObject obj in m_objList)
        {
            UnityEngine.Object.Destroy(obj);
        }
        m_objList.Clear();

        UnityEngine.Object.Destroy(m_roadRectPrefab);
        UnityEngine.Object.Destroy(m_roadCirclePrefab);
    }

    public void RemoveObj(GameObject obj)
    {
        UnityEngine.Object.Destroy(obj);
        m_objList.Remove(obj);
    }

    protected virtual void CreateRectObj(Vector2 pos1, Vector2 pos2)
    {
        // Implements in subclasses
    }

    public void Update()
    {
        // Set posY gradually
        if (m_isSettingPosY)
        {
            m_settingTimer += Time.deltaTime;
            if (m_settingTimer > m_settingTotalTime)
            {
                m_curPosY = m_settingTargetY;
                m_isSettingPosY = false;
            }
            else
            {
                m_curPosY = m_settingFromY + (m_settingTargetY - m_settingFromY) * (m_settingTimer / m_settingTotalTime);
            }
        }

        // move the line
        float diff = Time.deltaTime * m_canvas.GetMoveSpeed();
        List<Vector2> newList = new List<Vector2>();
        int toRemoveIndex = -1;
        for (int i = 0; i < m_posList.Count; ++i)
        {
            Vector2 pos = m_posList[i];
            float x = pos.x - diff;
            if (x < Const.leftX)
            {
                toRemoveIndex = i;
            }
            newList.Add(new Vector2(x, pos.y));
        }
        if (toRemoveIndex > 0)
        {
            newList = newList.GetRange(toRemoveIndex, newList.Count - toRemoveIndex);
        }
        m_posList = newList;

        foreach (GameObject obj in m_objList)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x - diff, obj.transform.localPosition.y, obj.transform.localPosition.z);
        }

        // add a point
        m_posList.Add(new Vector2(m_drawPosX, m_curPosY));

        CreateRectObj(m_posList[m_posList.Count - 2], m_posList[m_posList.Count - 1]);
    }

    public void OnTouchBegan(Vector2 touchPos)
    {
        Vector2 worldPos = GetTouchWorldPos(touchPos);
        m_beganPosY = m_curPosY;
        m_beganTouchPos = worldPos;
    }

    public void OnTouchMoved(Vector2 touchPos)
    {
        Vector2 worldPos = GetTouchWorldPos(touchPos);
        float diffY = worldPos.y - m_beganTouchPos.y;
        float posY = m_beganPosY + diffY;

        // limit y to prevent the ball move outside screen
        float clampedY = Mathf.Clamp(posY, m_minPosY, m_maxPosY);
        posY = clampedY;

        m_curPosY = posY;
    }

    public void OnTouchEnded(Vector2 touchPos)
    {

    }

    public void SetCurPosY(float posY, float time = 0.0f)
    {
        if (time <= 0)
        {
            m_curPosY = posY;
        }
        else
        {
            m_settingFromY = m_curPosY;
            m_settingTargetY = posY;
            m_settingTimer = 0;
            m_settingTotalTime = time;
            m_isSettingPosY = true;
        }
    }

    Vector2 GetTouchWorldPos(Vector2 touchPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
        return worldPos;
    }

    private Vector2 GetLineUpperPos(Vector2 pos, float angle)
    {
        float x = pos.x - m_lineWidth / 2 * Mathf.Sin(angle);
        float y = pos.y + m_lineWidth / 2 * Mathf.Cos(angle);
        return new Vector2(x, y);
    }

    public float GetBallPosY()
    {
        float ballRadius = m_canvas.ballRadius;
        float ballPosX = Const.ballPosX;
        List<Vector2> posList = new List<Vector2>(); // poses of the upper line around the ball
        float ball_minX = ballPosX - ballRadius;
        float ball_maxX = ballPosX + ballRadius;
        bool minPosFound = false;
        bool maxPosFound = false;
        for (int i = 1; i < m_posList.Count; ++i)
        {
            Vector2 pos = m_posList[i];
            Vector2 prevPos = m_posList[i - 1];
            float angle = Mathf.Atan2(pos.y - prevPos.y, pos.x - prevPos.x);
            pos = GetLineUpperPos(pos, angle);
            prevPos = GetLineUpperPos(prevPos, angle);
            if (pos.x > ball_minX && pos.x < ball_maxX)
            {
                if (i > 0 && !minPosFound)
                {
                    posList.Add(prevPos);
                }
                minPosFound = true;
                posList.Add(pos);
            }
            if (pos.x > ball_maxX && !maxPosFound)
            {
                maxPosFound = true;
                posList.Add(pos);
            }
        }

        float ballPosY = -10000;
        if (posList.Count == 1)
            ballPosY = posList[0].y + ballRadius;
        else
        {
            for (int i = 1; i < posList.Count; ++i)
            {
                Vector2 pos1 = posList[i - 1];
                Vector2 pos2 = posList[i];

                float tempMaxY = -10000;

                // Is it necessary to deal with circle cross line? maybe not necessary
                //float angle = Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x);
                //float y = (pos2.y - pos1.y) * (m_ballPosX - pos1.x) / (pos2.x - pos1.x) + pos1.y; // two dot formula
                //if (y > Mathf.Min(pos1.y, pos2.y) && y < Mathf.Max(pos1.y, pos2.y))
                //{
                //    tempMaxY = y + m_ballRadius / Mathf.Cos(angle); // y: when the circle cross the line, y of the circle
                //}

                if (pos1.x > ball_minX && pos1.x < ball_maxX)
                {
                    float y1 = pos1.y + ballRadius;// Mathf.Sqrt(Mathf.Pow(ballRadius, 2) - Mathf.Pow(pos1.x - ballPosX, 2));
                    if (y1 > tempMaxY)
                        tempMaxY = y1;
                }

                if (pos2.x > ball_minX && pos2.x < ball_maxX)
                {
                    float y2 = pos2.y + ballRadius;// Mathf.Sqrt(Mathf.Pow(ballRadius, 2) - Mathf.Pow(pos2.x - ballPosX, 2));
                    if (y2 > tempMaxY)
                        tempMaxY = y2;
                }

                // find the max Y
                if (tempMaxY > ballPosY)
                    ballPosY = tempMaxY;
            }
        }
        ballPosY += m_canvas.roadParent.transform.position.y;

        return ballPosY;
    }

}
