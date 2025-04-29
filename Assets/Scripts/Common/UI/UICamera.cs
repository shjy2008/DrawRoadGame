using UnityEngine;
using System.Collections;

public class UICamera : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        _AdjustCameraSize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // adjust resolution
    private void _AdjustCameraSize()
    {
        // FIXED HEIGHT

        //const float designRatio = Const.designHeight / Const.designWidth;
        float screenHeight = Const.designHeight;
        Camera cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = screenHeight / 2;

    }
}
