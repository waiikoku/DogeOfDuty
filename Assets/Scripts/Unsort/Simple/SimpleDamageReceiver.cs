using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roguelike;

namespace Dod
{
    [RequireComponent(typeof(Collider))]
    public class SimpleDamageReceiver : MonoBehaviour, IDamagable
    {
        [SerializeField] private LifeStatus life;
        public void TakeDamage(float dmg)
        {
            life.RemoveHealth(dmg);
        }

        public void TakeDamageV3(Vector3 hitPoint, float dmg)
        {
            life.RemoveHealth(dmg);
        }

        public bool IsAlive()
        {
            return life.currentHealth > 0;
        }
    }
}
