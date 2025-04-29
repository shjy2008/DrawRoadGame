using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoad3D_Continuous : BattleRoad3D
{
    public BattleRoad3D_Continuous(BattleCanvas canvas, int roadId) : base(canvas, roadId)
    {

    }

    protected override void CreateRectObj(Vector2 pos1, Vector2 pos2)
    {
        base.CreateRectObj(pos1, pos2);

        // rect
        //GameObject obj = UnityEngine.Object.Instantiate(m_roadRectPrefab, m_canvas.roadParent.transform);
        //obj.SetActive(true);
        //obj.AddComponent<BattleRoadObjMono>();
        //SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        //sr.drawMode = SpriteDrawMode.Tiled;
        //sr.size = new Vector2((pos1 - pos2).magnitude, m_lineWidth);
        //obj.transform.localPosition = (pos1 + pos2) / 2;
        Vector2 diff = pos2 - pos1;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //obj.transform.localRotation = Quaternion.Euler(0, 0, angle);
        //m_objList.Add(obj);


        GameObject obj = UnityEngine.Object.Instantiate(m_roadRectPrefab, m_canvas.roadParent.transform);
        obj.SetActive(true);
        obj.transform.localPosition = (pos1 + pos2) / 2;
        obj.transform.localScale = new Vector3((pos1 - pos2).magnitude, m_lineWidth, Const.roadWidth);
        obj.transform.localRotation = Quaternion.Euler(0, 0, angle);
        m_objList.Add(obj);

        // circle
        //GameObject obj_circle = UnityEngine.Object.Instantiate(m_roadCirclePrefab, m_canvas.roadParent.transform);
        //obj_circle.SetActive(true);
        //obj_circle.AddComponent<BattleRoadObjMono>();
        //SpriteRenderer sr_circle = obj_circle.GetComponent<SpriteRenderer>();
        //sr_circle.size = new Vector2(m_lineWidth, m_lineWidth);
        //obj_circle.transform.localRotation = Quaternion.Euler(0, 0, angle);
        //obj_circle.transform.localPosition = pos2;
        //m_objList.Add(obj_circle);

        GameObject obj_circle = UnityEngine.Object.Instantiate(m_roadCirclePrefab, m_canvas.roadParent.transform);
        obj_circle.SetActive(true);
        obj_circle.transform.localScale = new Vector3(m_lineWidth, Const.roadWidth / 2, m_lineWidth);
        obj_circle.transform.localPosition = new Vector3(pos2.x, pos2.y, obj_circle.transform.localPosition.z);
        m_objList.Add(obj_circle);

    }
}
