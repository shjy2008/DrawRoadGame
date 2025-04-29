using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : BasePanel
{
    public static PausePanel instance;

    PausePanel()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnOpenPanel()
    {
        base.OnOpenPanel();
        Time.timeScale = 0;
    }

    protected override void OnClosePanel()
    {
        base.OnClosePanel();
        Time.timeScale = 1;
    }

    public void OnCloseBtnClk()
    {
        ClosePanel();
    }

    public void OnHomeBtnClk()
    {
        ClosePanel();
        MainPanel.instance.BackToMainPanel();
        BattlePanel.instance.ResetPanel();
    }

    public void OnNoAdsBtnClk()
    {

    }
}
