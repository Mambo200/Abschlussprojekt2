using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEntity : AEntity
{
    bool isgrounded;

    float distancetoground;
 
    private CharacterController controller;

    Vector3 _direction;

    Vector3 walljumpDir;

    GameObject lastwalljumped;

    int jumpcount;

    float timeStamp;
    
    
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
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        TimeCounter();
        

        Debug.DrawRay(transform.position, Vector3.down, Color.green);

        isgrounded = Physics.Raycast(transform.position, Vector3.down, distancetoground);

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
        
        // Mouse Input
        Vector3 dir = Input.GetAxisRaw("Horizontal") * transform.right +
            Input.GetAxisRaw("Vertical") * transform.forward;

        if(dir.x != 0 && dir.z != 0 && isgrounded)
        {
            walljumpDir = dir;
        }

         


        Move(dir);

        float timeStamp = Time.time + 2;

        
        
            Dash(dir);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
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
        if(!isgrounded == false)
        {
            m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.VelocityChange);

        }
    }

    private void Move(Vector3 _direction)
    {
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

    

    public float m_maxdashtime = 1.0f;

    public float m_dashstopspeed = 0.25f;

    public float m_currentdashtime = 0f;

    public float m_resetdashtime = 2f;

    private float m_DefaultMovementSpeed;

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
        Ray ray = m_playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        CmdHit(ray.origin, ray.direction);
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
        // activate UI
        m_UI.gameObject.SetActive(true);
    }

    private void OnDisconnectedFromServer(NetworkIdentity info)
    {
        Debug.Log(info);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void TimeCounter()
    {
        LocalRoundTime -= Time.deltaTime;
    }
}
