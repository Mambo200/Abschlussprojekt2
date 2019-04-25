using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Weapon
{
    public abstract class AGun : AWeapon
    {
        public bool IsReloading { get; protected set; }

        /// <summary>amount of max Ammo the player can have</summary>
        public abstract int MaxAmmo { get; }

        private int currentAmmo;
        /// <summary>amount of current Ammo the player can have</summary>
        public int CurrentAmmo
        {
            get { { return currentAmmo; } }
            set { { currentAmmo = (value < 0) ? 0 : value; } }
        }

        protected abstract int AmmoPerShot { get; }

        /// <summary>The time when player start reload</summary>
        protected float reloadStart;

        /// <summary>The time need to reload the weapon</summary>
        public abstract float ReloadTime { get; }
    }
}
