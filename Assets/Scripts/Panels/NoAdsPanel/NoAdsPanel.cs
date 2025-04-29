using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsPanel : BasePanel
{
    public static NoAdsPanel instance;

    NoAdsPanel()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Event.AddEventListener(Event.Type.OnNoAdsBought, OnNoAdsBought);
    }

    private void OnNoAdsBought()
    {
        ClosePanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
