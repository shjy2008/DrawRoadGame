using UnityEngine;
using System.Collections;

public class BtnScaleLoop : MonoBehaviour
{
    private Animator m_animator;

    // Use this for initialization
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();

        PlayLoopAction();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("BtnScaleReset"))
        {
            if (info.normalizedTime >= 1.0f) // An animation has finished playing
            {
                PlayLoopAction();
            }
        }
    }

    public void OnBecameVisible()
    {
        PlayLoopAction();
    }

    public void OnEnable()
    {
        PlayLoopAction();
    }

    //public void On

    private void PlayLoopAction()
    {
        gameObject.GetComponent<Animator>().Play("BtnScaleLoop");
    }
}
