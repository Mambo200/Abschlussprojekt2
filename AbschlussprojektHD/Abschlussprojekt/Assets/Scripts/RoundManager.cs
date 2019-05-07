﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour {

    public int minimumPlayers;

    /// <summary>Round count variable (USE <see cref="RoundCount"/>)</summary>
    [SyncVar]
    private int roundCount = 0;
    /// <summary>Round count Property</summary>
    public int RoundCount
    {
        get { return roundCount; }
        set
        {
            if (isServer)
                roundCount = value;
        }
    }

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
    private float StartTime
    {
        get
        {
#if UNITY_EDITOR
            return 10f;
#endif
            return 30f;
        }
    }
    /// <summary>for each player add this to the round time (0 player -> 60 + (this * playercount) seconds)</summary>
    private float PlayerSeconds
    {
        get
        {
#if UNITY_EDITOR
            return 5f;
#endif
            return 10f;
        }
    }
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
        if (!isServer)
            return;


        // check if player playing list is more than minimum count
        if (MyNetworkManager.AllPlayersPlaying.Count < minimumPlayers && MyNetworkManager.gameRunning)
        {
            // if no round has started yet return
            if (RoundCount == 0) return;
            if (RoundCount > 0)
            {
                // if player left in round
                foreach (PlayerEntity player in MyNetworkManager.AllPlayersPlaying)
                {
                    // reset players position to a lobby position
                    player.RpcChangeStartButtonTextToStart();
                    player.RpcTeleport(SpawnpointHandler.NextLobbypoint(), ETP.LOBBYTP);
                    player.RpcChangeToDefaultColor(Chaser.CurrentChaser);
                    player.wannaPlay = false;
                    MyNetworkManager.gameRunning = false;
                    MyNetworkManager.ReloadPlayers();
                }
                // reset stats
                Reset();
                // reset player positions and lists
                MyNetworkManager.ResetGame();

                return;
            }
        }

        // get through playing list and check if more than 2 players are ready
        int particitians = 0;
        if (MyNetworkManager.gameRunning)
        {
            foreach (PlayerEntity player in MyNetworkManager.AllPlayersPlaying)
            {
                particitians++;
                if (particitians >= minimumPlayers) break;
            }
        }
        else
        {
            foreach (PlayerEntity player in MyNetworkManager.AllPlayersWannaPlay)
            {
                particitians++;
                if (particitians >= minimumPlayers) break;
            }
        }

        // if not enough particitians are there return
        if (particitians < minimumPlayers) return;

        if (MyNetworkManager.AllPlayersPlaying.Count > 1)
            CurrentRoundTime -= Time.deltaTime;

        if (CurrentRoundTime == 0)
        {
            // reduce time till next round
            TillNextRound -= Time.deltaTime;

            if (TillNextRound <= 0)
            {
                // if time is up transfer lists and start new round
                MyNetworkManager.NewRound();
                NextRound();
                MyNetworkManager.gameRunning = true;
            }
        }
	}

    /// <summary>
    /// New Round
    /// </summary>
    public void NextRound()
    {
        // rotate new chaser
        Chaser.ChooseChaser();

        TillNextRound = 5f;
        CurrentRoundTime = NextRoundTime;
        RoundCount++;

        // reset hp and sp values for new round and teleport to new position.
        foreach (PlayerEntity player in MyNetworkManager.AllPlayersPlaying)
        {
            // reset stats for player
            player.NewRoundReset();

            // teleport player to new position
            if (player.IsChaser)
            {
                player.RpcTeleport(SpawnpointHandler.NextChaserpoint(), ETP.CHASERTP);
            }
            else
            {
                player.RpcTeleport(SpawnpointHandler.NextSpawnpointPlayer(), ETP.HUNTEDTP);
            }

            // set current round time for player to save it local
            player.RpcSetRoundTime(currentRoundTime);
        }
    }

    /// <summary>
    /// Reset Timers
    /// </summary>
    private void Reset()
    {
        RoundCount = 0;
        CurrentRoundTime = 0;
        TillNextRound = 7;
    }
}
