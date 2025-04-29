using System;
using UnityEngine;

public class BlockGenerator_Wave : BlockGenerator
{
    private float m_tempRadian;
    private const float m_coinXDiff = 1.0f;
    private BattleBlock m_prevBlock;

    float m_blockWidth;
    float m_frequencyScale;
    float m_amplitudeScale;
    float m_gapHeight;
    float m_posY;

    public BlockGenerator_Wave(BattleCanvas canvas, float blockWidth, float frequencyScale, float amplitudeScale, float gapHeight, float posY) : base(canvas)
    {
        m_tempRadian = 0.0f;
        m_prevBlock = null;

        m_blockWidth = blockWidth;
        m_frequencyScale = frequencyScale;
        m_amplitudeScale = amplitudeScale;
        m_gapHeight = gapHeight;
        m_posY = posY;
    }

    public override void Update()
    {
        float moveSpeed = m_canvas.GetMoveSpeed();
        m_tempRadian += Time.deltaTime * moveSpeed * m_frequencyScale;
        float y = m_posY < Const.blockChangeToRandom ? UnityEngine.Random.Range(Const.blockRandomYMin, Const.blockRandomYMax) : m_posY;
        y += Mathf.Sin(m_tempRadian) * m_amplitudeScale;

        float blockWidth = m_blockWidth;
        float genTime = blockWidth / moveSpeed;
        m_timer += Time.deltaTime;
        if (m_timer > genTime - Time.deltaTime)
        {
            m_timer = 0.0f;
            BattleBlock block = GenerateBlock_UpDownRects(blockWidth, y, m_gapHeight);
            if (m_prevBlock != null)
            {
                block.pos.x = m_prevBlock.pos.x + blockWidth;
            }
            m_prevBlock = block;
        }
    }
}
