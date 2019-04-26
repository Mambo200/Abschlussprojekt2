using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    class Katana : ASword
    {
        public override WeaponType GetWeapon { get { return WeaponType.KATANA; } }

        public override float WaitTime { get { return 0.8f; } }

        public override bool HasRapidFire { get { return false; } }

        public override bool Shoot()
        {
            // if wait time is higher than times between last shot and this shot
            if (Time.time - lastShot < WaitTime)
                return false;

            lastShot = Time.time;
            return true;
        }
    }
}
