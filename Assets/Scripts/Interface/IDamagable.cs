using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike
{
    public interface IDamagable
    {
        public void TakeDamage(float dmg);

        public void TakeDamageV3(Vector3 hitPoint, float dmg);

        public bool IsAlive();
    }
}
