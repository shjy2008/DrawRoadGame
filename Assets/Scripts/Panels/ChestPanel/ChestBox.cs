using UnityEngine;
using System.Collections;

public class ChestBox : MonoBehaviour
{
    private float m_timer;
    private float m_nextActionTime;

    // Use this for initialization
    void Start()
    {
        CreateNextAction();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_nextActionTime)
        {
            gameObject.GetComponent<Animator>().Play("ChestBox", 0, 0);
            CreateNextAction();
        }
    }

    private void CreateNextAction()
    {
        m_timer = 0.0f;
        m_nextActionTime = 2.0f;//UnityEngine.Random.Range(2.0f, 3.0f);
    }
}
