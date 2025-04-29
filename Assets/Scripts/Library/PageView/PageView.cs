using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class PageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect rect;                        //滑动组件  
    private float targethorizontal = 0;             //滑动的起始坐标  
    private bool isDrag = false;                    //是否拖拽结束  
    private List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始  
    private int currentPageIndex = -1;
    public List<Action> OnSetPageCountList = new List<Action>();
    public List<Action<int>> OnPageChangedList = new List<Action<int>>();

    private bool stopMove = true;
    public float smooting = 1;      //滑动速度  
    public float sensitivity = 0.5f;
    private float startTime;

    public PageViewIndex pageViewIndex;

    private float startDragHorizontal;

    private float m_beganPosX;

    void Awake()
    {
    }

    public void SetPageCount(int count)
    {
        rect = transform.GetComponent<ScrollRect>();
        float horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < count; i++)
        {
            posList.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
        }
        foreach (Action cb in OnSetPageCountList)
        {
            cb();
        }
        SetPageIndex(0);

        if (pageViewIndex != null)
        {
            pageViewIndex.OnSetPageCount(count);
        }
    }

    public int GetPageCount()
    {
        return posList.Count;
    }

    void Update()
    {
        if (!isDrag && !stopMove)
        {
            startTime += Time.deltaTime;
            float t = startTime * smooting;
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
            if (t >= 1)
                stopMove = true;
        }
    }

    public void PageTo(int index)
    {
        if (index >= 0 && index < posList.Count)
        {
            rect.horizontalNormalizedPosition = posList[index];
            SetPageIndex(index, doAction:true);
        }
        else
        {
            Debug.LogWarning("[error] PageView::PageTo index not exists:" + index);
        }
    }

    private void SetPageIndex(int index, bool doAction = false)
    {
        if (currentPageIndex != index)
        {
            currentPageIndex = index;
            foreach (Action<int> cb in OnPageChangedList)
            {
                cb(index);
            }
            if (pageViewIndex != null)
            {
                pageViewIndex.OnPageChanged(index);
            }
        }

        if (doAction)
        {
            targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
            isDrag = false;
            startTime = 0;
            stopMove = false;
        }
    }

    public void OnLeftBtnClk()
    {
        if (currentPageIndex > 0)
        {
            SetPageIndex(currentPageIndex - 1, doAction:true);
        }
    }

    public void OnRightBtnClk()
    {
        if (currentPageIndex < GetPageCount() - 1)
        {
            SetPageIndex(currentPageIndex + 1, doAction: true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_beganPosX = eventData.position.x;

        isDrag = true;
        startDragHorizontal = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float endPosX = eventData.position.x;
        float threshold = GetComponent<RectTransform>().rect.width * sensitivity;
        int index = currentPageIndex;
        if (endPosX - m_beganPosX > threshold)
        {
            index -= 1;
        }
        else if (endPosX - m_beganPosX < -threshold)
        {
            index += 1;
        }
        index = Mathf.Clamp(index, 0, posList.Count - 1);

        SetPageIndex(index, doAction:true);
    }
}
