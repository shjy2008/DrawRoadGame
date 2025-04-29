using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoadItem : MonoBehaviour
{
    public GameObject selectObj;
    public GameObject imageObj;
    public GameObject questionObj;
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

    public void OnRoadItemClk()
    {
        Event.PostEvent(Event.Type.OnRoadItemClk, gameObject.name);
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
}
