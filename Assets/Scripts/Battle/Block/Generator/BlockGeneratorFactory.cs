using System;
using UnityEngine;

public class BlockGeneratorFactory
{
    public BlockGeneratorFactory()
    {
    }

    public static BlockGenerator GetGeneratorByKey(BattleCanvas canvas, string key)
    {
        Table_generator.data.TryGetValue(key, out Table_generator.Data data);
        BlockGenerator generator = null;
        switch (data.type)
        {
            case "circle":
                {
                    generator = new BlockGenerator_Circle(canvas, data.deltaX, data.radius, data.posY);
                }
                break;
            case "rect":
                {
                    generator = new BlockGenerator_Rect(canvas, data.deltaX, new Vector2(data.width, data.height), data.posY);
                }
                break;
            case "updownrect":
                {
                    generator = new BlockGenerator_UpDownRect(canvas, data.deltaX, data.width, data.gapHeight, data.posY, data.noCoin);
                }
                break;
            case "updowncircle":
                {
                    generator = new BlockGenerator_UpDownCircle(canvas, data.deltaX, data.radius, data.gapHeight, data.posY);
                }
                break;
            case "circlewave":
                {
                    generator = new BlockGenerator_CircleWave(canvas, data.deltaX, data.radius, data.gapHeight, data.posY);
                }
                break;
            case "wave":
                {
                    generator = new BlockGenerator_Wave(canvas, data.width, data.frequencyScale, data.amplitudeScale, data.gapHeight, data.posY);
                }
                break;
            case "coin":
                {
                    generator = new BlockGenerator_Coin(canvas, data.deltaX, data.posY, data.coinCountY, data.frequencyScale, data.amplitudeScale, data.isCos);
                }
                break;
            case "checkpoint":
                {
                    generator = new BlockGenerator_Checkpoint(canvas);
                }
                break;
            default:
                break;
        }
        if (generator != null)
        {
            if (data.action1 != "")
            {
                generator.AddActionKey(data.action1);
            }
            if (data.action2 != "")
            {
                generator.AddActionKey(data.action2);
            }
        }
        return generator;
    }
}
