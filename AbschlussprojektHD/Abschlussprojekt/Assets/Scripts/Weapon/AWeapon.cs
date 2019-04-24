using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

// (current) ammo, reload time

public abstract class AWeapon
{
    protected object m_currentAmmoLock = new object();

    /// <summary>Type of Weapon</summary>
    public enum WeaponType { REVOLVER, MACHINEGUN, PISTOL, UNDIFINED}

    /// <summary>Get the weapon.</summary>
    /// <value><see cref="WeaponType"/></value>
    public abstract WeaponType GetWeapon { get; }

    /// <summary>Time of last shot</summary>
    protected System.DateTime lastShot;

    /// <summary>Time player has to wait before next shot can be fired</summary>
    public abstract System.TimeSpan WaitTime { get; }
    
    /// <summary>Damage of Weapon. Uses <see cref="WeaponDamage.Damage(WeaponType)"/></summary>
    public float Damage { get { return WeaponDamage.Damage(GetWeapon); } }

    /// <summary>Return true if you can hold down the button</summary>
    public abstract bool HasRapidFire { get; }

    public bool IsReloading { get; protected set; }

    /// <summary>amount of max Ammo the player can have</summary>
    public abstract int MaxAmmo { get; }

    private int currentAmmo;
    /// <summary>amount of current Ammo the player can have</summary>
    public int CurrentAmmo
    {
        get { lock (m_currentAmmoLock) { return currentAmmo; } }
        set { lock (m_currentAmmoLock) { currentAmmo = (value < 0) ? 0 : value; } }
    }

    protected abstract int AmmoPerShot { get; }

    /// <summary>The time when player start reload</summary>
    protected System.DateTime reloadStart;

    /// <summary>The time need to reload the weapon</summary>
    public abstract System.TimeSpan ReloadTime { get; }

    [Client]
    public abstract bool Shoot();

    public virtual void StartReloading()
    {
        IsReloading = true;
        Thread t = new Thread(Reload);
        t.Start();
    }

    private void Reload()
    {
        IsReloading = true;
        Thread.Sleep(ReloadTime);
        IsReloading = false;

        CurrentAmmo = MaxAmmo;
    }

}
