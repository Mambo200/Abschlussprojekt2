﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEntity : AEntity
{
    bool isgrounded;

    float distancetoground;
 
    Vector3 _direction;

    Vector3 walljumpDir;

    GameObject lastwalljumped;

    int jumpcount;

    public float m_maxdashtime = 1.0f;

    public float m_dashstopspeed = 0.25f;

    public float m_currentdashtime = 0f;

    public float m_resetdashtime = 2f;

    public float m_isgrounedoffset = 0.1f;

    private float m_DefaultMovementSpeed;

    public float m_reduceSP = 10f;

    public float cooldowntime = 1f;

    public float _regenSP;

    float cooldownstart = 0f;

    float timestamp;

    bool isdashing;

    float timesincelastcall;

    

    ///<summary>Movement Speed of Player</summary>
    public float m_MovementSpeed;
    ///<summary>Speed with which the Player can rotate</summary>
    public float m_RotationSpeed;
    ///<summary>The force with which the Player can Jump</summary>
    public float m_JumpForce;
    // Use this for initialization
    void Start ()
    {
        m_currentdashtime = m_maxdashtime;

        m_DefaultMovementSpeed = m_MovementSpeed;

        distancetoground = GetComponentInChildren<Collider>().bounds.extents.y;

        
    }

    // Update is called once per frame
    override protected void Update ()
    {
        base.Update();

        if (!isLocalPlayer)
            return;

        TimeCounter();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        bool weaponChanged = false;

        // change weapon via mouse scroll wheel
        float mousescroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (mousescroll != 0)
        {
            int oldIndex = WeaponIndex;

            if (mousescroll < 0)
                WeaponIndex--;
            else if (mousescroll > 0)
                WeaponIndex++;

            if (oldIndex != WeaponIndex)
            {
                weaponChanged = true;
                Debug.Log("Weapon " + WeaponIndex + ": " + GetCurrentWeapon.GetWeaponName);
            }

        }

        // change weapon via numbers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int oldIndex = WeaponIndex;
            WeaponIndex = 0;
            if (oldIndex != WeaponIndex)
            {
                Debug.Log("Weapon " + WeaponIndex + ": " + GetCurrentWeapon.GetWeaponName);
                weaponChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int oldIndex = WeaponIndex;
            WeaponIndex = 1;
            if (oldIndex != WeaponIndex)
            {
                Debug.Log("Weapon " + WeaponIndex + ": " + GetCurrentWeapon.GetWeaponName);
                weaponChanged = true;
            }

        }

        // check if weapon was changed this frame
        if (weaponChanged)
            AmmoTextBox.text = GetCurrentWeapon.AmmoText;

        //Debug.DrawRay(transform.position, test , Color.black);

        //Debug.Log(isgrounded);

        //Debug.DrawRay(transform.position, Vector3.down, Color.green);

        //Debug.Log(isdashing);

        timesincelastcall += Time.deltaTime;

        _regenSP = Time.deltaTime * 2f;

        if (Time.time > timestamp)
        {
            CmdSetRegenSP(_regenSP);
        }

        isgrounded = Physics.Raycast(transform.position, Vector3.down, distancetoground + m_isgrounedoffset);

        if (isgrounded == true)
        {
            lastwalljumped = null;
            jumpcount = 0;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Shoot
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            Shoot();
        }

        var forward = m_playerCamera.transform.forward;
        forward.y = 0;
        var right = m_playerCamera.transform.right;
        right.y = 0;
        // Mouse Input
        Vector3 dir = Input.GetAxisRaw("Horizontal") * right +
            Input.GetAxisRaw("Vertical") * forward;

        if(dir.x != 0 && dir.z != 0 && isgrounded)
        {
            walljumpDir = dir;
        }

        Move(dir);

        Dash(dir);        
    }


    #region Override Functions
    protected override void Initialize()
    {
        base.Initialize();

        if (!isServer)
            return;
        // set players current values
        SetCurrentHP(MaxHP);
        SetCurrentSP(MaxSP);
        SetCurrentArmor(PlayerArmor);
    }
    #endregion

    #region Movement
    private void Jump()
    {
        if(isgrounded)
        {
            m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.VelocityChange);

        }
    }

    private void Move(Vector3 _direction)
    {
        m_lookAt.position = transform.position + m_playerCamera.transform.forward;

        Transform t = m_playerCamera.transform;
        Vector3 pos = (new Vector3(t.position.x, transform.position.y, t.position.z));
        t.position = pos;
        transform.LookAt(m_lookAt);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        if (isgrounded)
        {
            Vector3 velocity = _direction.normalized * m_MovementSpeed;
            velocity.y = m_rigidbody.velocity.y;
            m_rigidbody.velocity = velocity;
        }
          
    }

    private void WallMove(Vector3 _direction)
    {
        Vector3 velocity = _direction.normalized * m_MovementSpeed;
        velocity.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = velocity;
        jumpcount++;
    }

    

    

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isgrounded && lastwalljumped != collision.gameObject)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if(jumpcount % 2 == 0)
                {
                    WallMove(-walljumpDir);
                }
                else
                {
                    WallMove(walljumpDir);
                }
                m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.VelocityChange);
                lastwalljumped = collision.gameObject;
            }
               
        }
    }

    
    



    private void Dash(Vector3 _direction)
    {
        if (Time.time > timestamp && CurrentSP > 10 && isgrounded)
        {

            #region ---DASH RIGHT---
            if (Input.GetButtonDown("LeftShift")  && Input.GetButton("D_Key") || Input.GetButtonDown("D_Key") && Input.GetButton("LeftShift"))
            {
                timestamp = Time.time + cooldowntime;

                m_currentdashtime = 0.0f;

                isdashing = true;

                if (timesincelastcall >= 1)
                {
                    if (isdashing)
                    {
                        SetReducedSP(m_reduceSP);
                    }
                    timesincelastcall = 0;
                }

                if (m_currentdashtime < m_maxdashtime)
                {
                    m_MovementSpeed = 20f;
                }
                isdashing = false;

            }
            #endregion

            #region ---DASH LEFT---
            if (Input.GetButtonDown("LeftShift") && Input.GetButton("A_Key") || Input.GetButtonDown("A_Key") && Input.GetButton("LeftShift"))
            {
                timestamp = Time.time + cooldowntime;

                m_currentdashtime = 0.0f;

                isdashing = true;

                if (timesincelastcall >= 1)
                {
                    if (isdashing)
                    {
                        SetReducedSP(m_reduceSP);
                    }
                    timesincelastcall = 0;
                }

                if (m_currentdashtime < m_maxdashtime)
                {
                    m_MovementSpeed = 20f;
                }

                isdashing = false;
            }
            #endregion

            #region ---DASH FORWARD---
            if (Input.GetButtonDown("LeftShift") && Input.GetButton("W_Key") || Input.GetButtonDown("W_Key") && Input.GetButton("LeftShift"))
            {
                timestamp = Time.time + cooldowntime;

                m_currentdashtime = 0.0f;

                isdashing = true;

                if (timesincelastcall >= 1)
                {
                    if (isdashing)
                    {
                        SetReducedSP(m_reduceSP);
                    }
                    timesincelastcall = 0;
                }

                if (m_currentdashtime < m_maxdashtime)
                {
                    m_MovementSpeed = 20f;
                }
                isdashing = false;
            }
            #endregion

            #region ---DASH BACKWARDS---
            if (Input.GetButtonDown("LeftShift") && Input.GetButton("S_Key") || Input.GetButtonDown("S_Key") && Input.GetButton("LeftShift"))
            {
                timestamp = Time.time + cooldowntime;

                m_currentdashtime = 0.0f;

                isdashing = true;

                if (timesincelastcall >= 1)
                {
                    if (isdashing)
                    {
                        SetReducedSP(m_reduceSP);
                    }
                    timesincelastcall = 0;
                }

                if (m_currentdashtime < m_maxdashtime)
                {
                    m_MovementSpeed = 20f;
                }
                isdashing = false;
            }
            #endregion
        
        }

        m_currentdashtime += m_dashstopspeed;

        if (m_currentdashtime == m_resetdashtime)
        {
            m_MovementSpeed = m_DefaultMovementSpeed;
        }

    }

    private void Shoot()
    {
        if (!wannaPlay)
            return;

        if (!GetCurrentWeapon.Shoot())
            return;

        if (GetCurrentWeapon.GetWeaponName == AWeapon.WeaponName.MACHINEGUN)
        {
            Ray ray = m_playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            CmdWeapon(ray.origin, ray.direction, WeaponIndex);
        }
    }

    private void Rotate(Vector3 _rotation)
    {
        Vector3 rotation = transform.localEulerAngles
        + (_rotation.normalized * Time.deltaTime * m_RotationSpeed);
        transform.localEulerAngles = rotation;
    }
    #endregion

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // activate Player Camera
        m_playerCamera.gameObject.SetActive(true);
        CineMachineObject.gameObject.SetActive(true);
        // activate UI
        m_UI.gameObject.SetActive(true);
        LobbyUINotReady();
    }

    private void OnDisconnectedFromServer(NetworkIdentity info)
    {
        Debug.Log(info);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        JoinUI();
    }

    private void TimeCounter()
    {
        LocalRoundTime -= Time.deltaTime;
    }

}
