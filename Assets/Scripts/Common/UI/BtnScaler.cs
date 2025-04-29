using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnScaler : MonoBehaviour
{
    private RectTransform m_rectTransform;
    private Animator m_animator;
    private bool m_prevInRect;

    // Start is called before the first frame update
    void Start()
    {
        m_rectTransform = gameObject.GetComponent<RectTransform>();
        m_animator = gameObject.GetComponent<Animator>();
        m_prevInRect = false;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
    }

    private void OnDisable()
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (InputMgr.IsBegan())
            {
                m_animator.Play("BtnScale");
                //m_rectTransform.localScale = new Vector2(0.9f, 0.9f);
            }

            if (InputMgr.IsEnded())
            {
                if (m_prevInRect) // If it is outside, it was already reset when finger get outside the bounds
                {
                    m_animator.Play("BtnScaleReset");
                }
                //m_rectTransform.localScale = new Vector2(1.0f, 1.0f);
            }

            if (InputMgr.IsMoved())
            {
                Vector2 touchPos = InputMgr.GetTouchPosUI();
                Vector2 btnPos = gameObject.transform.position + new Vector3(Const.gameWidth / 2, Const.gameHeight / 2);
                Vector2 btnSize = m_rectTransform.rect.size;
                Vector2 btnLeftBottom = new Vector2(btnPos.x - m_rectTransform.pivot.x * btnSize.x, btnPos.y - m_rectTransform.pivot.y * btnSize.y);
                Rect rect = new Rect(btnLeftBottom, btnSize);
                bool inRect = rect.Contains(touchPos);

                if (!inRect && m_prevInRect)
                {
                    m_animator.Play("BtnScaleReset");
                    //m_rectTransform.localScale = new Vector2(1.0f, 1.0f);
                }
                else if (inRect && !m_prevInRect)
                {
                    m_animator.Play("BtnScale");
                    //m_rectTransform.localScale = new Vector2(0.9f, 0.9f);
                }
                m_prevInRect = inRect;
            }
        }
    }
}
