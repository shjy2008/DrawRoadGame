using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoad_Image : BattleRoad
{

    public BattleRoad_Image(BattleCanvas canvas, int roadId) : base(canvas, roadId)
    {

    }

    protected override void CreateRectObj(Vector2 pos1, Vector2 pos2)
    {
        base.CreateRectObj(pos1, pos2);

        List<Vector2> posList = new List<Vector2>();
        float imageMinDistance = m_lineWidth;
        if (m_objList.Count == 0)
        {
            posList.Add(pos1);
            Vector2 diff = (pos2 - pos1).normalized * imageMinDistance;
            int count = Convert.ToInt32((pos2 - pos1).magnitude / imageMinDistance) + 1;
            for (int i = 0; i < count; ++i)
            {
                posList.Add(pos1 + diff * i);
            }
        }
        else
        {
            Vector2 lastPos = m_objList[m_objList.Count - 1].transform.localPosition;
            float tryElapse = 0.01f;
            int tryCount = Convert.ToInt32((pos2 - pos1).magnitude / tryElapse);
            Vector2 tryDiff = (pos2 - pos1).normalized * tryElapse;
            Vector2 diff = (pos2 - pos1).normalized * imageMinDistance;
            for (int i = 0; i < tryCount; ++i)
            {
                Vector2 originPos = pos1 + tryDiff * i;
                if ((originPos - lastPos).magnitude > imageMinDistance)
                {
                    int count = Convert.ToInt32((pos2 - originPos).magnitude / imageMinDistance) + 1;
                    for (int j = 0; j < count; ++j)
                    {
                        posList.Add(originPos + diff * j);
                    }
                    break;
                }
            }
        }

        foreach (Vector2 pos in posList)
        {
            // rect
            GameObject obj = UnityEngine.Object.Instantiate(m_roadRectPrefab, m_canvas.roadParent.transform);
            obj.SetActive(true);
            obj.AddComponent<BattleRoadObjMono>();

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = new Vector2(m_lineWidth, m_lineWidth);

            obj.transform.localPosition = pos;
            Vector2 diff = pos2 - pos1;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            obj.transform.localRotation = Quaternion.Euler(0, 0, angle);

            obj.GetComponent<Animator>().Play("ImageRoad_show");

            m_objList.Add(obj);
        }
    }

}
