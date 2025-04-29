using UnityEngine;
using System.Collections;

public class ChestPanelReward : MonoBehaviour
{
    private bool m_hasOpened;

    // Use this for initialization
    void Start()
    {
        m_hasOpened = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (!m_hasOpened)
        {
            int index = int.Parse(gameObject.name);
            bool success = ChestPanel.instance.SelectRewardIndex(index);
            if (success)
            {
                m_hasOpened = true;
            }
        }
    }
}
