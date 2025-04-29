using System;
public class BlockAction
{
    protected BattleBlock m_entity;

    protected BlockAction()
    {
    }

    public void SetEntity(BattleBlock entity)
    {
        m_entity = entity;
        OnCreate();
    }

    public virtual void OnCreate()
    {

    }

    public virtual void Update()
    {

    }
}
