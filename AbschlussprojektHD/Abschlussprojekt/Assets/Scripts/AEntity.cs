using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public abstract class AEntity : NetworkBehaviour
{
    protected Rigidbody m_rigidbody;
    public Material[] mat;
    [SerializeField]
    protected GameObject m_body;
    ///<summary>Player Canvas</summary>
    [SerializeField]
    protected Canvas m_Canvas;
    ///<summary>Player HP Text</summary>
    [SerializeField]
    protected Text m_HPText;
    ///<summary>Player SP Text</summary>
    [SerializeField]
    protected Text m_SPText;    
    ///<summary>Player SP Text</summary>
    [SerializeField]
    protected Text m_TimeText;
    ///<summary>local time of current Round</summary>
    private float m_localRoundTime;
    private RoundManager RoundManager { get; set; }

    #region Game Variables
    /// <summary>Max HP variable (DO NOT USE! USE <see cref="MaxHP"/> INSTEAD)</summary>
    [SyncVar]
    private float maxHP = 150;
    /// <summary>Max HP property (Everyone can get, only Server can set in <see cref="SetMaxHP(float)"/>)</summary>
    public float MaxHP
    {
        get { return maxHP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Max HP to " + value, this.gameObject);
                return;
            }

            BeforeMaxHPChanged(value);
            maxHP = value;
            AfterMaxHPChanged();
            // change UI Text
            RpcChangeTextHP( currentHP, maxHP);
        }
    }

    /// <summary>Current HP variable (DO NOT USE! USE <see cref="CurrentHP"/> INSTEAD)</summary>
    [SyncVar]
    private float currentHP;
    /// <summary>Current HP property (Everyone can get, only Server can set is <see cref="SetCurrentHP(float)"/></summary>
    public float CurrentHP
    {
        get { return currentHP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current HP to " + value, this.gameObject);
                return;
            }

            value = BeforeCurrentHPChanged(value);
            currentHP = value;
            AfterCurrentHPChanged();
            // change UI Text
            RpcChangeTextHP((int)currentHP, maxHP);
        }
    }

    /// <summary>Max SP variable (DO NOT USE! USE <see cref="MaxSP"/> INSTEAD)</summary>
    [SyncVar]
    private float maxSP = 150;
    /// <summary>Max SP property (Everyone can get, only Server can set in <see cref="SetMaxSP(float)"/>)</summary>
    public float MaxSP
    {
        get { return maxSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value, this.gameObject);
                return;
            }

            BeforeMaxSPChanged(value);
            maxSP = value;
            AfterMaxSPChanged();
            // change UI Text
            RpcChangeTextSP((int)currentSP, maxSP);

        }
        /// <summary>Max HP property (Everyone can get, only Server can set)</summary>
    /// <summary>Max HP property (Everyone can get, only Server can set)</summary>
    }

    /// <summary>Current SP variable (DO NOT USE! USE <see cref="CurrentSP"/> INSTEAD)</summary>
    [SyncVar]
    private float currentSP;
    /// <summary>Current HP property (Everyone can get, only Server can set in <see cref="SetCurrentSP(float)"/>)</summary>
    public float CurrentSP
    {
        get { return currentSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value, this.gameObject);
                return;
            }

            value = BeforeCurrentSPChanged(value);
            currentSP = value;
            AfterCurrentSPChanged();
            // change UI Text
            RpcChangeTextSP(currentSP, maxSP);
        }
    }

    /// <summary>Kill count variable (DO NOT USE! USE <see cref="KillCount"/> INSTEAD)</summary>
    [SyncVar]
    private int killCount = 0;
    /// <summary>Kill count property (Everyone can get, only Server can set in <see cref="SetKillCount(int)"/>)</summary>
    public int KillCount
    {
        get { return killCount; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Kill count to " + value, this.gameObject);
                return;
            }

            BeforeKillCountChanged(value);
            killCount = value;
            AfterKillCountChanged();
        }

    }

    /// <summary>Death count variable (DO NOT USE! USE <see cref="DeathCount"/> INSTEAD)</summary>
    [SyncVar]
    private int deathCount = 0;
    /// <summary>Death count property (Everyone can get, only Server can set in <see cref="SetDeathCount(int)"/>)</summary>
    public int DeathCount
    {
        get { return deathCount; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Death count to " + value, this.gameObject);
                return;
            }

            BeforeDeathCountChanged(value);
            deathCount = value;
            AfterDeathCountChanged();
        }

    }

    /// <summary>Default variable of Player armor. 
    /// If you want to change the current armor of player use <see cref="CurrentArmor"/> instead. (DO NOT USE! USE <see cref="PlayerArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float playerArmor = 1f;
    /// <summary>Player Armor property. Please change current armor in <see cref="CurrentArmor"/>. (Everyone can get, only Server can set in <see cref="SetDefaultPlayerArmor(float)"/>)</summary>
    public float PlayerArmor
    {
        get { return playerArmor; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set default value of Player Armor to " + value, this.gameObject);
                return;
            }

            BeforeDefaultPlayerArmorChanged(value);
            playerArmor = value;
            AfterDefaultPlayerArmorChanged();
        }
    }

    /// <summary>Default variable of Chaser armor. 
    /// If you want to change the current armor of chaser use <see cref="CurrentArmor"/> instead. (DO NOT USE! USE <see cref="ChaserArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float chaserArmor = 50f;
    /// <summary>Chaser Armor property. Please change current armor in <see cref="CurrentArmor"/>. (Everyone can get, only Server can set in <see cref="SetDefaultChaserArmor(float)"/>)</summary>
    public float ChaserArmor
    {
        get { return chaserArmor; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set default value of chaser Armor to " + value, this.gameObject);
                return;
            }

            BeforeDefaultChaserArmorChanged(value);
            chaserArmor = value;
            AfterDefaultChaserArmorChanged();
        }

    }

    /// <summary>Current Armor variable (DO NOT USE! USE <see cref="CurrentArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float currentArmor;
    /// <summary>Current Armor property (Everyone can get, only Server can set in <see cref="SetCurrentArmor(float)"/>)</summary>
    public float CurrentArmor
    {
        get
        {
            return currentArmor;
        }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current armor to " + value, this.gameObject);
                return;
            }
            BeforeCurrentArmorChanged(value);
            currentArmor = value;
            AfterCurrentArmorChanged();
        }
    }

    /// <summary>How often was player Chaser</summary>
    [SyncVar]
    private int chaserCount = 0;
    /// <summary>Current Chaser count property (Everyone can get, only Server can set)</summary>
    public int ChaserCount
    {
        get { return chaserCount; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser count to " + value, this.gameObject);
                return;
            }
            BeforeChaserCountChanged(value);
            chaserCount = value;
            AfterChaserCountChanged();
        }
    }

    /// <summary>True if player was chaser last round</summary>
    [SyncVar]
    private bool wasChaserLastRound = false;
    /// <summary>Was Chaser last round property (Everyone can get, only Server can set)</summary>
    public bool WasChaserLastRound
    {
        get { return wasChaserLastRound; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set was chaser last round to " + value, this.gameObject);
                return;
            }
            BeforeLastRoundChaserChanged(value);
            wasChaserLastRound = value;
            AfterLastRoundChaserChanged();
        }
    }

    /// <summary>Chaser variable (DO NOT USE! USE <see cref="IsChaser"/> INSTEAD)</summary>
    [SyncVar]
    private bool isChaser = false;
    /// <summary>Chaser property (Everyone can get, only Server can set in <see cref="SetChaser(bool)"/>)</summary>
    public bool IsChaser
    {
        get { return isChaser; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser mode to " + value, this.gameObject);
                return;
            }
            BeforeChaserChanged(value);
            isChaser = value;
            AfterChaserChanged();
        }
    }
    #endregion

    /// <summary>Current round time property</summary>
    public float LocalRoundTime
    {
        get { return m_localRoundTime; }
        set
        {
            if (value <= 0)
                m_localRoundTime = 0;
            else
                m_localRoundTime = value;

            // change text
            m_TimeText.text = ((int)m_localRoundTime).ToString();
        }
    }

    #region Override Functions
    public override void OnStartServer()
    {
        base.OnStartServer();
        Initialize();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Initialize();
    }
    #endregion

    #region Abstract Functions
    #endregion

    #region Public Functions
    #region Set specific values
    /// <summary>
    /// Set new max HP using Property (<see cref="MaxHP"/>)
    /// </summary>
    /// <param name="_newMaxHP">new max HP of player</param>
    public void SetMaxHP (float _newMaxHP)
    {
        MaxHP = _newMaxHP;
    }

    /// <summary>
    /// Set new current HP using Property (<see cref="CurrentHP"/>)
    /// </summary>
    /// <param name="_newCurrentHP">new current HP of player</param>
    public void SetCurrentHP (float _newCurrentHP)
    {
        CurrentHP = _newCurrentHP;
    }

    /// <summary>
    /// Set new current HP using Property (<see cref="CurrentHP"/>)
    /// </summary>
    /// <param name="_damage">Damage taken</param>
    public void GetDamage(float _damage, PlayerEntity _enemy)
    {
        // when enemy is chaser multiply damage
        if (_enemy.IsChaser)
        {
            _damage *= Chaser.DamageMultiplier;
        }
        CurrentHP -= _damage / CurrentArmor;

        // check if dead
        if (CurrentHP <= 0)
        {
            DeathCount++;
            _enemy.KillCount++;
        }
    }

    /// <summary>
    /// Set new max SP using Property (<see cref="MaxSP"/>).
    /// </summary>
    /// <param name="_newMaxSP">new max SP of player</param>
    public void SetMaxSP (float _newMaxSP)
    {
        MaxSP = _newMaxSP;
    }

    /// <summary>
    /// Set new current SP using Property (<see cref="CurrentSP"/>)
    /// </summary>
    /// <param name="_newCurrentSP">new current SP of player</param>
    public void SetCurrentSP (float _newCurrentSP)
    {
        CurrentSP = _newCurrentSP;
    }

    /// <summary>
    /// Set new kill count using Property (<see cref="KillCount"/>)
    /// </summary>
    /// <param name="_newKillCount">new kill count of player</param>
    public void SetKillCount (int _newKillCount)
    {
        KillCount = _newKillCount;
    }

    /// <summary>
    /// Set new death count using Property (<see cref="DeathCount"/>)
    /// </summary>
    /// <param name="_newDeathCount">new death count of player</param>
    public void SetDeathCount (int _newDeathCount)
    {
        DeathCount = _newDeathCount;
    }

    /// <summary>
    /// Set new player default player armor using Property (<see cref="PlayerArmor"/>).
    /// Change Current armor in <see cref="SetCurrentArmor(float)"/>
    /// </summary>
    /// <param name="_newDefaultPlayerArmor">new player default player armor value</param>
    public void SetDefaultPlayerArmor (float _newDefaultPlayerArmor)
    {
        PlayerArmor = _newDefaultPlayerArmor;
    }

    /// <summary>
    /// Set new player default chaser armor using Property (<see cref="ChaserArmor"/>)
    /// Change Current armor in <see cref="SetCurrentArmor(float)"/>
    /// </summary>
    /// <param name="_newDefaultPlayerArmor">new player default chaser armor value</param>
    public void SetDefaultChaserArmor(float _newDefaultChaserArmor)
    {
        ChaserArmor = _newDefaultChaserArmor;
    }

    /// <summary>
    /// Set new current Armor using Property (<see cref="CurrentArmor"/>)
    /// </summary>
    /// <param name="_newCurrentArmor">new current Armor of player</param>
    public void SetCurrentArmor (float _newCurrentArmor)
    {
        CurrentArmor = _newCurrentArmor;
    }

    /// <summary>
    /// Set Chaser using Property (<see cref="IsChaser"/>) This Function has to be called every round for each player!!!
    /// </summary>
    /// <exception cref="System.NotImplementedException"/>
    /// <param name="_isChaser">chaser value</param>
    public void SetChaser(bool _isChaser)
    {
        // Check if value is the same and if new variable is false
        if (!IsChaser && !_isChaser)
        {
            // set chaser last round variable to false
            WasChaserLastRound = false;
            return;
        }
        // if both values are true, something went wrong
        if (IsChaser && _isChaser)
        {
            Debug.LogException(new System.NotImplementedException("This message should not appear! This Player is chaser two times in a row"), this.gameObject);
        }

        // set new chaser status
        IsChaser = _isChaser;

        // if last round player was no Chaser and now is increase chaser counter
        if (_isChaser)
            ChaserCount++;
        // if last round player was Chaser and now is not set last round chaser to true;
        else
            WasChaserLastRound = true;
    }
    #endregion

    /// <summary>
    /// Resets values for new round (current HP, current SP)
    /// </summary>
    public void NewRoundReset()
    {
        CurrentHP = MaxHP;
        CurrentSP = MaxSP;
    }

    /// <summary>
    /// Resets values for new game (current HP, current SP, killcount, Deathcount, current Armor)
    /// </summary>
    public void NewGameReset()
    {
        CurrentHP = MaxHP;
        CurrentSP = MaxSP;
        KillCount = 0;
        DeathCount = 0;
        CurrentArmor = 0;
    }
    #endregion

    #region Private Functions
    #endregion

    #region Protected Functions    

    #endregion

    #region Public static Functions
    #endregion

    #region Private static Functions
    #endregion

    #region Virtual Functions
    #region Max HP Changed
    /// <summary>
    /// Function is called before players Max HP is set
    /// </summary>
    /// <param name="_newValue">new Value of variable</param>
    public virtual void BeforeMaxHPChanged(float _newValue) { }
    /// <summary>
    /// Function is called after players Max HP was set
    /// </summary>
    public virtual void AfterMaxHPChanged() { }
    #endregion

    #region Current HP Changed
    /// <summary>
    /// Function is called before players Current HP is set
    /// </summary>
    /// <param name="_newValue">new Value of variable</param>
    public virtual float BeforeCurrentHPChanged(float _newValue)
    {
        if (_newValue <= 0)
            return 0;
        else
            return _newValue;
    }
    /// <summary>
    /// Function is called after players Current HP was set
    /// </summary>
    public virtual void AfterCurrentHPChanged() { }
    #endregion

    #region Max Sp Changed
    /// <summary>
    /// Function is called before player Max SP is set
    /// </summary>
    /// <param name="_newValue">new Value of variable</param>
    public virtual void BeforeMaxSPChanged(float _newValue) { }
    /// <summary>
    /// Function is called after players Max SP was set
    /// </summary>
    public virtual void AfterMaxSPChanged() { }
    #endregion

    #region Current SP Changed
    /// <summary>
    /// Function is called before players Current SP is set
    /// </summary>
    /// <param name="_newValue">new Value of variable</param>
    public virtual float BeforeCurrentSPChanged(float _newValue)
    {
        if (_newValue <= 0)
            return 0;
        else
            return _newValue;
    }
    /// <summary>
    /// Function is called after players Current SP was set
    /// </summary>
    public virtual void AfterCurrentSPChanged() { }
    #endregion

    #region Kill Count Changed
    /// <summary>
    /// Function is called before players kill count is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeKillCountChanged(int _newValue) { }
    /// <summary>
    /// Function is called after players kill count was set
    /// </summary>
    public virtual void AfterKillCountChanged() { }
    #endregion

    #region Death Count Changed
    /// <summary>
    /// Function is called before players death count is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeDeathCountChanged(int _newValue) { }
    /// <summary>
    /// Function is called after players kill count was set
    /// </summary>
    public virtual void AfterDeathCountChanged() { }
    #endregion

    #region Default Armor Changed    
    /// <summary>
    /// Function is called before players default value for Player Armor is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeDefaultPlayerArmorChanged(float _newValue) { }
    /// <summary>
    /// Function is called after players default value for Player Armor was set
    /// </summary>
    public virtual void AfterDefaultPlayerArmorChanged() { }
    /// <summary>
    /// Function is called before players default value for Chaser Armor is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeDefaultChaserArmorChanged(float _newValue) { }
    /// <summary>
    /// Function is called after players default value for Chaser Armor was set
    /// </summary>
    public virtual void AfterDefaultChaserArmorChanged() { }
    #endregion

    #region Current Armor changed
    /// <summary>
    /// Function is called before players Current armor is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeCurrentArmorChanged(float _newValue) { }
    /// <summary>
    /// Function is called after playes Current armor was set
    /// </summary>
    public virtual void AfterCurrentArmorChanged()
    {
        // check if armor was given right

        // check if player is chaser
        if (IsChaser)
        {
            // check if current armor equals Chaser armor
            if (CurrentArmor != ChaserArmor)
            {
                // if not throw new exception
                Debug.LogException(
                    new System.NotImplementedException(
                        "In Chaser Mode, Current Armor has to be "
                        + ChaserArmor
                        + ". Your Armor is "
                        + CurrentArmor
                        + ".")
                        );

            }
        }

        // check is player is not cheaser
        else if (!IsChaser)
        {
            // check if current armor equals Player armor
            if (CurrentArmor != PlayerArmor)
            {
                // if not throw new exception
                throw new System.NotImplementedException(
                    "In Player Mode, Current Armor has to be "
                    + PlayerArmor
                    + ". Your Armor is "
                    + CurrentArmor
                    + ".");
            }

        }
    }
    #endregion

    #region Chaser Count Changed
    /// <summary>
    /// Function is called before players Chaser count is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeChaserCountChanged(int _newValue) { }
    /// <summary>
    /// Function is called after players Chaser count was set
    /// </summary>
    public virtual void AfterChaserCountChanged() { }
    #endregion

    #region Last Round Chaser changed
    /// <summary>
    /// Function is called before players last round Chaser value is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeLastRoundChaserChanged(bool _newValue) { }
    /// <summary>
    /// Function is called after players last round Chaser value was set
    /// </summary>
    public virtual void AfterLastRoundChaserChanged() { }
    #endregion

    #region Chaser changed
    /// <summary>
    /// Function is called before players Chaser status is set
    /// </summary>
    /// <param name="_newValue">New value of Variable</param>
    public virtual void BeforeChaserChanged(bool _newValue) { }
    /// <summary>
    /// Function is called after players Chaser status was set.
    /// Set Armor
    /// </summary>
    public virtual void AfterChaserChanged()
    {
        if (IsChaser)
        {
            CurrentArmor = ChaserArmor;
            m_body.GetComponent<Renderer>().material = mat[0]; 
        }
        else
        {
            CurrentArmor = PlayerArmor;
            m_body.GetComponent<Renderer>().material = mat[1];
        }
    }
    #endregion
    #endregion

    #region Network
    #region RPC    
    /// <summary>
    /// Set new position of player (RPC Call)
    /// </summary>
    /// <param name="_newPosition">The new position.</param>
    [ClientRpc]
    public void RpcTeleport(Vector3 _newPosition)
    {
        gameObject.transform.position = _newPosition;
    }
    /// <summary>
    /// Set new Position of Player (Server Only)
    /// </summary>
    /// <param name="_newPosition">The new position.</param>
    public void ServerTeleport(Vector3 _newPosition)
    {
        gameObject.transform.position = _newPosition;
    }
    /// <summary>
    /// Changes the text.
    /// </summary>
    /// <param name="_text">Text UI Text to change</param>
    /// <param name="_currentValue">Current value</param>
    /// <param name="_maxValue">Maximum value</param>
    [ClientRpc]
    protected void RpcChangeTextHP(float _currentValue, float _maxValue)
    {
        // set new UI Text
        m_HPText.text = _currentValue + " / " + _maxValue;
    }

    /// <summary>
    /// Changes the text.
    /// </summary>
    /// <param name="_text">Text UI Text to change</param>
    /// <param name="_currentValue">Current value</param>
    /// <param name="_maxValue">Maximum value</param>
    [ClientRpc]
    protected void RpcChangeTextSP(float _currentValue, float _maxValue)
    {
        // set new UI Text
        m_SPText.text = _currentValue + " / " + _maxValue;
    }
    #endregion
    /// <summary>
    /// Set next round time and save it local
    /// </summary>
    /// <param name="_time">time for next round</param>
    [ClientRpc]
    public void RpcSetRoundTime(float _time)
    {
        LocalRoundTime = _time;
    }

    #region Command

    /// <summary>
    /// When Player hits another Player deal damage
    /// </summary>
    /// <param name="_origin">Start position</param>
    /// <param name="_direction">direction of shot</param>
    [Command]
    protected void CmdHit(Vector3 _origin, Vector3 _direction)
    {
        // if player dont have HP left return
        if (CurrentHP <= 0)
            return;

        // get roundmanager
        if (RoundManager == null)
            RoundManager = GameObject.Find("Rundenmanager").GetComponent<RoundManager>();

        // if round is overr no damage will be applied
        if (RoundManager.CurrentRoundTime <= 0)
            return;

        Ray ray = new Ray(_origin, _direction);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.black, 2f);
        if (Physics.Raycast(ray, out hit))
        {
            // check if hit object is player
            if (hit.collider.gameObject.tag != "Player")
                return;

            // get Playerentity (GetParent because Capsule is hit and Capsules parent has playerentity)
            PlayerEntity p = hit.collider.gameObject.GetComponentInParent<PlayerEntity>();

            // check if chaserstatus is not equal
            if (IsChaser != p.IsChaser)
            {
                p.GetDamage(2, p);
            }

            Debug.Log(p.gameObject.name + ", " + new Vector2(p.CurrentHP, p.CurrentSP));
        }
    }
    #endregion

    #endregion
    /// <summary>
    /// Initializes this instance
    /// </summary>
    protected virtual void Initialize()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    
}
