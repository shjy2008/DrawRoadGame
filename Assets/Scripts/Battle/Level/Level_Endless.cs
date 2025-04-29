using System;
using System.Collections.Generic;
using UnityEngine;

public class Level_Endless : BattleLevel
{
    private List<Table_generator.Data> m_generator_blocks = new List<Table_generator.Data>();
    private List<Table_generator.Data> m_generator_coins = new List<Table_generator.Data>();
    private bool m_isBlock;
    private float m_nextGenDistance;
    protected float m_timer;

    public Level_Endless(BattleCanvas canvas) : base(canvas)
    {
        m_isBlock = true;
        m_nextGenDistance = 0;
        m_timer = 0.0f;

        foreach (Table_generator.Data data in Table_generator.data.Values)
        {
            if (data.appearLevel != 0 && data.maxLevel == 0)
            {
                if (data.appearType == Const.GeneratorAppearType.Block)
                {
                    m_generator_blocks.Add(data);
                }
                else if (data.appearType == Const.GeneratorAppearType.RelaxCoin)
                {
                    m_generator_coins.Add(data);
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();

        // Speed
        m_timer += Time.deltaTime;
        float speedScale = 1.2f + m_timer * 0.01f;
        if (speedScale > Const.endlessMaxSpeedScale)
        {
            speedScale = Const.endlessMaxSpeedScale;
        }
        m_canvas.SetSpeedScale(speedScale);

        // Score
        int score = (int)(m_distance * 1.0f);
        m_canvas.SetScore(score);

        // Generator
        if (m_distance > m_nextGenDistance)
        {
            Table_generator.Data data;
            if (m_isBlock)
            {
                data = RandomUtils.ListChoiceOne(m_generator_blocks);
            }
            else
            {
                data = RandomUtils.ListChoiceOne(m_generator_coins);
            }

            // FOR TEST
            if (Const_test.testGenerator != "")
            {
                data = Table_generator.data[Const_test.testGenerator];
            }

            BlockGenerator generator = BlockGeneratorFactory.GetGeneratorByKey(m_canvas, data.key);
            float startDistance = m_distance;
            float endDistance = m_distance + data.distance;
            m_generatorDict.Add(new float[] { startDistance, endDistance }, generator);

            // Block coin
            if (data.coinGenerator != "")
            {
                BlockGenerator coinGen = BlockGeneratorFactory.GetGeneratorByKey(m_canvas, data.coinGenerator);
                m_generatorDict.Add(new float[] { startDistance, endDistance }, coinGen);
            }

            m_nextGenDistance = endDistance + Const.blocksGap;
            m_isBlock = !m_isBlock;
        }
    }
}
