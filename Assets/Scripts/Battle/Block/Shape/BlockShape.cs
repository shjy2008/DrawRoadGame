using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockShape
{
    protected BattleBlock m_entity;
    protected List<GameObject> m_objList;

    protected BlockShape()
    {
        m_objList = new List<GameObject>();
    }

    public void Destroy()
    {
        foreach (GameObject obj in m_objList)
        {
            UnityEngine.Object.Destroy(obj);
        }
    }

    public void SetEntity(BattleBlock entity)
    {
        m_entity = entity;
        OnCreate();
    }

    protected GameObject CreateGameObj(GameObject prefab, Transform parent)
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefab, parent);
        obj.GetComponent<BlockShapeMonoBehaviour>().blockShape = this;
        return obj;
    }

    public BattleBlock GetEntity()
    {
        return m_entity;
    }

    public virtual void OnCreate()
    {

    }

    public List<GameObject> GetObjList()
    {
        return m_objList;
    }

    //public virtual bool CheckCanRemove()
    //{
    //    return false;
    //}

    //public virtual bool CollideWithBall(float cx, float cy, float radius)
    //{
    //    return false;
    //}

}
