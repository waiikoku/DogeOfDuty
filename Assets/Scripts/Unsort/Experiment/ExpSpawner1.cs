using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dod
{
    public class ExpSpawner1 : MonoBehaviour
    {
        //Viewport
        private readonly Vector2 leftTop = new Vector2(0, 1);
        private readonly Vector2 rightTop = new Vector2(1, 1);
        private readonly Vector2 leftBottom = new Vector2(0, 0);
        private readonly Vector2 rightBottom = new Vector2(1, 0);
        private enum Pivot
        {
            LeftBottom,
            RightBottom,
            LeftTop,
            RightTop
        }
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private float offset = 4;
        public Transform player;

        //Caches
        private Ray rayCache;
        private RaycastHit raycastHitCache;
        private Vector3 posCache;
        private GameObject mobCache;
        private Vector3 playerDir;
        private Quaternion towardPlayer;
        public float delay = 1f;
        public int count;

        private async void Start()
        {
            var player = GameManager.Instance.GetPlayer();
            while (player == null)
            {
                player = GameManager.Instance.GetPlayer();
                await Task.Delay(100);
            }
            this.player = player.transform;
            InvokeRepeating(nameof(Spawn), 1f, delay);
        }

        private void Spawn()
        {
            posCache = Position();
            playerDir = player.position - posCache;
            towardPlayer = Quaternion.LookRotation(playerDir, Vector3.up);
            mobCache = Instantiate(RandomVariant(), posCache + playerDir.normalized * offset, towardPlayer);
            mobCache.name += $"({count})";
            count++;
            SurvivalManager.Instance.AddEnemy();
        }

        private Vector3 Position()
        {
            rayCache = cam.ViewportPointToRay(RandomLerpViewpoint());
            if(Physics.Raycast(rayCache, out raycastHitCache))
            {
                return raycastHitCache.point;
            }
            return default;
        }

        private GameObject RandomVariant()
        {
            return prefabs[Random.Range(0, prefabs.Length)];
        }

        private Vector2 RandomViewpoint()
        {
            Pivot pivot = (Pivot)Random.Range(0, 4);
            switch (pivot)
            {
                case Pivot.LeftBottom:
                    return leftBottom;
                case Pivot.RightBottom:
                    return rightBottom;
                case Pivot.LeftTop:
                    return leftTop;
                case Pivot.RightTop:
                    return rightTop;
                default:
                    return default;
            }
        }

        private Vector2 RandomLerpViewpoint()
        {
            float value = Random.Range(0f, 1f);
            Pivot pivot = (Pivot)Random.Range(0, 4);
            switch (pivot)
            {
                case Pivot.LeftBottom:
                    return Vector2.Lerp(leftBottom,rightBottom,value);
                case Pivot.RightBottom:
                    return Vector2.Lerp(rightBottom,rightTop,value);
                case Pivot.LeftTop:
                    return Vector2.Lerp(leftTop,leftBottom,value);
                case Pivot.RightTop:
                    return Vector2.Lerp(rightTop, leftTop, value);
                default:
                    return default;
            }
        }
    }
}
