using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class ASword : AWeapon
    {
        public override MainWeaponType GetMainWeapon { get { return MainWeaponType.SWORD; } }

        public override string AmmoText { get { return "∞ / ∞"; } }

        public override bool Shoot()
        {
            if (!base.Shoot()) return false;

            // if wait time is higher than times between last shot and this shot
            if (Time.time - lastShot < WaitTime)
                return false;

            lastShot = Time.time;
            return true;
        }
    }
}
