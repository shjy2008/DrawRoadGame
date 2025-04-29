using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageViewIndex : MonoBehaviour
{
    public GameObject selectedImage;
    public GameObject emptyImage;
    //public PageView pageView;
    public float offsetX = 30;

    private List<GameObject> m_emptyImageList = new List<GameObject>();
    private int m_pageCount;
    private int m_curIndex;

    // Use this for initialization
    void Start()
    {
        selectedImage.SetActive(true);
        emptyImage.SetActive(false);
        //pageView.OnSetPageCountList.Add(OnSetPageCount);
        //pageView.OnPageChangedList.Add(OnPageChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSetPageCount(int pageCount)
    {
        m_pageCount = pageCount;
        for (int i = 0; i < pageCount - 1; ++i)
        {
            GameObject obj = Instantiate(emptyImage, gameObject.transform);
            obj.SetActive(true);
            m_emptyImageList.Add(obj);
        }

        m_curIndex = -1;
        SetCurIndex(0);
    }

    public void OnPageChanged(int index)
    {
        SetCurIndex(index);
    }

    public void SetCurIndex(int index)
    {
        if (m_curIndex != index)
        {
            m_curIndex = index;
            int j = 0;
            for (int i = 0; i < m_pageCount; ++i)
            {
                float x = -((m_pageCount - 1) / 2.0f) * offsetX + offsetX * i;
                if (i == m_curIndex)
                {
                    selectedImage.transform.localPosition = new Vector2(x, 0);
                }
                else
                {
                    m_emptyImageList[j].transform.localPosition = new Vector2(x, 0);
                    ++j;
                }
            }
        }
    }
}
