using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    /// <summary>Get every Player in Lobby</summary>
    //public static NetworkLobbyPlayer[] GetSingletonPlayer { get { return ((MyNetworkManager)singleton).lobbyslots; } }
    /// <summary>Get Networkmanager Singleton</summary>
    public static NetworkManager GetSingleton { get { return singleton; } }
    /// <summary>List with all players</summary>
    private static List<PlayerEntity> allPlayers = new List<PlayerEntity>();
    /// <summary>List with all players</summary>
    public static List<PlayerEntity> AllPlayers
    {
        get
        {
            return allPlayers;
        }
    }

    private void Start()
    {
        SearchPlayer();
    }

    private void Update()
    {
        SearchPlayer();
        Debug.Log(allPlayers.Count);
    }

    public static void AddPlayer(PlayerEntity _player)
    {
        allPlayers.Add(_player);
    }

    /// <summary>
    /// Searching player in this Scene
    /// </summary>
    private void SearchPlayer()
    {

        // reset list
        allPlayers.Clear();

        // find objects with tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in players)
        {
            allPlayers.Add(go.gameObject.GetComponent<PlayerEntity>());
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        allPlayers.Clear();
    }

    private void OnPlayerConnected()
    {
        SearchPlayer();
    }

    private void OnPlayerDisconnected()
    {
        SearchPlayer();
    }
}
