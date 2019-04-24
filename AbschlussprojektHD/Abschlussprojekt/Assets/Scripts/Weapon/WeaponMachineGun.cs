using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using DT = System.DateTime;
using TS = System.TimeSpan;

public class WeaponMachineGun : AWeapon
{
    public override WeaponType GetWeapon { get { return WeaponType.MACHINEGUN; } }

    public override TS WaitTime { get { return new TS(0, 0, 0, 500); } }

    public override bool HasRapidFire { get { return true; } }

    public override int MaxAmmo { get { return 30; } }

    public override TS ReloadTime { get { return new TS(0, 0, 0, 1, 0); } }

    protected override int AmmoPerShot { get { return 1; } }

    [Client]
    public override bool Shoot()
    {
        // check if player is allowed to shoot
        if (CurrentAmmo <= 0 || IsReloading)
            return false;

        // if wait time is higher than times between last shot and this shot
        if (DT.Now - lastShot < WaitTime)
            return false;

        CurrentAmmo -= AmmoPerShot;
        lastShot = DT.Now;
        return true;
    }
}
