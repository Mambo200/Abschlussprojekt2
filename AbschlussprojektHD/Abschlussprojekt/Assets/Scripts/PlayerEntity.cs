using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEntity : AEntity {

    ///<summary>Movement Speed of Player</summary>
    public float m_MovementSpeed;
    ///<summary>Speed with which the Player can rotate</summary>
    public float m_RotationSpeed;
    ///<summary>The force with which the Player can Jump</summary>
    public float m_JumpForce;
    ///<summary>Playaer UI</summary>
    [SerializeField]
    protected GameObject m_UI;
    ///<summary>Player Camera</summary>
    [SerializeField]
    protected Camera m_playerCamera;
    // Use this for initialization
    void Start ()
    {

        m_currentdashtime = m_maxdashtime;

        m_DefaultMovementSpeed = m_MovementSpeed;

	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

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
        

        Vector3 dir = Input.GetAxisRaw("Horizontal") * transform.right +
            Input.GetAxisRaw("Vertical") * transform.forward;
        Move(dir);

        Dash(dir);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
    }


    #region Override Functions
    protected override void Initialize()
    {
        base.Initialize();
        // set players current values
        SetCurrentHP(MaxHP);
        SetCurrentSP(MaxSP);
        SetCurrentArmor(PlayerArmor);
    }
    #endregion

    #region Movement
    private void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.VelocityChange);
    }

    private void Move(Vector3 _direction)
    {
        Vector3 velocity = _direction.normalized * m_MovementSpeed;
        velocity.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = velocity;
    }

    

    public float m_maxdashtime = 1.0f;

    public float m_dashstopspeed = 0.25f;

    public float m_currentdashtime = 0f;

    public float m_resetdashtime = 2f;

    private float m_DefaultMovementSpeed;

    

    private void Dash(Vector3 _direction)
    {
        

        #region ---DASH RIGHT---
        if(Input.GetButtonDown("LeftShift")  && Input.GetButton("D_Key") || Input.GetButtonDown("D_Key") && Input.GetButton("LeftShift"))
        {

            m_currentdashtime = 0.0f;

            if (m_currentdashtime < m_maxdashtime)
            {
                m_MovementSpeed = 20f;
            }
            
        }
        #endregion

        #region ---DASH LEFT---
        if (Input.GetButtonDown("LeftShift") && Input.GetButton("A_Key") || Input.GetButtonDown("A_Key") && Input.GetButton("LeftShift"))
        {

            m_currentdashtime = 0.0f;

            if (m_currentdashtime < m_maxdashtime)
            {
                m_MovementSpeed = 20f;
            }

        }
        #endregion

        #region ---DASH FORWARD---
        if (Input.GetButtonDown("LeftShift") && Input.GetButton("W_Key") || Input.GetButtonDown("W_Key") && Input.GetButton("LeftShift"))
        {

            m_currentdashtime = 0.0f;

            if (m_currentdashtime < m_maxdashtime)
            {
                m_MovementSpeed = 20f;
            }

        }
        #endregion

        #region ---DASH BACKWARDS---
        if (Input.GetButtonDown("LeftShift") && Input.GetButton("S_Key") || Input.GetButtonDown("S_Key") && Input.GetButton("LeftShift"))
        {

            m_currentdashtime = 0.0f;

            if (m_currentdashtime < m_maxdashtime)
            {
                m_MovementSpeed = 20f;
            }

        }
        #endregion

        

        m_currentdashtime += m_dashstopspeed;

        if (m_currentdashtime == m_resetdashtime)
        {
            m_MovementSpeed = m_DefaultMovementSpeed;
        }

    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = m_playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag != "Player")
                return;
            // get Playerentity
            PlayerEntity p = hit.collider.gameObject.GetComponentInParent<PlayerEntity>();
            if (isServer)
            {
                p.SetCurrentHP(p.CurrentHP - 2);
                p.SetCurrentSP(p.CurrentSP - 1);
            }
            else
            {
                p.CmdChangeHP(p.CurrentHP - 2);
                p.CmdChangeSP(p.CurrentSP - 1);
            }

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
        m_UI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MyNetworkManager.AddPlayer(this);
    }

    private void OnDisconnectedFromServer(NetworkIdentity info)
    {
        Debug.Log(info);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


}
