using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMolotov2 : BaseSkill
    {
        [SerializeField] private Rigidbody molotov;
        public float spawnRate = 1;
        private float spawnTime;
        private float spawnInterval;

        public float bottleAmount = 1;
        public float projectileSpeed = 10;
        public float projectileAngle = 45;
        public float molotovDmg;

        [SerializeField] private Vector3 offset;
        [SerializeField] private float offsetRadius;
        [SerializeField] private SphereCollider zone;
        public string targetTag = "Untagged";
        public LayerMask targetLayer;

        //Caches
        private Vector3 lastPosition;
        private Vector3 lastForward;
        private Vector3 direction;
        private float distance;
        private Collider unconfirmTarget;

        private List<Collider> possibleTargets;
        private Collider leastTarget;
        private int indexFE;
        private Rigidbody projectile;
        private Vector3 throwAngle;

        public int FUCKINGLEVEL;

        [System.Serializable]
        private struct MolotovAttribute
        {
            public float damage;
            public float radius;
            public int amount;
        }
        [SerializeField] private MolotovAttribute[] attributes;
        protected override void Awake()
        {
            base.Awake();
            possibleTargets = new List<Collider>();
        }
        public override void Activation(bool active)
        {
            if (isCooldown) return;
            if (active)
            {
                OnCooldown.coroutine = StartCoroutine(Cooldown());
                OnActive.coroutine = StartCoroutine(Active());
            }
            else
            {
                if (OnActive.coroutine != null)
                {
                    StopCoroutine(OnActive.coroutine);
                }
            }
        }

        protected override void ActiveStart()
        {
            spawnInterval = 1f / spawnRate;
            print("Start Molotov");
        }

        protected override void ActiveEnd()
        {
            base.ActiveEnd();
        }

        protected override void ActiveUpdate()
        {
            if (m_param.center == null) return;
            if (spawnTime > Time.time) return;
            spawnTime = Time.time + spawnInterval;
            lastPosition = m_param.center.position;
            lastForward = m_param.center.forward;
            Search();
            indexFE = 0;
            Quaternion throwRot = Quaternion.Euler(projectileAngle, m_param.center.rotation.eulerAngles.y,0);
            throwAngle = throwRot * Vector3.up;
            foreach (var target in possibleTargets)
            {
                if (indexFE >= bottleAmount)
                {
                    print($"Exceed bottle ({indexFE})");
                    return;
                }
                if (target == null) continue;
                Vector3 dest = target.ClosestPoint(lastPosition);
                Vector3 dir = dest - lastPosition;
                Vector3 point = dir.normalized * offsetRadius;
                Fire(lastPosition + point, dir);
                indexFE++;
            }
        }

        private void Search()
        {
            Collider[] cols = Physics.OverlapSphere(lastPosition, zone.radius, targetLayer);
            distance = Mathf.Infinity;
            unconfirmTarget = null;
            possibleTargets.Clear();
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag))
                {
                    direction = col.transform.position - lastPosition;
                    direction.y = 0;
                    if (direction.magnitude < distance)
                    {
                        distance = direction.magnitude;
                        unconfirmTarget = col;
                    }
                    possibleTargets.Add(col);
                }
            }
            leastTarget = unconfirmTarget;
        }
        private int count;
        private void Fire(Vector3 source, Vector3 direction)
        {
            count++;
            projectile = Instantiate(molotov, offset + source, Quaternion.LookRotation(direction, Vector3.up));
            projectile.SendMessage("SetDamage", molotovDmg);
            projectile.velocity = projectileSpeed * (direction.normalized + throwAngle);
        }

        public override void Upgrade()
        {
            base.Upgrade();
            bottleAmount += 1;
            /*
            int index = m_level;
            if (index > 0) index -= 1;
            molotovDmg = attributes[index].damage;
            bottleAmount = attributes[index].amount;
            */
        }

        public override void Degrade()
        {
            base.Degrade();
            //slot.UpdateStar(m_level);
        }

        /*
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Upgrade"))
            {
                Upgrade();
            }
            if (GUILayout.Button("Degrade"))
            {
                Degrade();
            }
            GUILayout.EndVertical();
        }
        */
    }
}
