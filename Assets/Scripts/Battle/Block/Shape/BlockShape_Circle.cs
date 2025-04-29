using System;
using Shapes;
using UnityEngine;

public class BlockShape_Circle : BlockShape
{
    float m_radius;

    public BlockShape_Circle(float radius)
    {
        m_radius = radius;
    }

    public override void OnCreate()
    {
        GameObject prefab = m_entity.GetCanvas().blockCirclePrefab;
        GameObject obj = CreateGameObj(prefab, m_entity.GetCanvas().blockParent.transform);
        //SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        //sr.drawMode = SpriteDrawMode.Sliced;
        //sr.size = new Vector2(m_radius * 2, m_radius * 2);
        //CircleCollider2D cc2d = obj.GetComponent<CircleCollider2D>();
        //cc2d.radius = m_radius;
        
        //obj.transform.localScale = new Vector3(m_radius * 2, m_radius * 2, m_radius * 2); // For sphere

        obj.transform.localScale = new Vector3(m_radius * 2, obj.transform.localScale.y, m_radius * 2);

        m_objList.Add(obj);

    }

    //public override bool CheckCanRemove()
    //{
    //    return m_entity.pos.x + m_radius < Const.leftX;
    //}

    //public override bool CollideWithBall(float cx, float cy, float radius)
    //{
    //    return MathKit.IsCircleCrossCircle(new Vector2(cx, cy), radius, m_entity.pos, m_radius);
    //}
}
