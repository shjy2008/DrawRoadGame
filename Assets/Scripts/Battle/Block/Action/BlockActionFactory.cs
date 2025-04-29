using System;

public class BlockActionFactory
{
    public BlockActionFactory()
    {

    }

    public static BlockAction GetActionByKey(string key)
    {
        Table_action.data.TryGetValue(key, out Table_action.Data data);
        switch (data.type)
        {
            case "moveY":
                {
                    return new BlockAction_MoveY(data.speed, data.movingUp);
                }
            case "updown":
                {
                    return new BlockAction_UpDown(data.topY, data.bottomY, data.speed, data.movingUp);
                }
            case "rotate":
                {
                    return new BlockAction_Rotate(data.anglePerSecond);
                }
            default:
                break;
        }
        return null;
    }
}
