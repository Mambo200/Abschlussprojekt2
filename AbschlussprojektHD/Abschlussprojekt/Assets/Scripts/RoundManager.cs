using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour {

    /// <summary>Round count variable (USE <see cref="RoundCount"/>)</summary>
    [SyncVar]
    private int roundCount;
    /// <summary>Round count Property</summary>
    public int RoundCount
    {
        get { return roundCount; }
        set
        {
            if (isServer)
                roundCount++;
        }
    }
    /// <summary>Chaser Singleton</summary>
    //private static RoundManager _single;
    //private static object _lock = new object();
    /// <summary>Roundmanager Singleton Getter</summary>
    //public static RoundManager Get
    //{
    //    get
    //    {
    //        if (_single == null)
    //        {
    //            lock (_lock)
    //            {
    //                if (_single == null)
    //                {
    //                    _single = new RoundManager();
    //                }
    //            }
    //        }
    //
    //        return _single;
    //    }
    //}


    /// <summary>Time left of current round</summary>
    [SyncVar]
    public float currentRoundTime = 0;
    /// <summary>Current Time property. only server can set time</summary>
    public float CurrentRoundTime
    {
        get { return currentRoundTime; }
        [Server]
        set
        {
            // if value is lower than 0 set variable to 0, else to value
            if (value <= 0)
                currentRoundTime = 0;
            else
                currentRoundTime = value;
        }
    }

    /// <summary>Wait time for next round to start after round is finished</summary>
    [SyncVar]
    private float tillNextRound = 7;
    public float TillNextRound
    {
        get { return tillNextRound; }
        set
        {
            tillNextRound = value;
        }
    }
    /// <summary>When round starts, this value if the default time to start (0 players)</summary>
    private float StartTime { get { return 10f; } }
    /// <summary>for each player add this to the round time (0 player -> 60 + (this * playercount) seconds)</summary>
    private float PlayerSeconds { get { return 5f; } }
    /// <summary>calculates the time for the next round</summary>
    private float NextRoundTime
    {
        get
        {
            float time = StartTime;
            time += MyNetworkManager.AllPlayers.Count * PlayerSeconds;
            return time;
        }
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(CurrentRoundTime + ", " + TillNextRound + ", " + RoundCount);
        // reduce current round time by deltatime each frame
        if (!isServer)
            return;

        //if (MyNetworkManager.AllPlayersGo.Count <= 1)
        //    return;

        CurrentRoundTime -= Time.deltaTime;

        if (CurrentRoundTime == 0)
        {
            // reduce time till next round
            TillNextRound -= Time.deltaTime;

            if (TillNextRound <= 0)
            {
                // if time is up start new round
                NextRound();
            }
        }

        Debug.Log(new Vector3(CurrentRoundTime, TillNextRound, RoundCount));
	}

    /// <summary>
    /// New Round
    /// </summary>
    public void NextRound()
    {
        // rotate new chaser
        Chaser.ChooseChaser();
        // reset hp and sp values for new round and teleport to new position.
        foreach (PlayerEntity player in MyNetworkManager.AllPlayers)
        {
            // reset stats for player
            player.NewRoundReset();

            TillNextRound = 5f;
            CurrentRoundTime = NextRoundTime;
            RoundCount++;

            #region PROBLEM: Nach dem Teleport Springt der Compiler aus NextRound() raus
            // teleport player to new position
            if (player.IsChaser)
            {
                if (isServer)
                    player.ServerTeleport(SpawnpointHandler.NextChaserpoint());
                else
                    player.RpcTeleport(SpawnpointHandler.NextChaserpoint());
            }
            else
            {
                if (isServer)
                    player.ServerTeleport(SpawnpointHandler.NextSpawnpoint());
                else
                    player.RpcTeleport(SpawnpointHandler.NextSpawnpoint());
            }
            #endregion
        }
    }
}
