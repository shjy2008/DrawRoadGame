using UnityEngine;
using System.Collections;

public class BlockShapeMonoBehaviour : MonoBehaviour
{
    public Color color;

    public BlockShape blockShape;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = color;
        //gameObject.GetComponent<MeshRenderer>().material.color = Const.blockColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBecameInvisible()
    {
        // Rotate rect may be removed on the right, this is not correct
        // So need to add a "<leftX" condition
        if (gameObject.transform.position.x < Const.leftX)
        {
            BattleBlock block = blockShape.GetEntity();
            block.GetCanvas().RemoveBlock(block);
        }
    }
}
