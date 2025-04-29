using System;
using Shapes;
using UnityEngine;

public class BlockShape_Rect : BlockShape
{
    Vector2 m_size;

    public BlockShape_Rect(Vector2 size)
    {
        m_size = size;
    }

    public override void OnCreate()
    {
        GameObject prefab = m_entity.GetCanvas().blockRectPrefab;
        GameObject obj = CreateGameObj(prefab, m_entity.GetCanvas().blockParent.transform);
        //SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        //sr.drawMode = SpriteDrawMode.Tiled;
        //sr.size = m_size;
        //BoxCollider2D bc2d = obj.GetComponent<BoxCollider2D>();
        //bc2d.size = sr.bounds.size;

        obj.transform.localScale = new Vector3(m_size.x, m_size.y, obj.transform.localScale.z);

        m_objList.Add(obj);
    }

    //public override bool CheckCanRemove()
    //{
    //    return m_entity.pos.x + m_size.x / 2 < Const.leftX;
    //}

    //public override bool CollideWithBall(float cx, float cy, float radius)
    //{
    //    return MathKit.IsCircleCrossRect(cx, cy, radius, m_entity.pos.x, m_entity.pos.y, m_size.x, m_size.y, 0);
    //}
}
