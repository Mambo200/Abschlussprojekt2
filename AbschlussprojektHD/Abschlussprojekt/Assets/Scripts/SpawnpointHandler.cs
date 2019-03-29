using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnpointHandler : NetworkBehaviour{

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
            if (value >= m_spawnPoints.Count)
                index = 0;
            else
                index = value;
        }
    }
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

    /// <summary>List of all Player spawnpoints</summary>
    private static readonly List<Transform> m_spawnPoints = new List<Transform>();
    /// <summary>List of all spawnpoints</summary>
    private static readonly List<Transform> m_chaserSpawn = new List<Transform>();


    [Server]
    public void Start()
    {
        // clear list
        m_spawnPoints.Clear();

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
                    m_spawnPoints.Add(go);
            }
        }

        Debug.Log("Spawnfield count: " + m_spawnPoints.Count);
    }

    /// <summary>
    /// Get the next spawnpoint of Player. set new index
    /// </summary>
    /// <returns></returns>
    [Server]
    public static Vector3 NextSpawnpoint()
    {
        Vector3 toReturn = m_spawnPoints[Index].position;
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
    /// Get all Spawnpoints in game and set them to <see cref="m_spawnPoints"/> and <see cref="m_chaserSpawn"/>
    /// </summary>
    [Server]
    private void GetSpawnPoints()
    {
        // clear list
        m_spawnPoints.Clear();

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
                    m_spawnPoints.Add(go);
            }
        }

        Debug.Log("Spawnfield count: " + m_spawnPoints.Count);
    }

}
