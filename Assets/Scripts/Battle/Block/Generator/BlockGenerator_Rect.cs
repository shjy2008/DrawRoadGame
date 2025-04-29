using System;
using UnityEngine;

public class BlockGenerator_Rect : BlockGenerator
{
    float m_deltaX;
    Vector2 m_size;
    float m_posY;

    public BlockGenerator_Rect(BattleCanvas canvas, float deltaX, Vector2 size, float posY) : base(canvas)
    {
        m_deltaX = deltaX;
        m_size = size;
        m_posY = posY;
    }

    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_deltaX / m_canvas.GetMoveSpeed())
        {
            m_timer = 0.0f;
            float y = m_posY < Const.blockChangeToRandom ? UnityEngine.Random.Range(Const.blockRandomYMin, Const.blockRandomYMax) : m_posY;
            GenerateBlock_Rect(m_size, y);
        }
    }
}
