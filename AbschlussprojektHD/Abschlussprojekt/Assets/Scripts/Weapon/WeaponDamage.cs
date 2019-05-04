using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class WeaponDamage
{
	public static float Damage(AWeapon.WeaponName _weaponType)
    {
        switch (_weaponType)
        {
            case AWeapon.WeaponName.MACHINEGUN:
                return 2.4f;

            case AWeapon.WeaponName.KATANA:
                return 20.0f;
            // broken things
            default:
                throw new System.NotImplementedException(_weaponType + " is not valid");
        }
    }

    //public static float WaitTime(AWeapon.WeaponName _weaponType)
    //{
    //    switch (_weaponType)
    //    {
    //        case AWeapon.WeaponName.MACHINEGUN:
    //            return 1.100f;
    //
    //        case AWeapon.WeaponName.KATANA:
    //            return 0f;
    //
    //        // broken things
    //        default:
    //            throw new System.NotImplementedException(_weaponType + " is not valid");
    //    }
    //}
}
