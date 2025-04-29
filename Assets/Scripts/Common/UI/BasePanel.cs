using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePanel : MonoBehaviour
{
    private List<BasePanel> m_subPanelList;
    private Transform m_prevParentTransform;
    private bool m_isClosing;
    private bool m_hasBeenCreated;
    private int m_sortZOrder;

    public BasePanel()
    {
        m_subPanelList = new List<BasePanel>();
        m_prevParentTransform = null;
        m_isClosing = false;
        m_hasBeenCreated = false;
    }

    protected void Awake()
    {
        m_hasBeenCreated = true;
        m_sortZOrder = gameObject.GetComponent<Canvas>().sortingOrder;
        OnOpenPanel();
    }

    public bool IsOpened()
    {
        return gameObject.activeSelf && gameObject.transform.parent != UIManager.instance.invisibleParent.transform;
    }

    public virtual void OpenPanel()
    {
        if (!IsOpened())
        {
            bool hasBeenCreated = m_hasBeenCreated; // SetActive may call Awake(), so make a backup temporarily

            gameObject.transform.SetParent(m_prevParentTransform);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            gameObject.GetComponent<Canvas>().sortingOrder = m_sortZOrder;

            // If the panel is created the first time, OnOpenPanel will be call in OnStart()
            if (hasBeenCreated)
            {
                OnOpenPanel();
            }
        }
    }

    public virtual void ClosePanel()
    {
        if (IsOpened() && !m_isClosing)
        {
            // Close action
            if (gameObject.GetComponent<OpenCloseAction>())
            {
                Animator animator = gameObject.GetComponent<Animator>();
                string actionName = gameObject.GetComponent<OpenCloseAction>().closeActionName;
                animator.Play(actionName);
                m_isClosing = true;
                float length = UiUtil.GetAnimLength(gameObject, actionName);
                Invoke("ReallyClosePanel", length);
            }
            else // NO close action
            {
                ReallyClosePanel();
            }
        }
    }

    private void ReallyClosePanel()
    {
        // There are some bugs with animation after setting inactive and active.
        // Prevent setting inactive
        //gameObject.SetActive(false);
        m_prevParentTransform = gameObject.transform.parent;
        gameObject.transform.SetParent(UIManager.instance.invisibleParent.transform);
        OnClosePanel();
        m_isClosing = false;
        gameObject.GetComponent<Canvas>().sortingOrder = -1000;
    }

    protected virtual void OnOpenPanel()
    {
        // Open action
        if (gameObject.GetComponent<OpenCloseAction>())
        {
            gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<OpenCloseAction>().openActionName);
        }

        foreach (BasePanel panel in m_subPanelList)
        {
            panel.OnOpenPanel();
        }
    }

    protected virtual void OnClosePanel()
    {
        foreach (BasePanel panel in m_subPanelList)
        {
            panel.OnClosePanel();
        }
    }

    public virtual void ResetPanel()
    {

    }

    public void AddSubPanel(BasePanel panel)
    {
        m_subPanelList.Add(panel);
    }

    public void RemoveSubPanel(BasePanel panel)
    {
        m_subPanelList.Remove(panel);
    }
}
