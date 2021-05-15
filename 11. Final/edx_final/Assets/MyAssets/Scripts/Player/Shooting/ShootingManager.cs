using helpers;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace player.shooting
{
    public class ShootingManager : MonoBehaviour
    {

        // ========================== Weapons configuration ============================

        [Header("Weapon Installs")]
        [SerializeField] private WeaponInstall _weaponLeft;
        [SerializeField] private WeaponInstall _weaponCenter;
        [SerializeField] private WeaponInstall _weaponRight;

        private Dictionary<WeaponPosition, WeaponInstall> _weapons;
        private Pool<Weapon> _weaponsPool;

        private void Awake()
        {
            _weaponsPool = new Pool<Weapon>(3, "Prefabs/Weapon");

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
            public Quaternion rotation = Quaternion.identity;

            private Weapon _weapon;
            public Weapon weapon
            {
                get => _weapon;
                set
                {
                    _weapon = value;
                    _weapon.gameObject.transform.SetParent(position, false);
                    _weapon.gameObject.transform.rotation = rotation;
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