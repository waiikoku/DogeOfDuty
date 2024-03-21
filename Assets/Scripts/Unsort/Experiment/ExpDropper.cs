using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpDropper : MonoBehaviour
    {
        [SerializeField] private LifeStatus life;
        [SerializeField] private ExpDropItem item;
        [SerializeField] private Vector2 xpRange;
        private void Awake()
        {
            life.OnHealthDepleted += Drop;
            life.OnHealthDepleted += SurvivalManager.Instance.AddKill;
        }

        private void OnDestroy()
        {
            life.OnHealthDepleted = null;
        }

        private void Drop()
        {
            Vector3 pos = transform.position;
            Vector3 dir = -transform.forward + new Vector3(0,1f,0);
            var go = Instantiate(item, pos, Quaternion.identity);
            go.SetExp((int)Random.Range(xpRange.x, xpRange.y));
            go.Knockback(dir, 5f, 0.2f,() => go.Activate(true));
        }
    }
}
