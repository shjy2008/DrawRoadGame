using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class BattleBlock
{
    protected BattleCanvas m_canvas;
    public Vector2 pos;
    public float rotateAngle;
    private List<BlockShape> m_shapeList;
    private List<BlockAction> m_actionList;

    public BattleBlock(BattleCanvas canvas, Vector2 pos)
    {
        m_canvas = canvas;
        this.pos = pos;
        rotateAngle = 0;
        m_shapeList = new List<BlockShape>();
        m_actionList = new List<BlockAction>();
    }

    public void Destroy()
    {
        foreach (BlockShape shape in m_shapeList)
        {
            shape.Destroy();
        }
    }

    public BattleCanvas GetCanvas()
    {
        return m_canvas;
    }

    //public bool CollideWithBall(float cx, float cy, float radius)
    //{
    //    foreach (BlockShape shape in m_shapeList)
    //    {
    //        if (shape.CollideWithBall(cx, cy, radius))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //private bool CheckCanRemove()
    //{
    //    foreach (BlockShape shape in m_shapeList)
    //    {
    //        if (!shape.CheckCanRemove())
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    public void Update()
    {
        float diff = Time.deltaTime * m_canvas.GetMoveSpeed();
        pos.x -= diff;

        //if (CheckCanRemove())
        //{
        //    m_canvas.RemoveBlock(this);
        //}

        foreach (BlockShape shape in m_shapeList)
        {
            List<GameObject> objList = shape.GetObjList();
            foreach (GameObject obj in objList)
            {
                obj.transform.position = new Vector3(pos.x, pos.y, obj.transform.position.z);
                Vector3 prevRot = obj.transform.localRotation.eulerAngles;
                obj.transform.localRotation = Quaternion.Euler(prevRot.x, prevRot.y, rotateAngle);
            }
        }

        foreach (BlockAction action in m_actionList)
        {
            action.Update();
        }
    }

    public void AddShape(BlockShape shape)
    {
        shape.SetEntity(this);
        m_shapeList.Add(shape);
    }

    public void AddAction(BlockAction action)
    {
        action.SetEntity(this);
        m_actionList.Add(action);
    }

}
