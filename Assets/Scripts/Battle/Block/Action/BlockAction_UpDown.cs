using System;
using UnityEngine;

public class BlockAction_UpDown : BlockAction
{
    private float m_topY;
    private float m_bottomY;
    private float m_speed;
    private bool m_movingUp;

    public BlockAction_UpDown(float topY, float bottomY, float speed, bool movingUp)
    {
        m_topY = topY;
        m_bottomY = bottomY;
        m_speed = speed;
        m_movingUp = movingUp;
    }

    public override void Update()
    {
        float diff = m_speed * Time.deltaTime * m_entity.GetCanvas().GetSpeedScale();
        if (m_movingUp)
        {
            float y = m_entity.pos.y + diff;
            if (y > m_topY)
            {
                y = m_topY;
                m_movingUp = false;
            }
            m_entity.pos.y = y;
        }
        else
        {
            float y = m_entity.pos.y - diff;
            if (y < m_bottomY)
            {
                y = m_bottomY;
                m_movingUp = true;
            }
            m_entity.pos.y = y;
        }
    }
}
