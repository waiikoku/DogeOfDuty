using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dod
{
    public class Mech : MonoBehaviour
    {
        private Transform m_transform;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator anim;

        [SerializeField] private string targetTag = "Untagged";
        [SerializeField] private LayerMask targetLayer;

        [SerializeField] private BoxCollider hitBox;
        [SerializeField] private SphereCollider hitZone;
        [SerializeField] private AudioSource player;

        //Animation
        private float desireSpeed;
        private float currentSpeed;
        private float velocitySpeed;
        [SerializeField] private float smoothTime = .1f;
        private void Awake()
        {
            m_transform = transform;
        }

        private void Start()
        {
            ChangeState(State.Idle);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("Fire");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                desireSpeed = 0;
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                desireSpeed = 1;
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                desireSpeed = 2;
            }
        }
        private void FixedUpdate()
        {
            switch (currentStage)
            {
                case Stage.Update:
                    UpdateState();
                    break;
                default:
                    break;
            }

        }
        private void LateUpdate()
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, desireSpeed, ref velocitySpeed, smoothTime);
            anim.SetFloat("Speed", currentSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 dest = agent.destination;
            Gizmos.DrawLine(dest, dest + Vector3.up);
        }

        #region StateMachine
        private enum State
        {
            Invalid = -1,
            Idle,
            Move,
            Attack
        }

        private enum Stage
        {
            Enter,
            Update,
            Exit
        }

        private State currentState;
        private Stage currentStage;
        private void ChangeState(State state)
        {
            ExitState(currentState);
            currentState = state;
            EnterState(state);
            currentStage = Stage.Update;
        }

        private void EnterState(State state)
        {
            currentStage = Stage.Enter;
            switch (state)
            {
                case State.Idle:
                    desireSpeed = 0;
                    break;
                case State.Move:
                    desireSpeed = 2;
                    break;
                case State.Attack:
                    desireSpeed = 1;
                    break;
                default:
                    break;
            }
        }

        private void ExitState(State state)
        {
            currentStage = Stage.Exit;
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Move:
                    break;
                case State.Attack:
                    break;
                default:
                    break;
            }
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case State.Idle:
                    if (Search())
                    {
                        ChangeState(State.Move);
                        return;
                    }
                    break;
                case State.Move:
                    if (Moving())
                    {
                        if (CheckAttack())
                        {
                            ChangeState(State.Attack);
                            return;
                        }
                    }
                    else
                    {
                        ChangeState(State.Idle);
                        return;
                    }
                    break;
                case State.Attack:
                    if (CheckTarget())
                    {
                        Attack();
                    }
                    else
                    {
                        ChangeState(State.Idle);
                        return;
                    }
                    break;
                default:
                    break;
            }
            currentStage = Stage.Update;
        }

        private Collider possibleTarget;
        [SerializeField] private float searchRadius = 3;
        private bool Search()
        {
            Vector3 lastPosition = m_transform.position;
            Collider[] cols = Physics.OverlapSphere(lastPosition, searchRadius, targetLayer);
            Collider leastTarget = null;
            float leastDistance = Mathf.Infinity;
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag) == false) continue;
                float dist = Vector3.Distance(col.transform.position, lastPosition);
                if (dist < leastDistance)
                {
                    leastTarget = col;
                }
            }
            if(leastTarget != null)
            {
                possibleTarget = leastTarget;
                target = possibleTarget.GetComponent<IDamagable>();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Moving()
        {
            if(possibleTarget == null)
            {
                return false;
            }
            agent.SetDestination(possibleTarget.transform.position);
            return true;
        }

        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float attackOutsideDistance = 4f;
        private bool CheckAttack()
        {
            return Vector3.Distance(m_transform.position, possibleTarget.transform.position) < attackDistance;
        }

        [SerializeField] private float damage;
        [SerializeField] private float attackCD = 1;
        private float attackTime;
        private Roguelike.IDamagable target;
        private void Attack()
        {
            if (Time.time < attackTime) return;
            attackTime = Time.time + attackCD;
            desireSpeed = 0;
            //Command Fire
            anim.SetTrigger("Fire");
            Collider[] cols = Physics.OverlapSphere(m_transform.position, searchRadius, targetLayer);
            foreach (var target in cols)
            {
                if(target.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }
        }

        private bool CheckTarget()
        {
            if(possibleTarget == null)
            {
                return false;
            }
            if(Vector3.Distance(m_transform.position, possibleTarget.transform.position) < attackOutsideDistance)
            {
                return target.IsAlive();
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
