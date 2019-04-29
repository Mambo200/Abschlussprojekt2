using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;

// (current) ammo, reload time

public abstract class AWeapon : MonoBehaviour
{

    /// <summary>Type of Weapon</summary>
    [Flags]
    public enum WeaponType
    {
        //GUN = 1 << 0,
        MACHINEGUN = 1 << 1,

        //SWORD = 1 << 100,
        KATANA = 1 << 101
    }

    /// <summary>Player of weapon</summary>
    [SerializeField]
    protected AEntity m_Player;

    /// <summary>Get the weapon.</summary>
    /// <value><see cref="WeaponType"/></value>
    public abstract WeaponType GetWeapon { get; }

    /// <summary>Time of last shot</summary>
    protected float lastShot;

    public abstract string AmmoText { get; }

    /// <summary>Time player has to wait before next shot can be fired</summary>
    public abstract float WaitTime { get; }
    
    /// <summary>Damage of Weapon. Uses <see cref="WeaponDamage.Damage(WeaponType)"/></summary>
    public float Damage { get { return WeaponDamage.Damage(GetWeapon); } }

    /// <summary>Return true if you can hold down the button</summary>
    public abstract bool HasRapidFire { get; }

    public virtual bool Shoot()
    {
        // if wait time is higher than times between last shot and this shot
        if (Time.time - lastShot < WaitTime)
            return false;
        else
            return true;
    }

    protected virtual void Update()
    {

    }
}
