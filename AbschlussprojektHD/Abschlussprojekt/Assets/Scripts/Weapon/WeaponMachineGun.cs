﻿using Assets.Scripts.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMachineGun : AGun
{
    public override WeaponType GetWeapon { get { return WeaponType.MACHINEGUN; } }

    public override float WaitTime { get { return 0.5f; } }

    public override bool HasRapidFire { get { return true; } }

    public override int MaxAmmo { get { return 30; } }

    public override float ReloadTime { get { return 1f; } }

    protected override int AmmoPerShot { get { return 1; } }

    [Client]
    public override bool Shoot()
    {
        // check if player is allowed to shoot
        if (CurrentAmmo <= 0 || IsReloading)
            return false;

        // if wait time is higher than times between last shot and this shot
        if (lastShot - Time.time < WaitTime)
            return false;

        CurrentAmmo -= AmmoPerShot;
        lastShot = Time.time;
        return true;
    }

    protected override void Update()
    {
        base.Update();

        if (IsReloading)
        {
            if (Time.time - reloadStart >= ReloadTime)
            {
                CurrentAmmo = MaxAmmo;
                IsReloading = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            IsReloading = true;
            reloadStart = Time.time;
        }
    }
}
