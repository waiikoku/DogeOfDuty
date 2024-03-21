using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpWeapon1 : MonoBehaviour
    {
        [Header("Identity")]
        public int weaponID;
        public string weaponName;
        public Sprite weaponIcon;
        [TextArea]
        public string weaponDesc;

        private enum WeaponType
        {
            Undefined = -1,
            Pistol = 0,
            Rifle,
            Shotgun,
            Sniper,
            Gatling,
            Rocket,
            Laser
        }

        [SerializeField] private WeaponType weaponType = WeaponType.Undefined;

        [Header("Stats")]
        public WeaponStat stat;
        public struct WeaponStat
        {
            public float dmg;
            public float fireRate;
            public int magazine;
            public float reloadTime;
        }

        public int tier;
    }
}
