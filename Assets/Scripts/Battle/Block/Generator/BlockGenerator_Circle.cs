using System;
using UnityEngine;

public class BlockGenerator_Circle : BlockGenerator
{
    float m_deltaX;
    float m_radius;
    float m_posY;

    public BlockGenerator_Circle(BattleCanvas canvas, float deltaX, float radius, float posY) : base(canvas)
    {
        m_deltaX = deltaX;
        m_radius = radius;
        m_posY = posY;
    }

    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_deltaX / m_canvas.GetMoveSpeed())
        {
            m_timer = 0.0f;
            float y = m_posY < Const.blockChangeToRandom ? UnityEngine.Random.Range(Const.blockRandomYMin, Const.blockRandomYMax) : m_posY;
            GenerateBlock_Circle(m_radius, y);
        }
    }
}
