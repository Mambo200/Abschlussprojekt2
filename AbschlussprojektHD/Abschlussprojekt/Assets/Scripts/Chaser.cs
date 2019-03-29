using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Chaser : NetworkBehaviour {
    
    /// <summary>static Random</summary>
    private static readonly System.Random random = new System.Random();

    //[SyncVar]
    ///// <summary>Chaser Singleton</summary>
    //public static GameObject currentChaser;

    /// <summary>
    /// Chooses the chaser randomly. Eliminate players who do not have the lowest amount of Chaser players, afterwards choose randomly from pool
    /// </summary>
    [Server]
    public static void ChooseChaser()
    {
        #region Old Code
        /*

        // get playerentity from current chaser
        PlayerEntity pe;
        // set stats for last round chaser
        if (currentChaser != null)
        {
            pe = currentChaser.gameObject.GetComponent<PlayerEntity>();
            pe.SetChaser(false);
        }

        // get chaser pool and exclude current chaser
        List<GameObject> chaserPool = MyNetworkManager.AllPlayersGo;
        if (currentChaser != null)
            chaserPool.Remove(currentChaser);
        currentChaser = null;

        // copy of current chaser pool
        List<GameObject> poolCopy = chaserPool;

        // get lowest Chaser number and remove player who was chaser last round
        int lowest = int.MaxValue;
        foreach (GameObject itemG in poolCopy)
        {
            // get playerentity of gameobject
            PlayerEntity item = itemG.gameObject.GetComponent<PlayerEntity>();
            // check if player was chaser last round
            if (item.WasChaserLastRound)
            {
                // set chaser to false
                item.SetChaser(false);
                chaserPool.Remove(itemG);
                continue;
            }

            // check if chaser amount is lower than lowest value
            if (item.ChaserCount <= lowest)
            {
                // if true set lowest value
                lowest = item.ChaserCount;
            }
            else
            {
                // set chaser to false
                item.SetChaser(false);
                // if false remove this player from chaser pool
                chaserPool.Remove(itemG);
            }
        }

        // a Copy of the current chaser pool for foreach
        poolCopy = null;
        poolCopy = chaserPool;

        // check if players chasercount in List are equal lowest. if not remove from pool
        foreach (GameObject itemG in poolCopy)
        {
            PlayerEntity item = itemG.gameObject.GetComponent<PlayerEntity>();
            if (item.ChaserCount != lowest)
            {
                // set chaser mode to false
                item.SetChaser(false);
                // remove player from pool
                chaserPool.Remove(itemG);
            }
        }

        // delete poolcopy
        poolCopy = null;

        // check if no player is in pool
        if (chaserPool.Count == 0)
        {
            currentChaser = MyNetworkManager.AllPlayersGo[0];
        }
        else
        {
            // chose player randomly
            int chaserIndex = Random.Range(0, (chaserPool.Count - 1));

            // save player
            currentChaser = chaserPool[chaserIndex];
        }

        // set chaser
        currentChaser.gameObject.GetComponent<PlayerEntity>().SetChaser(true);

        // set all other players chaser status to false
        foreach (GameObject itemG in chaserPool)
        {
            PlayerEntity item = itemG.gameObject.GetComponent<PlayerEntity>();
            // if current item is the chaser continue
            if (item == currentChaser)
                continue;

            item.SetChaser(false);
        }
        */
        #endregion

        // Copy of all players
        List<PlayerEntity> chaserPool = MyNetworkManager.AllPlayerCopy();
        // copy of chaser pool
        List<PlayerEntity> peCopy;
        // create an array copy to copy the pool list to array and transfer array to list
        PlayerEntity[] peCopyArray = new PlayerEntity[chaserPool.Count];
        chaserPool.CopyTo(peCopyArray);
        peCopy = peCopyArray.ToList();

        foreach (PlayerEntity pe in peCopy)
        {
            // if player was chaser last round delete from pool
            if (pe.IsChaser || pe.WasChaserLastRound)
            {
                pe.SetChaser(false);
                chaserPool.Remove(pe);
            }
        }

        // Get lowest chasernumber
        peCopy.Clear();
        peCopyArray = new PlayerEntity[chaserPool.Count];
        chaserPool.CopyTo(peCopyArray);
        peCopy = peCopyArray.ToList();

        int lowest = int.MaxValue;
        foreach (PlayerEntity pe in peCopy)
        {
            // check if variable is lower than players chaser count.
            if (pe.ChaserCount <= lowest)
                // set new count
                lowest = pe.ChaserCount;
            else
            {
                pe.SetChaser(false);
                // remove player from list
                chaserPool.Remove(pe);
            }
        }

        // remove all players which does not match chasercount with lowest variable
        peCopy.Clear();
        peCopyArray = new PlayerEntity[chaserPool.Count];
        chaserPool.CopyTo(peCopyArray);
        peCopy = peCopyArray.ToList();
        foreach (PlayerEntity pe in peCopy)
        {
            if (pe.ChaserCount != lowest)
            {
                pe.SetChaser(false);
                chaserPool.Remove(pe);
            }
        }

        peCopy = null;

        // if pool bigger that 1 choose randomly
        if (chaserPool.Count == 0)
        {
            Debug.LogWarning("No Chaser found!");
            MyNetworkManager.AllPlayers[0].SetChaser(true);
        }
        else if (chaserPool.Count == 1)
        {
            PlayerEntity p = chaserPool[0];
            p.SetChaser(true);
            chaserPool.Remove(p);
        }
        else
        {
            // chose player randomly
            int chaserIndex = Random.Range(0, (chaserPool.Count - 1));

            // save player
            PlayerEntity p = chaserPool[chaserIndex];

            // set player to chaser
            p.SetChaser(true);

            // remove from list
            chaserPool.Remove(p);
        }

        foreach (PlayerEntity pe in chaserPool)
        {
            pe.SetChaser(false);
        }
    }
}
