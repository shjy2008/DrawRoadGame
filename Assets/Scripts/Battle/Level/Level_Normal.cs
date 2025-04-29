using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Level_Normal : BattleLevel
{
    private GameObject m_finishLineObj = null;
    private bool m_hasCheckedHasKey = false; // Has checked whether this level has a chest key, if has, create it before the finishLine
    private int m_level;
    private float m_finishDistance;
    private bool m_isBonusLevel;

    public Level_Normal(BattleCanvas canvas, int level) : base(canvas)
    {
        m_level = level;

        m_isBonusLevel = (m_level == Const.bonus_levelId);

        List<Table_generator.Data> generatorList = new List<Table_generator.Data>();

        if (m_isBonusLevel)
        {
            // coins 5s -> coins 5s -> coins 5s
            List<Table_generator.Data> generator_coins = new List<Table_generator.Data>();
            int curLevel = LevelData.GetCurLevel();
            foreach (Table_generator.Data data in Table_generator.data.Values)
            {
                if (data.appearLevel != 0 && data.appearType == Const.GeneratorAppearType.BonusCoin && curLevel >= data.appearLevel &&
                    (data.maxLevel == 0 || curLevel <= data.maxLevel))
                {
                    generator_coins.Add(data);
                }
            }

            int stepCount = 3;
            for (int i = 0; i < stepCount; ++i)
            {
                generatorList.Add(RandomUtils.ListChoiceOne(generator_coins));
            }
        }
        else
        {
            // block 5s -> coin 3s -> block 5s ...
            int stepCount = 0;
            float speedScale = 1.0f;
            foreach (Table_normal_level.Data data in Table_normal_level.data.Values)
            {
                if (m_level >= data.level[0] && m_level <= data.level[1])
                {
                    stepCount = data.step;
                    speedScale = data.speedScale;
                }
            }

            m_canvas.SetSpeedScale(speedScale);

            int coinCount = stepCount / 2;
            int blockCount = stepCount - coinCount;

            List<Table_generator.Data> generator_blocks = new List<Table_generator.Data>();
            List<Table_generator.Data> generator_coins = new List<Table_generator.Data>();
            foreach (Table_generator.Data data in Table_generator.data.Values)
            {
                if (data.appearLevel != 0 && m_level >= data.appearLevel && (data.maxLevel == 0 || m_level <= data.maxLevel))
                {
                    if (data.appearType == Const.GeneratorAppearType.Block)
                    {
                        generator_blocks.Add(data);
                    }
                    else if (data.appearType == Const.GeneratorAppearType.RelaxCoin)
                    {
                        generator_coins.Add(data);
                    }
                }
            }
            // Random generator
            List<Table_generator.Data> choose_blocks = RandomUtils.GetRandomFromList(generator_blocks, blockCount);
            List<Table_generator.Data> choose_coins = RandomUtils.GetRandomFromList(generator_coins, coinCount);
            //float checkpointTime = 0.0f;
            for (int i = 0; i < coinCount; ++i)
            {
                generatorList.Add(choose_blocks[i]);
                generatorList.Add(choose_coins[i]);

                //checkpointTime += choose_blocks[i].time + choose_coins[i].time;
                //BlockGenerator generator = new BlockGenerator_Checkpoint(canvas); 
                //m_generatorDict.Add(new float[] { checkpointTime, checkpointTime + 0.5f }, generator);
            }
            if (choose_blocks.Count > choose_coins.Count)
            {
                generatorList.Add(choose_blocks[choose_blocks.Count - 1]);
            }
        }

        // FOR TEST
        if (Const_test.testGenerator != "")
        {
            int count = generatorList.Count;
            generatorList.Clear();
            for (int i = 0; i < count; ++i)
            {
                generatorList.Add(Table_generator.data[Const_test.testGenerator]);
            }
        }


        float distance = 0.0f;
        foreach (Table_generator.Data data in generatorList)
        {
            string generatorKey = data.key;
            BlockGenerator generator = BlockGeneratorFactory.GetGeneratorByKey(m_canvas, generatorKey);
            float startDistance = distance;
            float endDistance = distance + data.distance;
            m_generatorDict.Add(new float[] { startDistance, endDistance }, generator);

            // Block coin
            if (data.coinGenerator != "")
            {
                BlockGenerator coinGen = BlockGeneratorFactory.GetGeneratorByKey(m_canvas, data.coinGenerator);
                m_generatorDict.Add(new float[] { startDistance, endDistance }, coinGen);
            }

            distance = endDistance + Const.blocksGap; // Gap between two generators
        }

        // Finish distance
        m_finishDistance = distance + 3.0f;
    }

    public override void Destroy()
    {
        base.Destroy();
        if (m_finishLineObj != null)
        {
            UnityEngine.Object.Destroy(m_finishLineObj);
            m_finishLineObj = null;
        }
    }

    public override void Update()
    {
        base.Update();

        // ChestKey
        if (m_distance > (m_finishDistance - 4.0f) && !m_hasCheckedHasKey)
        {
            m_hasCheckedHasKey = true;
            bool hasChestKey = !m_isBonusLevel && (m_level % Const.bonusLevelGap == 0);
            if (hasChestKey)
            {
                float w = 1.5f;
                float gapHeight = 4.0f;
                float y = 0.0f;
                Vector2 blockPos = new Vector2(Const.rightX + w / 2, 0);
                BattleBlock block = new BattleBlock(m_canvas, blockPos);
                block.AddShape(new BattleBlock_UpDownRects(w, y, gapHeight));
                block.Update();
                m_canvas.AddBlock(block);

                m_canvas.CreateChestKey(y, true, Const.rightX + w / 2);
            }
        }

        // finish line
        if (m_distance > m_finishDistance && m_finishLineObj == null)
        {
            float finishLineWidth = 0.5f;
            GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Item3D/FinishLine"));
            obj.name = "FinishLine";
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = new Vector2(finishLineWidth, Const.topY - Const.bottomY);
            //BoxCollider2D bc2d = obj.GetComponent<BoxCollider2D>();
            //bc2d.size = sr.bounds.size;
            BoxCollider bc = obj.GetComponent<BoxCollider>();
            bc.size = new Vector3(finishLineWidth, Const.topY - Const.bottomY, 100);

            obj.transform.position = new Vector3(Const.rightX + finishLineWidth / 2, 0, Const.roadWidth / 2 + 0.01f);
            obj.transform.SetParent(m_canvas.blockParent.transform);
            m_finishLineObj = obj;
        }

        if (m_finishLineObj != null)
        {
            Vector3 prevPos = m_finishLineObj.transform.position;
            m_finishLineObj.transform.position = new Vector3(prevPos.x - Time.deltaTime * m_canvas.GetMoveSpeed(), prevPos.y, prevPos.z);
        }
    }

    public override void OnBattleEnded(bool isWin)
    {
        if (isWin)
        {
            if (!IsBonusLevel())
            {
                LevelData.SetPassedLevel(m_level);
            }
        }
    }

    protected override float GetFinishDistance()
    {
        return m_finishDistance;
    }

    public override GameObject GetFinishLineObj()
    {
        return m_finishLineObj;
    }

    public override bool IsBonusLevel()
    {
        return m_isBonusLevel;
    }

}
