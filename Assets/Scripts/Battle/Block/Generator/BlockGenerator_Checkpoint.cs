using System;
using UnityEngine;

public class BlockGenerator_Checkpoint : BlockGenerator
{
    private bool m_hasCreated;

    public BlockGenerator_Checkpoint(BattleCanvas canvas) : base(canvas)
    {
        m_hasCreated = false;
    }

    public override void Update()
    {
        if (!m_hasCreated)
        {
            m_hasCreated = true;
            m_canvas.CreateCheckpoint();
        }
    }
}
