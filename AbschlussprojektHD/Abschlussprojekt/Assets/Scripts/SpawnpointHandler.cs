using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnpointHandler : NetworkBehaviour{

    /// <summary>Index of Player Spawn in Scene</summary>
    private static int index = 0;
    private static int Index
    {
        get
        {
            // if no spawn points can be found search them
            return index;
        }
        set
        {
            // if index is bigger than Spawnpoint array set index to 0
            if (value >= m_playerSpawn.Count)
                index = 0;
            else
                index = value;
        }
    }
    /// <summary>Index of Chaser Spawn in Scene</summary>
    private static int indexChaser = 0;
    private static int IndexChaser
    {
        get
        {
            // if no spawn points can be found search them
            return indexChaser;
        }
            set
        {
            // if index is bigger than player array set index to 0
            if (value >= m_chaserSpawn.Count)
                indexChaser = 0;
            else
                indexChaser = value;
        }
    }
    /// <summary>Index of Player Spawn in Lobby</summary>
    private static int indexLobby = 0;
    private static int IndexLobby
    {
        get
        {
            // if no spawn points can be found search them
            return indexLobby;
        }
        set
        {
            // if index is bigger than player array set index to 0
            if (value >= m_lobbySpawn.Count)
                indexLobby = 0;
            else
                indexLobby = value;
        }
    }


    /// <summary>List of all Player spawnpoints</summary>
    private static readonly List<Transform> m_playerSpawn = new List<Transform>();
    /// <summary>List of all chaser spawnpoints</summary>
    private static readonly List<Transform> m_chaserSpawn = new List<Transform>();
    /// <summary>List of all lobby spawnpoints</summary>
    private static readonly List<Transform> m_lobbySpawn = new List<Transform>();

    [Server]
    public void Start()
    {
        // clear list
        ClearAll();

        // fill spawnpoint gameobjects
        Transform[] goArray = GetComponentsInChildren<Transform>();
        foreach (Transform go in goArray)
        {
            if (go != this.gameObject.transform)
            {
                // If chaserspawn add to chaser spawn list
                if (go.name == "SpawnPointChaser")
                    m_chaserSpawn.Add(go);
                // if playerspawn add to player spawn list
                else if (go.name == "SpawnPointPlayer")
                    m_playerSpawn.Add(go);
                // if lobbyspawn add to lobby spawn list
                else if (go.name == "SpawnPointLobby")
                    m_lobbySpawn.Add(go);
            }
        }

        Debug.Log("Player Spawn count: " + m_playerSpawn.Count);
        Debug.Log("Chaser Spawn count: " + m_chaserSpawn.Count);
        Debug.Log("Lobby Spawn count: " + m_lobbySpawn.Count);
    }

    /// <summary>
    /// Get the next spawnpoint of Player. set new index
    /// </summary>
    /// <returns></returns>
    [Server]
    public static Vector3 NextSpawnpointPlayer()
    {
        Vector3 toReturn = m_playerSpawn[Index].position;
        Index++;
        return toReturn;
    }

    /// <summary>
    /// Get the next spawnposition of Chaser. set new index
    /// </summary>
    [Server]
    public static Vector3 NextChaserpoint()
    {
        Vector3 toReturn = m_chaserSpawn[IndexChaser].position;
        IndexChaser++;
        return toReturn;
    }

    /// <summary>
    /// Get the next spawnposition of Lobby. set new index
    /// </summary>
    [Server]
    public static Vector3 NextLobbypoint()
    {
        Vector3 toReturn = m_lobbySpawn[IndexLobby].position;
        IndexLobby++;
        return toReturn;
    }

    /// <summary>
    /// Get all Spawnpoints in game and set them to <see cref="m_playerSpawn"/> and <see cref="m_chaserSpawn"/>
    /// </summary>
    [Server]
    private void GetSpawnPoints()
    {
        // clear list
        m_playerSpawn.Clear();

        // fill spawnpoint gameobjects
        Transform[] goArray = GetComponentsInChildren<Transform>();
        foreach (Transform go in goArray)
        {
            if (go != this.gameObject.transform)
            {
                // If chaserspawn add to chaser spawn list
                if (go.name == "SpawnPointChaser")
                    m_chaserSpawn.Add(go);
                // else add to player spawn
                else
                    m_playerSpawn.Add(go);
            }
        }

        Debug.Log("Spawnfield count: " + m_playerSpawn.Count);
    }

    /// <summary>
    /// Clear every spawn list
    /// </summary>
    private static void ClearAll()
    {
        m_playerSpawn.Clear();
        m_chaserSpawn.Clear();
        m_lobbySpawn.Clear();
    }
}
