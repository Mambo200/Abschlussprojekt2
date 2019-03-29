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
