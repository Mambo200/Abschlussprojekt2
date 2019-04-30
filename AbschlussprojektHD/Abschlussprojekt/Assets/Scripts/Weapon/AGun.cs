﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class AGun : AWeapon
    {
        public override MainWeaponType GetMainWeapon { get { return MainWeaponType.GUN; } }

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

        /// <summary>Text for UI</summary>
        public override string AmmoText
        {
            get { return CurrentAmmo + " / " + MaxAmmo; }
        }

        public override bool Shoot()
        {
            // return if player is reloading
            if (IsReloading)
                return false;
            if (m_Player.AmmoTextBox.gameObject.activeSelf == false)
            {
                return false;
            }

            // check if player is allowed to shoot
            if (CurrentAmmo <= 0 || IsReloading)
                return false;

            CurrentAmmo -= AmmoPerShot;
            lastShot = Time.time;
            m_Player.AmmoTextBox.text = AmmoText;
            return true;
        }
        protected override void Update()
        {
            base.Update();

            if (IsReloading)
            {
                if (Time.time - reloadStart >= ReloadTime)
                {
                    Debug.Log("Reloading");
                    CurrentAmmo = MaxAmmo;
                    IsReloading = false;
                    ReloadEnd();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                reloadStart = Time.time;
                IsReloading = true;
                ReloadStart();
            }
        }

        protected void ReloadStart()
        {
            m_Player.AmmoTextBox.text = "Reloading";
        }

        protected void ReloadEnd()
        {
            m_Player.AmmoTextBox.text = AmmoText;
        }

        private void Start()
        {
            CurrentAmmo = MaxAmmo;
        }
    }
}
