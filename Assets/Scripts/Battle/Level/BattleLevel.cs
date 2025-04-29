using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevel
{
    protected BattleCanvas m_canvas;
    protected float m_distance;
    protected Dictionary<float[], BlockGenerator> m_generatorDict = new Dictionary<float[], BlockGenerator>();
    protected List<BlockGenerator> m_currGeneratorList = new List<BlockGenerator>();

    public BattleLevel(BattleCanvas canvas)
    {
        m_canvas = canvas;
    }

    public virtual void Update()
    {
        // Distance
        m_distance += Time.deltaTime * m_canvas.GetMoveSpeed();

        foreach (KeyValuePair<float[], BlockGenerator> data in m_generatorDict)
        {
            BlockGenerator generator = data.Value;
            if (m_distance > data.Key[0])
            {
                if (!m_currGeneratorList.Contains(generator))
                {
                    m_currGeneratorList.Add(generator);
                    m_canvas.AddBlockGenerator(generator);
                    if (Const_test.showLevelLine)
                    {
                        m_canvas.CreateDebugLine(true);
                    }
                    break;
                }
            }
            if (m_distance > data.Key[1])
            {
                if (m_currGeneratorList.Contains(generator))
                {
                    m_currGeneratorList.Remove(generator);
                    m_generatorDict.Remove(data.Key);
                    m_canvas.RemoveBlockGenerator(generator);
                    if (Const_test.showLevelLine)
                    {
                        m_canvas.CreateDebugLine(false);
                    }
                    break;
                }
            }
        }
    }

    public BattleCanvas GetCanvas()
    {
        return m_canvas;
    }

    // only for Level_Normal, not for endless mode
    public float GetPercent()
    {
        float curDistance = m_distance;
        float totalDistance = GetFinishDistance() + (Const.rightX - Const.ballPosX);
        return curDistance / totalDistance;
    }

    protected virtual float GetFinishDistance()
    {
        return 0.0f;
    }

    public virtual GameObject GetFinishLineObj()
    {
        return null;
    }

    public virtual void Destroy()
    {
        Time.timeScale = 1.0f;
    }

    public virtual void OnBattleEnded(bool isWin)
    {

    }

    public virtual bool IsBonusLevel()
    {
        return false;
    }
}
