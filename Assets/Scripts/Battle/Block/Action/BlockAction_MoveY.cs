using System;
using UnityEngine;

public class BlockAction_MoveY : BlockAction
{
    private float m_speed;
    private bool m_movingUp;

    public BlockAction_MoveY(float speed, bool movingUp)
    {
        m_speed = speed;
        m_movingUp = movingUp;
    }

    public override void Update()
    {
        float diff = m_speed * Time.deltaTime * m_entity.GetCanvas().GetSpeedScale();
        if (m_movingUp)
        {
            m_entity.pos.y += diff;
        }
        else
        {
            m_entity.pos.y -= diff;
        }
    }
}
