using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    /// <summary>when true, looking for players</summary>
    private static bool Reload { get; set; }
    /// <summary>Timer for Manager, every x seconds Server is looking for new player</summary>
    private float Timer { get; set; }
    /// <summary>Get Networkmanager Singleton</summary>
    public static NetworkManager GetSingleton { get { return singleton; } }

    /// <summary>List with all players in Scene</summary>
    private static List<PlayerEntity> allPlayers = new List<PlayerEntity>();
    /// <summary>List with all players</summary>
    public static List<PlayerEntity> AllPlayers { get { return allPlayers; } }

    /// <summary>List with all players in Scene</summary>
    private static List<GameObject> allPlayersGo = new List<GameObject>();
    /// <summary>List with all players</summary>
    public static List<GameObject> AllPlayersGo { get { return allPlayersGo; } }

    /// <summary>List with all players in Game</summary>
    private static List<PlayerEntity> allPlayersPlaying = new List<PlayerEntity>();
    /// <summary>List with all players who are playing</summary>
    public static List<PlayerEntity> AllPlayersPlaying { get { return allPlayersPlaying; } }

    /// <summary>List with all players in Lobby</summary>
    private static List<PlayerEntity> allPlayersLobby = new List<PlayerEntity>();
    /// <summary>List with all players in lobby</summary>
    public static List<PlayerEntity> AllPlayersLobby { get { return allPlayersLobby; } }
    private void Start()
    {
        Timer = 30f;
    }

    private void Update()
    {
        if (Reload)
        {
            SearchPlayer(true);
            Debug.Log(AllPlayers.Count);
        }
        // reduce Timer. Every x seconds search for new player
        //Timer -= Time.deltaTime;
        //if (Timer <= 0)
        //{
        //    SearchPlayer(true);
        //    Timer = 30f;
        //}
        //Debug.Log(AllPlayers.Count + " / Lobby: " + allPlayersLobby.Count + " / Playing: " + allPlayersPlaying.Count);
    }

    /// <summary>
    /// Add player to lobby
    /// </summary>
    /// <param name="_player">The player</param>
    /// <param name="_setWannaPlay">true: Set <see cref="AEntity.wannaPlay"/> value to true</param>
    public static void AddPlayer(GameObject _player, bool _setWannaPlay = true)
    {
        PlayerEntity p = _player.GetComponent<PlayerEntity>();
        allPlayersPlaying.Add(p);
        allPlayersLobby.Remove(p);
        if(_setWannaPlay) p.wannaPlay = true;
    }

    /// <summary>
    /// Remove player from lobby
    /// </summary>
    /// <param name="_player">The player</param>
    public static void RemovePlayer(GameObject _player)
    {
        PlayerEntity p = _player.GetComponent<PlayerEntity>();
        allPlayersLobby.Add(p);
        allPlayersPlaying.Remove(p);
        p.wannaPlay = false;
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

        // set Reload to false to the server stop looking for new players next frame
        Reload = false;

        // reset list
        allPlayers.Clear();
        allPlayersGo.Clear();
        allPlayersPlaying.Clear();
        allPlayersLobby.Clear();

        foreach (GameObject go in tempGO)
        {
            if (go.name == "Player(Clone)")
            {
                // add to all player (go) list
                allPlayersGo.Add(go);
                allPlayers.Add(go.gameObject.GetComponent<PlayerEntity>());

                // add to playing or lobby list
                if (go.gameObject.transform.position.y > 400)
                {
                    // check if player wants to play
                    if (go.gameObject.GetComponent<PlayerEntity>().wannaPlay == true)
                    {
                        allPlayersPlaying.Add(go.GetComponent<PlayerEntity>());
                    }
                    else
                    {
                        allPlayersLobby.Add(go.GetComponent<PlayerEntity>());
                    }
                }
                else
                    allPlayersPlaying.Add(go.GetComponent<PlayerEntity>());
            }
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        allPlayersGo.Clear();
        allPlayers.Clear();
        allPlayersPlaying.Clear();
        allPlayersLobby.Clear();
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
        Reload = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Reload = true;
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
