using helpers;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace behaviors.shooting
{
    public class ShootingManager : MonoBehaviour
    {

        // ========================== Weapons configuration ============================

        [SerializeField] private GameObject _weaponPrefab;

        [Header("Weapon Installs")]
        [SerializeField] private WeaponInstall _weaponLeft;
        [SerializeField] private WeaponInstall _weaponCenter;
        [SerializeField] private WeaponInstall _weaponRight;

        private Dictionary<WeaponPosition, WeaponInstall> _weapons;
        private Pool<Weapon> _weaponsPool;

        private void Awake()
        {
            if (_weaponPrefab != null)
                _weaponsPool = new Pool<Weapon>(3, _weaponPrefab);
            else
                _weaponsPool = new Pool<Weapon>(3, Constants.PREFAB_WEAPON);

            _weapons = new Dictionary<WeaponPosition, WeaponInstall>()
            {
                { WeaponPosition.LEFT, _weaponLeft },
                { WeaponPosition.CENTER, _weaponCenter },
                { WeaponPosition.RIGHT, _weaponRight },
            };
        }

        // ========================== Weapons instantiation ============================

        public void InstallWeapon(WeaponPosition position)
        {
            if (_weapons[position].weapon != null) RemoveWeapon(position);
            _weapons[position].weapon = _weaponsPool.Instantiate();
        }

        public void RemoveWeapon(WeaponPosition position)
        {
            if (_weapons[position].weapon != null)
                _weaponsPool.Destroy(_weapons[position].weapon);
        }

        public enum WeaponPosition
        {
            LEFT,
            CENTER,
            RIGHT
        }

        [System.Serializable]
        public class WeaponInstall
        {
            public Transform position;

            private Weapon _weapon;
            public Weapon weapon
            {
                get => _weapon;
                set
                {
                    _weapon = value;
                    _weapon.gameObject.transform.SetParent(position, false);
                }
            }
        }

        // ========================== Fire ============================

        public void Fire()
        {
            foreach (var item in _weapons.Values)
            {
                if (item.weapon != null)
                    item.weapon.Fire();
            }
        }

        public void ChargeFire()
        {
            foreach (var item in _weapons.Values)
            {
                if (item.weapon != null)
                    item.weapon.ChargeFire();
            }
        }
    }
}