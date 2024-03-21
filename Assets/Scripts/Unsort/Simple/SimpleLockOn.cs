using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleLockOn : MonoBehaviour
    {
        [SerializeField] private Transform center;
        [SerializeField] private Transform rotator;
        public Transform target;
        [SerializeField] private float detectRadius = 10f;
        [SerializeField] private LayerMask layerMask;
        private Collider[] cols;
        private int detect;
        [SerializeField] private int size = 64;

        [SerializeField] private string targetTag = "Untagged";

        private List<Collider> possibleTarget;
        private float distance;
        private int processTarget;
        [SerializeField] private float delaySearch = 0.5f;
        private enum State
        {
            Idle,
            Search,
            Lock
        }
        [SerializeField] private State state = State.Idle;
        private void Awake()
        {
            UpdateSize(size);
            possibleTarget = new List<Collider>();
        }

        private void Start()
        {
            StartCoroutine(SearchThread());
        }

        private IEnumerator SearchThread()
        {
            while (true)
            {
                //Search();
                SearchAllocate();
                //SearchDynamic();
                yield return new WaitForSeconds(delaySearch);
            }
        }

        private void FixedUpdate()
        {
            UpdateTarget();
            Direction();
            LerpPosition();
            LockOn();
        }
        #region Search
        private void Search()
        {
            detect = Physics.OverlapSphereNonAlloc(center.position, detectRadius, cols, layerMask);
            possibleTarget.Clear();
            for (int i = 0; i < detect; i++)
            {
                if (CheckTag(cols[i]))
                {
                    possibleTarget.Add(cols[i]);
                    break;
                }
            }
            if (possibleTarget.Count == 0) return;
            distance = Mathf.Infinity;
            processTarget = -1;
            for (int i = 0; i < possibleTarget.Count; i++)
            {
                if (CheckDistance(possibleTarget[i]))
                {
                    distance = cacheDistance;
                    processTarget = i;
                }
            }
            target = possibleTarget[processTarget].transform;
            print($"Update Target {target.name} ({distance.ToString("F2")})");
            //ChangeState(State.Lock);
        }

        private void SearchAllocate()
        {
            Vector3 lastPosition = center.position;
            detect = Physics.OverlapSphereNonAlloc(lastPosition, detectRadius, cols, layerMask);
            distance = Mathf.Infinity;
            processTarget = -1;
            for (int i = 0; i < detect; i++)
            {
                if (CheckTag(cols[i]))
                {
                    cacheDistance = Vector3.Distance(cols[i].transform.position, lastPosition);
                    if (cacheDistance < distance)
                    {
                        distance = cacheDistance;
                        processTarget = i;
                    }
                }
            }
            if (processTarget == -1) return;
            target = cols[processTarget].transform;
            //print($"Update Target {target.name} ({distance.ToString("F2")})");
        }

        private void SearchDynamic()
        {
            Vector3 lastPosition = center.position;
            Collider[] cols = Physics.OverlapSphere(lastPosition, detectRadius);
            float leastDistance = Mathf.Infinity;
            Collider leastTarget = null;
            foreach (var target in cols)
            {
                if (CheckTag(target))
                {
                    float distance = Vector3.Distance(target.transform.position, lastPosition);
                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        leastTarget = target;
                    }
                }        
            }
            if (leastTarget == null) return;
            target = leastTarget.transform;
            print($"Update Target {target.name} ({distance.ToString("F2")})");
        }

        public void UpdateSize(int size)
        {
            cols = new Collider[size];
        }

        private bool CheckTag(Collider col)
        {
            return col.CompareTag(targetTag);
        }
        private float cacheDistance;
        private bool CheckDistance(Collider col)
        {
            cacheDistance = Vector3.Distance(center.position, col.transform.position);
            return cacheDistance < distance;
        }
        #endregion
        #region LockOn
        private void UpdateTarget()
        {
            if (target == null) return;
            targetPosition = target.position;
        }

        private Vector3 targetPosition;
        private Vector3 targetDirection;
        private void Direction()
        {
            targetDirection = currentPosition - rotator.position;
            targetDirection.y = 0;
        }
        private Vector3 currentPosition;
        private Vector3 velocityPosition;
        [SerializeField] private float smoothPosition = 0.5f;
        private void LerpPosition()
        {
            currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocityPosition, smoothPosition);
        }
        private void LockOn()
        {
            if (targetDirection == Vector3.zero) return;
            Quaternion rot = Quaternion.LookRotation(targetDirection, Vector3.up);
            rotator.rotation = rot;
        }
        #endregion
        private void ChangeState(State state)
        {
            this.state = state;
        }

    }
}
