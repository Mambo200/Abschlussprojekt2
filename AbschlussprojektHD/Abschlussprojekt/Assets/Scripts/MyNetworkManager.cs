using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        SearchPlayer();
    }

    private void Update()
    {
        SearchPlayer();
        //Debug.Log(allPlayers.Count);
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
        allPlayersGo.Clear();

        // find objects with tag
        List<GameObject> tempGO = (GameObject.FindGameObjectsWithTag("Player")).ToList();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnPlayerConnected()
    {
        SearchPlayer();
    }

    private void OnPlayerDisconnected()
    {
        SearchPlayer();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static List<PlayerEntity> AllPlayerCopy()
    {
        PlayerEntity[] temp = new PlayerEntity[allPlayers.Count];
        System.Array.Copy(AllPlayers.ToArray(), temp, temp.Length);
        allPlayers.CopyTo(temp);
        return temp.ToList();
    }
}
