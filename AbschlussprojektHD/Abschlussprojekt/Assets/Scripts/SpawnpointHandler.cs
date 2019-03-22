using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnpointHandler : NetworkBehaviour{

    private static int index = 0;
    private static int Index
    {
        get { return index; }
        set
        {
            // if index is bigger than player array set index to 0
            if (value >= MyNetworkManager.AllPlayers.Count)
                index = 0;
            else
                index = value;
        }
    }

    private static SpawnpointHandler _single;
    private static object _lock = new object();
    /// <summary>SpawnpointHandler Singleton Getter</summary>
    //public static SpawnpointHandler Get
    //{
    //    [Server]
    //    get
    //    {
    //        if (_single == null)
    //        {
    //            lock (_lock)
    //            {
    //                if (_single == null)
    //                {
    //                    _single = new SpawnpointHandler();
    //                }
    //            }
    //        }
    //
    //        return _single;
    //    }
    //}

    /// <summary>List of all spawnpoints</summary>
    private static List<Transform> m_spawnPoints = new List<Transform>();

    [Server]
    public void Start()
    {
        // fill spawnpoint gameobjects
        Transform[] goArray = GetComponentsInChildren<Transform>();
        foreach (Transform go in goArray)
        {
            if (go != this.gameObject)
            {
                m_spawnPoints.Add(go);
            }
        }

        Debug.Log("Spawnfield count: " + m_spawnPoints.Count);
    }

    /// <summary>
    /// Get the next spawnpoint. set new index
    /// </summary>
    /// <returns></returns>
    [Server]
    public static Vector3 NextSpawnpoint()
    {
        Vector3 toReturn = m_spawnPoints[Index].position;
        Index++;
        return toReturn;
    }

}
