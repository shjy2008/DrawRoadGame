using System;
using UnityEngine;

public class BlockGenerator_UpDownRect : BlockGenerator
{
    float m_deltaX;
    float m_width;
    float m_gapHeight;
    float m_posY;
    bool m_noCoin;

    public BlockGenerator_UpDownRect(BattleCanvas canvas, float deltaX, float width, float gapHeight, float posY, bool noCoin) : base(canvas)
    {
        m_deltaX = deltaX;
        m_width = width;
        m_gapHeight = gapHeight;
        m_posY = posY;
        m_noCoin = noCoin;
    }

    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_deltaX / m_canvas.GetMoveSpeed())
        {
            m_timer = 0.0f;
            float y = m_posY < Const.blockChangeToRandom ? UnityEngine.Random.Range(Const.blockRandomYMin, Const.blockRandomYMax) : m_posY;
            BattleBlock block = GenerateBlock_UpDownRects(m_width, y, m_gapHeight);
            if (!m_noCoin)
            {
                m_canvas.CreateCoin(y, true, block.pos.x);
            }
        }
    }
}
