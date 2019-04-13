using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour {

    /// <summary>parent object</summary>
    [SerializeField]
    private GameObject m_PlayerParent;
    /// <summary>playerentity of parent object. USE <see cref="Playerentity"/></summary>
    private PlayerEntity m_Playerentity;
    /// <summary>playerentity of parent object</summary>
    private PlayerEntity Playerentity
    {
        get
        {
            if (m_Playerentity == null)
            {
                m_Playerentity = m_PlayerParent.GetComponent<PlayerEntity>();
            }
            return m_Playerentity;
        }
    }
    /// <summary>Invert X mouse input</summary>
    public bool invertX = false;
    /// <summary>Invert Y mouse input</summary>
    public bool invertY = false;
    /// <summary>allowed distance from player to camera</summary>
    public float distanceToCamera = 2f;
    /// <summary>allowed distance from camera to objects</summary>
    private float distanceFromCamera = 3f;
    /// <summary>allowed min distance from player to camera</summary>
    public float minCameraDistanceToPlayer;
    /// <summary>the speed with which the camera goes back (NOT NEGATIVE)</summary>
    public float cameraSpeed = 1f;
    /// <summary>Rotate speed of x Axis</summary>
    [Range(0.1f, 10f)]
    public float rotateSpeedX = 1;
    /// <summary>Rotate speed of y Axis</summary>
    [Range(0.1f, 10f)]
    public float rotateSpeedY = 1;
    [SerializeField]
    private Camera m_camera;
    private Vector3 lastValidAxis;

    /// <summary>
    /// Set mouse position (DO NOT USE!!!)
    /// </summary>
    /// <param name="X">x position of mouse</param>
    /// <param name="Y">y position of mouse</param>
    /// <returns></returns>
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

        if (x != 0 && y != 0) Playerentity.moved = true;

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

        //m_camera.transform.LookAt(this.transform);
    }

    private void Rotate(float _x, float _y)
    {
        _x *= rotateSpeedX;
        _x += gameObject.transform.eulerAngles.x;
        _y *= rotateSpeedY;
        _y += m_PlayerParent.gameObject.transform.eulerAngles.y;

        // clamp x
        if (_x >= 0 && _x <= 180)
        {
            if (_x >= 50)
            {
                _x = 50;
            }
        }
        else if (_x >= 190 && _x <= 360)
        {
            if (_x <= 290)
            {
                _x = 290;
            }
        }

        gameObject.transform.eulerAngles = new Vector3(_x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);

        // set rotation of parent
        m_PlayerParent.transform.eulerAngles = new Vector3(m_PlayerParent.transform.eulerAngles.x, _y, m_PlayerParent.transform.eulerAngles.z);

        m_camera.transform.LookAt(this.transform);

        Debug.Log(m_camera.transform.localEulerAngles);

        if (m_camera.transform.localEulerAngles.y > 100)
        {
            //m_camera.transform.localEulerAngles.Set(lastValidAxis.x, lastValidAxis.y, lastValidAxis.z);
            m_camera.transform.localRotation = Quaternion.Euler(lastValidAxis);
        }
        else
            lastValidAxis = m_camera.transform.localEulerAngles ;
    }

    private void CheckWall()
    {
        // get direction
        Vector3 dir = m_camera.transform.position - m_PlayerParent.transform.position;

        // create ray
        Ray playerToCamera = new Ray(m_PlayerParent.transform.position, dir);
        Ray CameraToWorld = new Ray(m_camera.transform.position, dir);

        // check hit of each raycast
        RaycastHit hitPlayerToCamera;
        Physics.SphereCast(playerToCamera, 1f, out hitPlayerToCamera);
        //Physics.Raycast(playerToCamera, out hitPlayerToCamera);

        RaycastHit hitCameraToWorld;
        Physics.SphereCast(CameraToWorld, 1f, out hitCameraToWorld); 
        //Physics.Raycast(CameraToWorld, out hitCameraToWorld);

        #region debuglog
        Debug.DrawRay(CameraToWorld.origin, CameraToWorld.direction);
        #endregion

        bool cameraContinue = true;

        // reset camera if something goes wrong
        if (hitPlayerToCamera.collider.name != "Main Camera")
        {
            m_camera.transform.position = hitPlayerToCamera.point;
            //m_camera.transform.Translate(0, 0, hitPlayerToCamera.distance, Space.Self);
            return;
        }

        // check if ray from camera hits an object
        if (hitCameraToWorld.distance < distanceFromCamera &&
            hitCameraToWorld.collider != null)
        {
            if (hitCameraToWorld.collider.tag == "Player") return;
            
            cameraContinue = false;

            // get distance
            float d = distanceFromCamera - hitCameraToWorld.distance;
            m_camera.transform.Translate(0, 0, d, Space.Self);
            Debug.Log("Camera to World");
        }

        // exit if camera was moved
        if (!cameraContinue) return;

        // check distance from player to camera
        if (hitPlayerToCamera.distance < distanceToCamera)
        {
            //if (hitCameraToWorld.distance < distanceFromCamera * 1.1f)
            //    return;

            // move camera back
            m_camera.transform.Translate(0, 0, -(cameraSpeed * Time.deltaTime), Space.Self);
            Debug.Log("Player to Camera");
        }

        //m_camera.transform.rotation.SetLookRotation(this.transform.position);
    }
}
