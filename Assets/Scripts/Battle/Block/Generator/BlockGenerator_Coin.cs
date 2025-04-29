using System;
using UnityEngine;

public class BlockGenerator_Coin : BlockGenerator
{
    private float m_tempRadian;

    float m_deltaX;
    float m_posY;
    float m_coin_count;
    float m_frequencyScale;
    float m_amplitudeScale;
    bool m_isCos;

    private const float yDiff = 1.0f;

    public BlockGenerator_Coin(BattleCanvas canvas, float deltaX, float posY, int coin_count, float frequencyScale, float amplitudeScale, bool isCos) : base(canvas)
    {
        m_deltaX = deltaX;
        m_posY = posY;
        m_coin_count = coin_count;
        m_frequencyScale = frequencyScale;
        m_amplitudeScale = amplitudeScale;
        m_isCos = isCos;
    }

    public override void Update()
    {
        float moveSpeed = m_canvas.GetMoveSpeed();
        m_tempRadian += Time.deltaTime * moveSpeed * m_frequencyScale;

        m_timer += Time.deltaTime;
        if (m_timer > m_deltaX / moveSpeed)
        {
            m_timer = 0.0f;
            //GenerateBlock_Circle(m_radius, UnityEngine.Random.Range(Const.bottomY, Const.topY));
            for (int i = 0; i < m_coin_count; ++i)
            {
                float y = m_posY - (m_coin_count - 1) / 2.0f * yDiff + yDiff * i;
                if (m_isCos)
                {
                    y += Mathf.Cos(m_tempRadian) * m_amplitudeScale;
                }
                else
                {
                    y += Mathf.Sin(m_tempRadian) * m_amplitudeScale;
                }
                m_canvas.CreateCoin(y);
            }
        }
    }
}
