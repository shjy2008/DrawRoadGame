using System;
using UnityEngine;

public class BlockGenerator_CircleWave : BlockGenerator
{
    float m_deltaX;
    float m_radius;
    float m_gapHeight;
    float m_posY;

    bool m_curUp;

    public BlockGenerator_CircleWave(BattleCanvas canvas, float deltaX, float radius, float gapHeight, float posY) : base(canvas)
    {
        m_deltaX = deltaX;
        m_radius = radius;
        m_gapHeight = gapHeight;
        m_posY = posY;

        m_curUp = false;
    }

    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_deltaX / m_canvas.GetMoveSpeed())
        {
            m_timer = 0.0f;
            float y = m_posY < Const.blockChangeToRandom ? UnityEngine.Random.Range(Const.blockRandomYMin, Const.blockRandomYMax) : m_posY;
            if (m_curUp)
            {
                float upCenterY = y + m_gapHeight / 2.0f + m_radius;
                BattleBlock block_up = GenerateBlock_Circle(m_radius, upCenterY);
            }
            else
            {
                float downCenterY = y - m_gapHeight / 2.0f - m_radius;
                BattleBlock block_down = GenerateBlock_Circle(m_radius, downCenterY);
            }

            m_curUp = !m_curUp;
        }
    }
}

