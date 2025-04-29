using System;
using Shapes;
using UnityEngine;

public class BattleBlock_UpDownRects : BlockShape
{
    float m_w;
    float m_gapMidY;
    float m_gapHeight;

    Vector2 m_ceil_pos_top;
    Vector2 m_ceil_pos_bottom;

    Vector2 m_floor_pos_top;
    Vector2 m_floor_pos_bottom;
    
    public BattleBlock_UpDownRects(float w, float gapMidY, float gapHeight)
    {
        m_w = w;
        m_gapMidY = gapMidY;
        m_gapHeight = gapHeight;
    }

    public override void OnCreate()
    {
        GameObject parent = new GameObject();

        CalcCeilFloorPos();
        // ceil
        GameObject prefab = m_entity.GetCanvas().blockRectPrefab;
        GameObject obj = CreateGameObj(prefab, parent.transform);
        //SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        //sr.drawMode = SpriteDrawMode.Tiled;
        //sr.size = new Vector2(m_w, m_ceil_pos_top.y - m_ceil_pos_bottom.y);
        //BoxCollider2D bc2d = obj.GetComponent<BoxCollider2D>();
        //bc2d.size = sr.bounds.size;
        obj.transform.localScale = new Vector3(m_w, m_ceil_pos_top.y - m_ceil_pos_bottom.y, obj.transform.localScale.z);
        obj.transform.localPosition = new Vector3(0, (m_ceil_pos_top.y + m_ceil_pos_bottom.y) / 2, obj.transform.localPosition.z);

        // bottom
        GameObject obj2 = CreateGameObj(prefab, parent.transform);
        //SpriteRenderer sr2 = obj2.GetComponent<SpriteRenderer>();
        //sr2.drawMode = SpriteDrawMode.Tiled;
        //sr2.size = new Vector2(m_w, m_floor_pos_top.y - m_floor_pos_bottom.y);
        //BoxCollider2D bc2d2 = obj2.GetComponent<BoxCollider2D>();
        //bc2d2.size = sr2.size;
        obj2.transform.localScale = new Vector3(m_w, m_floor_pos_top.y - m_floor_pos_bottom.y, obj.transform.localScale.z);
        obj2.transform.localPosition = new Vector3(0, (m_floor_pos_top.y + m_floor_pos_bottom.y) / 2, obj2.transform.localPosition.z);

        parent.transform.SetParent(m_entity.GetCanvas().blockParent.transform);
        m_objList.Add(parent);
    }

    private void CalcCeilFloorPos()
    {
        m_ceil_pos_top = new Vector2(m_entity.pos.x, Const.topY);
        m_ceil_pos_bottom = new Vector2(m_entity.pos.x, m_gapMidY + m_gapHeight / 2);
        m_floor_pos_top = new Vector2(m_entity.pos.x, m_gapMidY - m_gapHeight / 2);
        m_floor_pos_bottom = new Vector2(m_entity.pos.x, Const.bottomY); 
    }

    //public override bool CheckCanRemove()
    //{
    //    return m_entity.pos.x + m_w / 2 < Const.leftX;
    //}

    //public override bool CollideWithBall(float cx, float cy, float radius)
    //{
    //    CalcCeilFloorPos();
    //    return MathKit.IsCircleCrossLineSeg(cx, cy, radius, m_ceil_pos1, m_ceil_pos2, m_w) ||
    //        MathKit.IsCircleCrossLineSeg(cx, cy, radius, m_floor_pos1, m_floor_pos2, m_w);
    //}

}
