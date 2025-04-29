using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallItem : MonoBehaviour
{
    public GameObject selectObj;
    public GameObject imageObj;
    public GameObject questionObj;
    public GameObject randomObj;
    public GameObject imageBgObj;
    public GameObject lockedBgObj;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBallItemClk()
    {
        Event.PostEvent(Event.Type.OnBallItemClk, gameObject.name);
    }

    public void SetSelected(bool selected)
    {
        selectObj.SetActive(selected);
    }

    public void SetImage(string path)
    {
        Texture2D tex = Resources.Load<Texture2D>(path);
        imageObj.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public void SetLocked(bool isLocked)
    {
        questionObj.SetActive(isLocked);
        imageObj.SetActive(!isLocked);
        imageBgObj.SetActive(!isLocked);
        lockedBgObj.SetActive(isLocked);
    }

    public void SetRandoming(bool randoming)
    {
        randomObj.SetActive(randoming);
    }
}
