﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    /// <summary>Timer for Manager, every x seconds Server is looking for new player</summary>
    private float Timer { get; set; }
    /// <summary>Get Networkmanager Singleton</summary>
    public static NetworkManager GetSingleton { get { return singleton; } }
    /// <summary>List with all players</summary>
    private static List<PlayerEntity> allPlayers = new List<PlayerEntity>();
    /// <summary>List with all players</summary>
    private static List<GameObject> allPlayersGo = new List<GameObject>();
    /// <summary>List with all players</summary>
    public static List<PlayerEntity> AllPlayers
    {
        get
        {
            return allPlayers;
        }
    }
    /// <summary>List with all players</summary>
    public static List<GameObject> AllPlayersGo
    {
        get
        {
            return allPlayersGo;
        }
    }

    private void Start()
    {
        Timer = 30f;
    }

    private void Update()
    {
        // reduce Timer. Every x seconds search for new player
        //Timer -= Time.deltaTime;
        //if (Timer <= 0)
        //{
            SearchPlayer(true);
        //    Timer = 30f;
        //}
        Debug.Log(AllPlayers.Count);
    }

    /// <summary>
    /// Add player
    /// </summary>
    /// <param name="_player">The player</param>
    public static void AddPlayer(GameObject _player)
    {
        allPlayersGo.Add(_player);
        allPlayers.Add(_player.GetComponent<PlayerEntity>());
    }

    /// <summary>
    /// Remove player
    /// </summary>
    /// <param name="_player">The player</param>
    public static void RemovePlayer(GameObject _player)
    {
        allPlayersGo.Remove(_player);
        allPlayers.Remove(_player.GetComponent<PlayerEntity>());
    }

    /// <summary>
    /// Searching player in this Scene
    /// </summary>
    /// <param name="_force">
    /// false: Only overrite list when playercount is equal with found players
    /// || 
    /// true: overrite list even that playercount is equal with found players
    /// </param>
    private static void SearchPlayer(bool _force = false)
    {
        // find objects with tag
        List<GameObject> tempGO = (GameObject.FindGameObjectsWithTag("Player")).ToList();
        if (!_force)
        {
            if (tempGO.Count == allPlayers.Count)
            {
                return;
            }
        }

        // reset list
        allPlayers.Clear();
        allPlayersGo.Clear();

        foreach (GameObject go in tempGO)
        {
            if (go.name == "Player(Clone)")
            {
                allPlayersGo.Add(go);
            }
        }

        foreach (GameObject go in allPlayersGo)
        {
            allPlayers.Add(go.gameObject.GetComponent<PlayerEntity>());
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        allPlayers.Clear();
        allPlayersGo.Clear();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Timer = 3f;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Returns a Copy of all players
    /// </summary>
    /// <returns>copy of all players</returns>
    public static List<PlayerEntity> AllPlayerCopy()
    {
        return new List<PlayerEntity>(AllPlayers);
    }
}
