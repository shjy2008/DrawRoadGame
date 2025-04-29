using System;
using UnityEngine;

public class BlockAction_Rotate : BlockAction
{
    private float m_anglePerSecond;

    public BlockAction_Rotate(float anglePerSecond)
    {
        m_anglePerSecond = anglePerSecond;
    }

    public override void Update()
    {
        m_entity.rotateAngle += m_anglePerSecond * Time.deltaTime * m_entity.GetCanvas().GetSpeedScale();
    }
}
