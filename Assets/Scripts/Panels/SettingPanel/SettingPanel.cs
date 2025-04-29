using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel
{
    public static SettingPanel instance;

    SettingPanel()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSoundBtn();
        UpdateVibrationBtn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSoundBtn()
    {
        gameObject.transform.Find("Bg/SoundBtn/Disabled").gameObject.SetActive(!PlayerData.enableSound);
    }

    private void UpdateVibrationBtn()
    {
        gameObject.transform.Find("Bg/VibrationBtn/Disabled").gameObject.SetActive(!PlayerData.enableVibration);
    }

    public void OnSoundBtnClick()
    {
        PlayerData.SetSoundEnabled(!PlayerData.enableSound);
        UpdateSoundBtn();
    }

    public void OnVibrationBtnClick()
    {
        PlayerData.SetVibrationEnabled(!PlayerData.enableVibration);
        UpdateVibrationBtn();
    }

    public void OnPrivacyPolicyBtnClick()
    {
        // TODO
    }

}
