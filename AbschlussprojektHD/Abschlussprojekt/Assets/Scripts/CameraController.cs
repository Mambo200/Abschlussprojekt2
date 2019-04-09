using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private GameObject m_PlayerParent;
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
    [SerializeField]
    private Camera m_camera;

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
        if (Input.GetKeyDown(KeyCode.Z))
            invertX = !invertX;
        if (Input.GetKeyDown(KeyCode.Y))
            invertY = !invertY;
#endif
        // Check Collision
        CheckWall();

        // do rotation
        Rotate(x, y);
    }

    private void Rotate(float _x, float _y)
    {
        _x *= rotateSpeedX;
        _x += gameObject.transform.eulerAngles.x;
        _y *= rotateSpeedY;
        _y += m_PlayerParent.gameObject.transform.eulerAngles.y;
        gameObject.transform.eulerAngles = new Vector3(_x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);

        // set rotation of parent
        m_PlayerParent.transform.eulerAngles = new Vector3(m_PlayerParent.transform.eulerAngles.x, _y, m_PlayerParent.transform.eulerAngles.z);
    }

    private void CheckWall()
    {
        Vector3 dir = m_camera.transform.position - m_PlayerParent.transform.position;
        Ray r = new Ray(m_PlayerParent.transform.position, dir);
        RaycastHit hit;
        Physics.Raycast(r, out hit);
        Debug.DrawRay(r.origin, r.direction);
        Debug.Log(hit.collider.name);

        if (hit.collider.name != "Main Camera")
        {
            m_camera.transform.Translate(0, 0, 1, this.transform);
        }
    }
}
