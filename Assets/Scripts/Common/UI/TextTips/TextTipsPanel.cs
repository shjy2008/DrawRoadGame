using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTipsPanel : BasePanel
{
    public static TextTipsPanel instance;

    public GameObject objPrefab;

    private List<GameObject> m_objList = new List<GameObject>();

    TextTipsPanel()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(string text)
    {
        GameObject obj = Instantiate(objPrefab, objPrefab.transform.parent);
        obj.SetActive(true);
        obj.transform.Find("Text").GetComponent<Text>().text = text;

        //foreach (GameObject prevObj in m_objList)
        //{
        //    prevObj.transform.position = new Vector2(prevObj.transform.position.x, prevObj.transform.position.y + 110.0f);
        //}

        if (m_objList.Count == 0)
        {
            OpenPanel();
        }

        foreach (GameObject prevObj in m_objList)
        {
            Destroy(prevObj);
        }
        m_objList.Clear();

        m_objList.Add(obj);

        IEnumerator _()
        {
            yield return new WaitForSeconds(3.0f);

            string aniName = "TextTips_fadeout";
            obj.GetComponent<Animator>().Play(aniName);

            float fadeoutTime = UiUtil.GetAnimLength(obj, aniName);
            IEnumerator _1()
            {
                yield return new WaitForSeconds(fadeoutTime);
                m_objList.Remove(obj);
                Destroy(obj);
                if (m_objList.Count == 0)
                {
                    ClosePanel();
                }
            }
            StartCoroutine(_1());
        }
        StartCoroutine(_());
    }
}
