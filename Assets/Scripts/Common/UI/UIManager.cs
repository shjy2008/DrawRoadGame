using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject invisibleParent;

    public UIManager()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        invisibleParent = GameObject.Find("InvisibleParent");
        //invisibleParent.SetActive(false); // Maybe some bug with SetActive, use Canvas Group instead.
    }

    // Update is called once per frame
    void Update()
    {

    }
}
