using UnityEngine;
using System.Collections;

public class BattleRoadObjMono : MonoBehaviour
{
    //public Color color;

    // Use this for initialization
    void Start()
    {
        //gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        BattleScene.instance.GetCanvas().GetBattleRoad().RemoveObj(gameObject);
    }
}
