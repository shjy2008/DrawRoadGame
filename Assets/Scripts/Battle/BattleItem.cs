using System;
using System.Collections.Generic;
using UnityEngine;

// key or coin
public class BattleItem : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBecameInvisible()
    {
        BattleScene.instance.GetCanvas().RemoveItem(gameObject);
    }
}

