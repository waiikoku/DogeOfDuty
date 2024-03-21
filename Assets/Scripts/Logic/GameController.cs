using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SimpleFollow camFollow;
        //[SerializeField] private SimpleFollow orbiter;

        [SerializeField] private float mechInterval = 60f;
        private float spawnTimer;
        private bool spawnAvailable = false;

        public struct Param
        {
            public Transform target;
            public Transform head;
        }
        public void Setup(Param param)
        {
            camFollow.SetTarget(param.target);
            //orbiter.SetTarget(param.head);
        }

        private void LateUpdate()
        {
            if (spawnAvailable) return;
            spawnTimer += Time.deltaTime;
            UIManager.Instance.IngameUI.UpdateGuage(Mathf.Clamp(spawnTimer / mechInterval,0,mechInterval));
            if (spawnTimer > mechInterval)
            {
                GameManager.Instance.SpawnMech();
                spawnAvailable = true;
            }
        }
    }
}
