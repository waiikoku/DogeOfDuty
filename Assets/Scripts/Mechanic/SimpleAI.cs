using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Roguelike
{
    public class SimpleAI : MonoBehaviour, IDamagable
    {
        private NavMeshAgent agent;

        [SerializeField] private string targetTag = "Untagged";
        private Transform target;

        [SerializeField] private bool manualAI = false;

        [SerializeField] private LifeStatus lifeStat;
        //[SerializeField] private Vector2 randomDiedTime;
        //[SerializeField] private Dropitem itemDrop; //Out-dated

        private float distance;
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private Vector2 bodyHitRange;

        [SerializeField] private Animator anim;
        private int animID_shoot;
        private int animID_attack;
        private int animID_move;
        private int animID_dmg;
        private int animID_die;

        public System.Action<float> OnDamage;
        private void Awake()
        {
            AnimationIDs();
            Setup();
        }

        private void Start()
        {
            lifeStat.OnHealthDepleted += Died;
            var go = GameObject.FindGameObjectWithTag(targetTag);
            if (go != null) target = go.transform;
            anim.SetBool(animID_move, true);
            //Invoke(nameof(Died), Random.Range(randomDiedTime.x, randomDiedTime.y));
        }

        private void Setup()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void AnimationIDs()
        {
            animID_move = Animator.StringToHash("Move");
            animID_shoot = Animator.StringToHash("Shoot");
            animID_attack = Animator.StringToHash("Attack");
            animID_dmg = Animator.StringToHash("Take Damage");
            animID_die = Animator.StringToHash("Die");
            attackHash = Animator.StringToHash(attackId);
        }

        private void Died()
        {
            anim.SetTrigger(animID_die);
            lifeStat.OnHealthDepleted -= Died;
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.Killed();
            }
            /*
            var reward = Instantiate(itemDrop, transform.position, Quaternion.Euler(0,45,0));
            reward.SetAmount(Random.Range(1, 10));
            */
            Destroy(gameObject);
        }
        public float speed;
        public float threshold = 0.1f;
        private void FixedUpdate()
        {
            if (manualAI)
            {
                if (target == null)
                {
                    agent.isStopped = true;
                }
                else
                {
                    Movement();
                    Combat();
                }
            }
        }

        private void Movement()
        {
            distance = Vector3.Distance(transform.position, target.position);
            agent.SetDestination(target.position);
            speed = agent.velocity.sqrMagnitude;
            anim.SetBool(animID_move, speed > threshold);
        }

        [Header("Combat")]
        [SerializeField] private float attackDist = 3f;
        [SerializeField] private float attackCD = 2f;
        private float attackCdTime;
        private const string attackId = "Smash Attack";
        private int attackHash;
        private void Combat()
        {
            if (attackDist > distance) return;
            if (attackCdTime > Time.time) return;
            attackCdTime = Time.time + attackCD;
            Attack();
        }

        private const string playerTag = "Player";
        [SerializeField] private BoxCollider hitBox;
        private void Attack()
        {
            anim.SetTrigger(attackHash);
            Collider[] cols = Physics.OverlapBox(hitBox.transform.position, hitBox.size / 2f, hitBox.transform.rotation);
            foreach (var col in cols)
            {
                if (col.CompareTag(playerTag) == false) continue;
                if (col.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(1);
                }
            }
        }
        /*
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(targetTag))
            {
                if (collision.collider.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(Random.Range(1, 15));
                }
            }
        }
        */
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (other.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(Random.Range(bodyHitRange.x,bodyHitRange.y));
                }
            }
        }

        public void TakeDamage(float dmg)
        {
            anim.SetTrigger(animID_dmg);
            lifeStat.RemoveHealth(dmg);
            OnDamage?.Invoke(dmg);
        }

        public void TakeDamageV3(Vector3 hitPoint, float dmg)
        {
            anim.SetTrigger(animID_dmg);
            lifeStat.RemoveHealth(dmg);
            OnDamage?.Invoke(dmg);
        }

        public bool IsAlive()
        {
            return lifeStat.currentHealth > 0;
        }

        public void SetSpeed(float speed)
        {
            agent.speed = speed;
        }

        public void SetRotateSpeed(float speed)
        {
            agent.angularSpeed = speed;
        }
    }
}
