using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerEntity : AEntity {

    ///<summary>Movement Speed of Player</summary>
    public float m_MovementSpeed;
    ///<summary>Speed with which the Player can rotate</summary>
    public float m_RotationSpeed;
    ///<summary>The force with which the Player can Jump</summary>
    public float m_JumpForce;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Jump();
        }

        Vector3 dir = Input.GetAxisRaw("Horizontal") * transform.right +
            Input.GetAxisRaw("Vertical") * transform.forward;
        Move(dir);

        Dash(dir);
        
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
        MyNetworkManager.AddPlayer(this);
    }

    
}
