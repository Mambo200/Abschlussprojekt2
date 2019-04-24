using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponDamage
{
	public static float Damage(AWeapon.WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case AWeapon.WeaponType.REVOLVER:
                return 20f;
            case AWeapon.WeaponType.MACHINEGUN:
                return 2.4f;
            case AWeapon.WeaponType.PISTOL:
                return 5.1f;
            case AWeapon.WeaponType.UNDIFINED:
                throw new System.NotImplementedException("Weapon can not be undefined");
            default:
                throw new System.NotImplementedException(_weaponType + " is not valid");
        }
    }

    public static System.TimeSpan WaitTime(AWeapon.WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case AWeapon.WeaponType.REVOLVER:
                return new System.TimeSpan(0, 0, 0, 1, 100);
            case AWeapon.WeaponType.MACHINEGUN:
                return new System.TimeSpan(0, 0, 0, 0, 350);
            case AWeapon.WeaponType.PISTOL:
                return new System.TimeSpan(0, 0, 0, 0, 800);
            case AWeapon.WeaponType.UNDIFINED:
                throw new System.NotImplementedException("Weapon can not be undefined");
            default:
                throw new System.NotImplementedException(_weaponType + " is not valid");
        }
    }
}
