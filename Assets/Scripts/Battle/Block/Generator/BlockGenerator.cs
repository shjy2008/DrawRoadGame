using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator
{
    protected BattleCanvas m_canvas;
    protected float m_timer;
    private List<string> m_actionKeyList;

    protected BlockGenerator(BattleCanvas canvas)
    {
        m_canvas = canvas;
        m_timer = 100000.0f; // generate immediately, don't wait m_timeInterval
        m_actionKeyList = new List<string>();
    }

    public virtual void Update()
    {

    }

    public void AddActionKey(string key)
    {
        m_actionKeyList.Add(key);
    }

    private BattleBlock CreateBlock(Vector2 blockPos)
    {
        BattleBlock block = new BattleBlock(m_canvas, blockPos);
        foreach (string actionKey in m_actionKeyList)
        {
            block.AddAction(BlockActionFactory.GetActionByKey(actionKey));
        }
        m_canvas.AddBlock(block);
        return block;
    }

    protected BattleBlock GenerateBlock_Rect(Vector2 size, float midPosY)
    {
        Vector2 blockPos = new Vector2(Const.rightX + size.x / 2, midPosY);
        BattleBlock block = CreateBlock(blockPos);
        block.AddShape(new BlockShape_Rect(size));
        return block;
    }

    protected BattleBlock GenerateBlock_Circle(float radius, float midPosY)
    {
        Vector2 blockPos = new Vector2(Const.rightX + radius, midPosY);
        BattleBlock block = CreateBlock(blockPos);
        block.AddShape(new BlockShape_Circle(radius));
        return block;
    }

    protected BattleBlock GenerateBlock_UpDownRects(float w, float gapMidY, float gapHeight)
    {
        Vector2 blockPos = new Vector2(Const.rightX + w / 2, 0);
        BattleBlock block = CreateBlock(blockPos);
        block.AddShape(new BattleBlock_UpDownRects(w, gapMidY, gapHeight));
        return block;
    }
}
