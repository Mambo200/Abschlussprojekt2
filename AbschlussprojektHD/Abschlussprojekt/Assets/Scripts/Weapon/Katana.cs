using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    class Katana : ASword
    {
        public override WeaponName GetWeaponName { get { return WeaponName.KATANA; } }

        public override float WaitTime { get { return 0.8f; } }

        public override bool HasRapidFire { get { return false; } }


        public override bool Shoot()
        {
            return base.Shoot();
        }
    }
}
