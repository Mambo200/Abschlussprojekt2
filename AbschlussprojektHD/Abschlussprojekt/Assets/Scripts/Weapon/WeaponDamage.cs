using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class WeaponDamage
{
	public static float Damage(AWeapon.WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case AWeapon.WeaponType.MACHINEGUN:
                return 2.4f;

            case AWeapon.WeaponType.KATANA:
                return 10.0f;
            // broken things
            default:
                throw new System.NotImplementedException(_weaponType + " is not valid");
        }
    }

    public static float WaitTime(AWeapon.WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case AWeapon.WeaponType.MACHINEGUN:
                return 1.100f;

            case AWeapon.WeaponType.KATANA:
                return 0f;

            // broken things
            default:
                throw new System.NotImplementedException(_weaponType + " is not valid");
        }
    }
}
