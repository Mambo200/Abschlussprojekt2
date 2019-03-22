using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private GameObject m_Body;
    /// <summary>Invert X mouse input</summary>
    public bool invertX = false;
    /// <summary>Invert Y mouse input</summary>
    public bool invertY = false;
    /// <summary>Rotate speed of x Axis</summary>
    [Range(0.1f, 10f)]
    public float rotateSpeedX = 1;
    /// <summary>Rotate speed of y Axis</summary>
    [Range(0.1f, 10f)]
    public float rotateSpeedY = 1;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        //if (Cursor.lockState != CursorLockMode.Locked)
        //{
        //    int width = Screen.width;
        //    int height = Screen.height;
        //
        //    SetCursorPos(width / 2, height / 2);
        //}

        // get mouse rotation
        float x = Input.GetAxis("Mouse Y");
        float y = Input.GetAxis("Mouse X");

        // invert if needed
        if (invertY)
            x = -x;
        if (invertX)
            y = -y;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X))
            invertX = !invertX;
        if (Input.GetKeyDown(KeyCode.Y))
            invertY = !invertY;
#endif
        // do rotation
        Rotate(x, y);
    }

    private void Rotate(float _x, float _y)
    {
        _x *= rotateSpeedX;
        _x += gameObject.transform.eulerAngles.x;
        _y *= rotateSpeedY;
        _y += gameObject.transform.eulerAngles.y;
        gameObject.transform.eulerAngles = new Vector3(_x, _y, 0);

        // set rotation of body
        m_Body.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
    }
}
