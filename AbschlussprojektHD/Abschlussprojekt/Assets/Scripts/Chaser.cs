using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chaser : PlayerEntity {
    
    /// <summary>static Random</summary>
    private static readonly System.Random random = new System.Random();
    /// <summary>Chaser Singleton</summary>
    private static Chaser _single;
    private static object _lock = new object();

    /// <summary>Chaser Singleton Getter</summary>
    public static Chaser Get
    {
        [Server]
        get
        {
            if (_single == null)
            {
                lock (_lock)
                {
                    if (_single == null)
                    {
                        _single = new Chaser();
                    }
                }
            }

            return _single;
        }
    }

    /// <summary>Chaser Singleton</summary>
    public static PlayerEntity currentChaser;

    /// <summary>
    /// Chooses the chaser randomly. Eliminate players who do not have the lowest amount of Chaser players, afterwards choose randomly from pool
    /// </summary>
    [Server]
    public void ChooseChaser()
    {
        // set stats for last round chaser
        if (currentChaser != null)
            currentChaser.SetChaser(false);

        // get chaser pool and exclude current chaser
        List<PlayerEntity> chaserPool = MyNetworkManager.AllPlayers;
        chaserPool.Remove(currentChaser);
        currentChaser = null;

        // copy of current chaser pool
        List<PlayerEntity> poolCopy = chaserPool;

        // get lowest Chaser number and remove player who was chaser last round
        int lowest = int.MaxValue;
        foreach (PlayerEntity item in poolCopy)
        {
            // check if player was chaser last round
            if (item.WasChaserLastRound)
            {
                // set chaser to false
                item.SetChaser(false);
                chaserPool.Remove(item);
                continue;
            }

            // check if chaser amount is lower than lowest value
            if (item.ChaserCount <= lowest)
            {
                {

                }
                // if true set lowest value
                lowest = ChaserCount;
            }
            else
            {
                // set chaser to false
                item.SetChaser(false);
                // if false remove this player from chaser pool
                chaserPool.Remove(item);
            }
        }

        // a Copy of the current chaser pool for foreach
        poolCopy = null;
        poolCopy = chaserPool;

        // check if players chasercount in List are equal lowest. if not remove from pool
        foreach (PlayerEntity item in poolCopy)
        {
            if (item.ChaserCount != lowest)
            {
                // set chaser mode to false
                item.SetChaser(false);
                // remove player from pool
                chaserPool.Remove(item);
            }
        }

        // delete poolcopy
        poolCopy = null;

        // check if no player is in pool
        if (chaserPool.Count == 0)
        {
            currentChaser = MyNetworkManager.AllPlayers[0];
        }
        else
        {
            // chose player randomly
            int chaserIndex = Random.Range(0, (chaserPool.Count - 1));

            // save player
            currentChaser = chaserPool[chaserIndex];
        }

        // set chaser
        currentChaser.SetChaser(true);

        // set all other players chaser status to false
        foreach (PlayerEntity item in chaserPool)
        {
            // if current item is the chaser continue
            if (item == currentChaser)
                continue;

            item.SetChaser(false);
        }
    }
}
