using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestKey : MonoBehaviour
{
    private List<GameObject> m_chestKeyObjList;

    // Use this for initialization
    void Start()
    {
        m_chestKeyObjList = new List<GameObject>();
        InitChestKey();

        Event.AddEventListener(Event.Type.OnGainChestKey, OnGainChestKey);
        Event.AddEventListener(Event.Type.OnUseAllChestKey, OnUseAllChestKey);
    }

    private void OnDestroy()
    {
        Event.RemoveEventListener(Event.Type.OnGainChestKey, OnGainChestKey);
        Event.RemoveEventListener(Event.Type.OnUseAllChestKey, OnUseAllChestKey);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitChestKey()
    {
        GameObject chestKeyParent = gameObject;
        GameObject keyObjPrefab = chestKeyParent.transform.Find("KeyObjPrefab").gameObject;
        m_chestKeyObjList.Add(keyObjPrefab);
        Vector2 originPos = keyObjPrefab.transform.localPosition;
        float keyOffsetX = chestKeyParent.transform.Find("Offset").gameObject.GetComponent<RectTransform>().rect.size.x;
        for (int i = 1; i < Const.maxChestKey; ++i)
        {
            GameObject keyObj = Instantiate(keyObjPrefab, chestKeyParent.transform);
            keyObj.transform.localPosition = new Vector2(originPos.x + i * keyOffsetX, originPos.y);
            m_chestKeyObjList.Add(keyObj);
        }

        UpdateChestKey(false);
    }

    private void OnGainChestKey()
    {
        UpdateChestKey(true);
    }

    private void OnUseAllChestKey()
    {
        UpdateChestKey(false);
    }

    private void UpdateChestKey(bool doAction)
    {
        for (int i = 0; i < Const.maxChestKey; ++i)
        {
            bool have = i < PlayerData.chestKeyNum;
            m_chestKeyObjList[i].transform.Find("Have").gameObject.SetActive(have);
            m_chestKeyObjList[i].transform.Find("NotHave").gameObject.SetActive(!have);
        }

        if (doAction)
        {
            GameObject obj = m_chestKeyObjList[PlayerData.chestKeyNum - 1];
            obj.GetComponent<Animator>().Play("CoinAnim", 0, 0);
        }
    }

}
